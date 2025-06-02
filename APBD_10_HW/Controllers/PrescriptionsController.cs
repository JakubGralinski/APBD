using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APBD_10_HW.Data;
using APBD_10_HW.Models;
using APBD_10_HW.Models.DTOs;

namespace APBD_10_HW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PrescriptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Prescriptions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prescription>>> GetPrescriptions()
        {
            return await _context.Prescriptions
                .Include(p => p.Patient)
                .Include(p => p.Doctor)
                .Include(p => p.PrescriptionMedications)
                    .ThenInclude(pm => pm.Medication)
                .ToListAsync();
        }

        // GET: api/Prescriptions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Prescription>> GetPrescription(int id)
        {
            var prescription = await _context.Prescriptions
                .Include(p => p.Patient)
                .Include(p => p.Doctor)
                .Include(p => p.PrescriptionMedications)
                    .ThenInclude(pm => pm.Medication)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prescription == null)
            {
                return NotFound();
            }

            return prescription;
        }

        // GET: api/Prescriptions/patient/5
        [HttpGet("patient/{id}")]
        public async Task<ActionResult<PatientDetailsDto>> GetPatientDetails(int id)
        {
            var patient = await _context.Patients
                .Include(p => p.Prescriptions)
                    .ThenInclude(pr => pr.Doctor)
                .Include(p => p.Prescriptions)
                    .ThenInclude(pr => pr.PrescriptionMedications)
                        .ThenInclude(pm => pm.Medication)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
            {
                return NotFound();
            }

            var patientDto = new PatientDetailsDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                BirthDate = patient.BirthDate,
                Prescriptions = patient.Prescriptions
                    .OrderBy(p => p.DueDate)
                    .Select(p => new PrescriptionDetailsDto
                    {
                        Id = p.Id,
                        Date = p.Date,
                        DueDate = p.DueDate,
                        Doctor = new DoctorDto
                        {
                            Id = p.Doctor.Id,
                            FirstName = p.Doctor.FirstName,
                            LastName = p.Doctor.LastName,
                            Email = p.Doctor.Email
                        },
                        Medications = p.PrescriptionMedications.Select(pm => new MedicationDetailsDto
                        {
                            Id = pm.Medication.Id,
                            Name = pm.Medication.Name,
                            Description = pm.Medication.Description,
                            Type = pm.Medication.Type,
                            Dose = pm.Dose,
                            Details = pm.Details
                        }).ToList()
                    }).ToList()
            };

            return patientDto;
        }

        // POST: api/Prescriptions
        [HttpPost]
        public async Task<ActionResult<Prescription>> PostPrescription(CreatePrescriptionDto prescriptionDto)
        {
            if (prescriptionDto.DueDate < prescriptionDto.Date)
            {
                return BadRequest("DueDate must be greater than or equal to Date");
            }

            if (prescriptionDto.Medications.Count > 10)
            {
                return BadRequest("A prescription can include a maximum of 10 medications");
            }

            // Check if all medications exist
            var medicationIds = prescriptionDto.Medications.Select(m => m.IdMedication).ToList();
            var existingMedications = await _context.Medications
                .Where(m => medicationIds.Contains(m.Id))
                .Select(m => m.Id)
                .ToListAsync();

            if (existingMedications.Count != medicationIds.Count)
            {
                return BadRequest("One or more medications do not exist");
            }

            // Find or create patient
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => 
                    p.FirstName == prescriptionDto.Patient.FirstName &&
                    p.LastName == prescriptionDto.Patient.LastName &&
                    p.BirthDate == prescriptionDto.Patient.BirthDate);

            if (patient == null)
            {
                patient = new Patient
                {
                    FirstName = prescriptionDto.Patient.FirstName,
                    LastName = prescriptionDto.Patient.LastName,
                    BirthDate = prescriptionDto.Patient.BirthDate
                };
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
            }

            // Create prescription
            var prescription = new Prescription
            {
                Date = prescriptionDto.Date,
                DueDate = prescriptionDto.DueDate,
                IdDoctor = prescriptionDto.IdDoctor,
                IdPatient = patient.Id,
                PrescriptionMedications = prescriptionDto.Medications.Select(m => new PrescriptionMedication
                {
                    IdMedication = m.IdMedication,
                    Dose = m.Dose,
                    Details = m.Details
                }).ToList()
            };

            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPrescription", new { id = prescription.Id }, prescription);
        }
    }
} 