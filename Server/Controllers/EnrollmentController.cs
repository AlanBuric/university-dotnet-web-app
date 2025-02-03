using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityWebApp.Database;
using UniversityWebApp.Shared;

namespace UniversityWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        [HttpGet]
        public ActionResult<IEnumerable<Enrollment>> GetEnrollments()
        {
            var enrollments = _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Subject)
                .ToList();

            return Ok(enrollments);
        }

        [HttpGet("{id}")]
        public ActionResult<Enrollment> GetEnrollment(int id)
        {
            var enrollment = _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Subject)
                .FirstOrDefault(e => e.Id == id);

            return enrollment == null ? NotFound() : Ok(enrollment);
        }

        [HttpPost]
        public ActionResult<Enrollment> CreateEnrollment(Enrollment enrollment)
        {
            bool studentExists = _context.Students.Any(s => s.Id == enrollment.StudentId);

            if (!studentExists)
            {
                return BadRequest($"Student with ID {enrollment.StudentId} does not exist.");
            }

            bool subjectExists = _context.Subjects.Any(s => s.Id == enrollment.SubjectId);

            if (!subjectExists)
            {
                return BadRequest($"Subject with ID {enrollment.SubjectId} does not exist.");
            }

            _context.Enrollments.Add(enrollment);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetEnrollment), new { id = enrollment.Id }, enrollment);
        }


        [HttpPut]
        public IActionResult UpdateEnrollment(Enrollment enrollment)
        {
            var existingEnrollment = _context.Enrollments.Find(enrollment.Id);

            if (existingEnrollment == null)
            {
                return NotFound();
            }

            existingEnrollment.StudentId = enrollment.StudentId;
            existingEnrollment.SubjectId = enrollment.SubjectId;
            existingEnrollment.Points = enrollment.Points;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEnrollment(int id)
        {
            var enrollment = _context.Enrollments.Find(id);

            if (enrollment == null)
            {
                return NotFound();
            }

            _context.Enrollments.Remove(enrollment);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
