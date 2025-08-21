using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AinAlfahd.Data;
using AinAlfahd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestNotesController : ControllerBase
    {
        MasterDBContext db;
        public RequestNotesController(MasterDBContext db)
        {
            this.db = db;
        }

        [HttpGet("{CustId}")]
        public async Task<IActionResult> GetCustomerReqNotes(int CustId)
        {
            var reqNote = await db.RequestNotes.Include(r => r.Customer).Where(r => r.CustomerId == CustId && r.Status == 0).ToListAsync(); 
            return Ok(reqNote);
        }

        [HttpPost]
        public async Task<IActionResult> AddNoteForCustomer([FromBody] RequestNoteDto model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var note = new RequestNote
            {
                CustomerId = model.CustomerId,
                Status = 0,
                Description = model.Description,
                RequestDate = model.RequestDate,
                InsertDt = DateOnly.FromDateTime(DateTime.Now),
            };

            await db.RequestNotes.AddAsync(note);
            await db.SaveChangesAsync();
            return Ok(model);
        }

        [HttpPut("{noteId}")]
        public async Task<IActionResult> MakeNoteResponed(int noteId)
        {
            var note = await db.RequestNotes.FindAsync(noteId);
            note.Status = 1;
            await db.SaveChangesAsync();
            return Ok(note);
        }
    }

    public class RequestNoteDto
    {
        [Key]
        public int CustomerId { get; set; }
        public string Description { get; set; }
        public DateOnly RequestDate { get; set; }
    }
}
