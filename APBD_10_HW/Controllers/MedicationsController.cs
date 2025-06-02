using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APBD_10_HW.Data;
using APBD_10_HW.Models;

namespace APBD_10_HW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MedicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Medications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medication>>> GetMedications()
        {
            return await _context.Medications.ToListAsync();
        }

        // GET: api/Medications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Medication>> GetMedication(int id)
        {
            var medication = await _context.Medications.FindAsync(id);

            if (medication == null)
            {
                return NotFound();
            }

            return medication;
        }

        // POST: api/Medications
        [HttpPost]
        public async Task<ActionResult<Medication>> PostMedication(Medication medication)
        {
            _context.Medications.Add(medication);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMedication", new { id = medication.Id }, medication);
        }
    }
} 