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
            var reciepts = await _db.Reciepts.ToListAsync();
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
            var user = HttpContext?.User?.Identity?.Name;
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var recipt = new Reciept
            {
                Cost = model.Weight * 5,
                Currency = "$",
                CustomerId = model.CustomerId,
                InsertBy = user ?? userId.ToString(),
                InsertDate = DateTime.Now,
                Weight = model.Weight,
                DisCount = model.DisCount,
                SellingCurrency = "IQ",
                RecieptDate = model.RecieptDate,
                SellingPrice = model.Weight * 8,
                IsFinanced = model.IsFinanced,
                SellingDisCount = model.SellingDisCount,


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

            rec.Cost = model.Weight * 5;
            rec.SellingPrice = model.Weight * 8;
            _db.Entry(rec).CurrentValues.SetValues(model);
            await _db.SaveChangesAsync();

            return Ok(rec);
        }


        [HttpDelete]
        public async Task<IActionResult> Deletedata(int id)
        {
            var rec = await _db.Reciepts.FindAsync(id);
            if (rec == null)
                return NotFound();

            _db.Reciepts.Remove(rec);
            await _db.SaveChangesAsync();
            return Ok($"Reciept {rec.RecieptId} deleted successfuly");

        }
    }
}
