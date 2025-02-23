using AinAlfahd.Data;
using AinAlfahd.Models;
using AinAlfahd.ModelsDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using static OpenQA.Selenium.PrintOptions;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecieptController : ControllerBase
    {
        MasterDBContext _db;
        public RecieptController(MasterDBContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reciepts = await _db.Reciepts.Include(r => r.Customer).Include(r => r.ShippingBatch).ToListAsync();
            return Ok(reciepts);
        }

        [HttpGet("GetAllShippingNull")]
        public async Task<IActionResult> GetAllShippingNull()
        {
            var reciepts = await _db.Reciepts.Include(r => r.Customer)
                .Where(r => r.ShippingBatchId == null)
                .ToListAsync();
            return Ok(reciepts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var reciept = await _db.Reciepts.Include(r => r.Customer).Include(r => r.ShippingBatch)
                .FirstOrDefaultAsync(r => r.RecieptId == id);
            return Ok(reciept);
        }

		[HttpGet("GetBySomeProperities/{weight}")]
		public async Task<IActionResult> GetBySomeProperities(decimal weight)
		{
			var reciept = await _db.Reciepts.Include(r => r.Customer).Include(r => r.ShippingBatch)
				.Where(r => r.Weight == weight).ToListAsync();
			return Ok(reciept);
		}


		[HttpGet("GetLastFiveRecords")]
        public async Task<IActionResult> GetLastFiveRecords()
        {
            var reciepts = await _db.Reciepts.Include(r => r.Customer).ToListAsync();

            if (reciepts.Count > 5)
            {
                var actualRec = reciepts.Skip(reciepts.Count - 5).Take(5).ToList();
                return Ok(actualRec);
            }

            return Ok(reciepts);
        }

        [HttpGet("GetByCustomerIdOrCost")]
        public async Task<IActionResult> GetByCustomerIdOrCost(string word)
        {
            var reciept = await _db.Reciepts.FirstOrDefaultAsync(r => r.CustomerId.ToString() == word || r.Cost.ToString() == word);
            return Ok(reciept);
        }

        [HttpPost]
        public async Task<IActionResult> AddData(RecirptDto model)
        {
            var user = HttpContext?.User?.Identity?.Name ?? "Not Found";
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rec = await _db.Reciepts.FirstOrDefaultAsync(a => a.Weight == model.Weight && a.RecieptDate == model.RecieptDate && a.CustomerId == model.CustomerId);

            if (rec != null)
            {
                return BadRequest("الإيصال موجود مسبقا");
            }

            var recipt = new Reciept
            {
                Cost = model.Cost,
                Currency = "$",
                CustomerId = model.CustomerId,
                DisCount = model.DisCount,
                InsertBy = "Anyy one",
                IsFinanced = model.IsFinanced,
                InsertDate = DateTime.Now,
                SellingCurrency = "IQ",
                RecieptDate = model.RecieptDate,
                SellingDisCount = model.SellingDisCount,
                SellingPrice = model.SellingPrice,
                Weight = model.Weight,
                TotalPriceFromCust = Math.Ceiling(model.TotalPriceFromCust),
                CurrentState = model.CurrentState,
                Notes = model.Notes,
                ShippingBatchId = model.ShippingBatchId,
                CostIQ = model.CostIQ,
                SellingUSD = model.SellingUSD,


            };
            await _db.Reciepts.AddAsync(recipt);
            await _db.SaveChangesAsync();

            return Ok(recipt);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateData(int id, [FromBody] RecirptDto model)
        {
            var rec = await _db.Reciepts.FindAsync(id);

            if (rec == null)
                return BadRequest(ModelState);

            rec.Cost = model.Cost;
            rec.Currency = "$";
            rec.CustomerId = model.CustomerId;
            rec.DisCount = model.DisCount;
            rec.InsertBy = "Anyy one";
            rec.IsFinanced = model.IsFinanced;
            rec.InsertDate = DateTime.Now;
            rec.SellingCurrency = "IQ";
            rec.RecieptDate = model.RecieptDate;
            rec.SellingDisCount = model.SellingDisCount;
            rec.SellingPrice = model.SellingPrice;
            rec.Weight = model.Weight;
            rec.TotalPriceFromCust = Math.Ceiling(model.TotalPriceFromCust);
            rec.CurrentState = true;
            rec.CostIQ = model.CostIQ;
            rec.SellingUSD = model.SellingUSD;

            _db.Entry(rec).CurrentValues.SetValues(model);
            await _db.SaveChangesAsync();

            return Ok(rec);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletedata(int id)
        {
            var rec = await _db.Reciepts.FindAsync(id);
            if (rec == null)
                return NotFound();

            _db.Reciepts.Remove(rec);
            await _db.SaveChangesAsync();
            return Ok($"Reciept {rec.RecieptId} deleted successfuly");

        }

        [HttpGet("SerachByDateApi/{from}/{to}")]
        public async Task<IActionResult> SerachByDateApi(string from, string to)
        {
            var recipts = await _db.Reciepts
                .Where(c => c.CurrentState == true & c.RecieptDate >= DateTime.Parse(from) & c.RecieptDate <= DateTime.Parse(to))
                .OrderBy(r => r.RecieptDate)
                .Include(r => r.Customer).ToListAsync();


            return Ok(recipts);
        }


        /// ///////////////////////////For Print Recieps ////////////////////////////////////////////////////////////////////
        /// 

        [HttpGet("GeneratePdf/{shippingBatchId?}")]
        public async Task<IActionResult> GeneratePdf(int? shippingBatchId)
        {
            var pdf = await CreatePDF(shippingBatchId);
            return pdf;
        }

        public async Task<IActionResult> CreatePDF(int? shippingBatchId)
        {
            var recipts = new List<Reciept>();
            if (shippingBatchId == null)
            {
                recipts = await _db.Reciepts
                    .Include(o => o.ShippingBatch).Include(o => o.Customer)
                    .OrderByDescending(o => o.ShippingBatch.ArrivelDate)
                    .ToListAsync();
            }
            else
            {
                recipts = await _db.Reciepts.Include(o => o.Customer).Include(o => o.ShippingBatch).OrderByDescending(o => o)
                .Where(o => o.ShippingBatchId == shippingBatchId).ToListAsync();

            }
            var shipping = await _db.ShippingBatchs.Where(s => s.ShippingBatchId == shippingBatchId).FirstOrDefaultAsync();
            var shippingDate = shipping?.ArrivelDate?.ToString("yyyy/MM/dd");


            var totalCostInDollar = recipts.Sum(p => p.Cost);
            var totalSellingPrice = recipts.Sum(p => p.SellingPrice);
            int count = 0;

            var pdfDocument = Document.Create(async container =>
            {

                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Column(header =>
                        {
                            header.Item().AlignRight().PaddingBottom(20)
                                .Text($"({DateTime.Now:yyyy-MM-dd hh:mm tt}): تاريخ الطباعة").FontSize(10).Bold();
                            if (shippingBatchId == null)
                            {
                                header.Item().Border(1).AlignCenter().Text($"تقرير الإيصالات العام").FontSize(20).Bold();
                            }
                            else
                            {
                                header.Item().Border(1).AlignCenter().Padding(10).Text($"تقرير الإيصالات لوجبة  {shippingDate} م ").FontSize(20).Bold();
                            }
                        });

                    page.Content().Column(content =>
                    {
                        content.Item().AlignCenter().Text("").FontSize(12).Bold();
                        content.Item().AlignCenter().Text("").FontSize(12).Bold();
                        content.Item().Table(table =>
                        {
                            // استخدام RelativeColumn لجعل الأعمدة تأخذ عرض الصفحة
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(); // عمود لتاريخ الإيصال
                                columns.RelativeColumn(); // عمود للمبيع بالدينار
                                columns.RelativeColumn(); // عمود للتكلفة بالدولار
                                columns.RelativeColumn(); // عمود للوزن
                                columns.RelativeColumn(); // عمود لاسم العميل
                            });

                            table.Header(header =>
                            {
                                header.Cell().Border(1).AlignCenter().Padding(3).Text("تاريخ الإيصال").Bold();
                                header.Cell().Border(1).AlignCenter().Padding(3).Text("المبيع بالدينار").Bold();
                                header.Cell().Border(1).AlignCenter().Padding(3).Text("التكلفة بالدولار").Bold();
                                header.Cell().Border(1).AlignCenter().Padding(3).Text("الوزن").Bold();
                                header.Cell().Border(1).AlignCenter().Padding(3).Text("اسم العميل").Bold();
                            });

                            foreach (var recipt in recipts)
                            {
                                table.Cell().Border(1).AlignCenter().Padding(3).Text(recipt.RecieptDate.ToString("yyyy-MM-dd"));
                                table.Cell().Border(1).AlignCenter().Padding(3).Text(recipt.SellingPrice.ToString("N0") + "IQ");
                                table.Cell().Border(1).AlignCenter().Padding(3).Text(recipt.Cost.ToString("N0") + "$");
                                table.Cell().Border(1).AlignCenter().Padding(3).Text(recipt.Weight + "KG");
                                table.Cell().Border(1).AlignCenter().Padding(3).Text(recipt.Customer.CustName);
                                count++;
                            }
                        });

                        content.Item().PaddingTop(10);

                        content.Item().PaddingTop(20).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(); // العمود الأول: مجموع الكمية
                                columns.RelativeColumn(); // العمود الثاني: المجموع العام
                                columns.RelativeColumn(); // العمود الثالث: عدد الإيصالات
                            });

                            table.Header(header =>
                            {
                                header.Cell().AlignCenter().Padding(3).Text("مجموع التكلفة").FontSize(12).Bold();
                                header.Cell().AlignCenter().Padding(3).Text("المجموع البيع").FontSize(12).Bold();
                                header.Cell().AlignCenter().Padding(3).Text("عدد الإيصالات").FontSize(12).Bold();
                            });

                            table.Cell().Border(1).AlignCenter().Padding(3).Text($"${totalCostInDollar}").FontSize(12);
                            table.Cell().Border(1).AlignCenter().Padding(3).Text($"IQ{totalSellingPrice}").FontSize(12);
                            table.Cell().Border(1).AlignCenter().Padding(3).Text($"{count}").FontSize(12);
                        });

                        content.Item().PaddingTop(20).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(); // عمود الفاصل
                                columns.RelativeColumn(); // عمود الفاصل الثاني
                            });

                            table.Cell().Border(0).PaddingTop(10).AlignCenter().Text($"----------------------------------------------------------------------------")
                                .FontSize(12);
                        });
                    });
                });

            });
            var pdfData = pdfDocument.GeneratePdf();
            return File(pdfData, "application/pdf", "Report.pdf");
        }
    }
}
