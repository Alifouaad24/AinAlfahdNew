using AinAlfahd.Data;
using AinAlfahd.Models;
using AinAlfahd.ModelsDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
    }
}
