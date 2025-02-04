using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UniversityWebApp.Config;
using UniversityWebApp.Database;
using UniversityWebApp.Shared;

namespace UniversityWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController(ApplicationDbContext context, IOptions<AppOptions> appOptions) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        [HttpGet]
        public ActionResult<IEnumerable<Enrollment>> GetEnrollments([FromQuery] int? studentId, [FromQuery] int? courseId)
        {
            var query = _context.Enrollments.AsQueryable();

            if (studentId != null)
            {
                query = query.Where(e => e.StudentId == studentId);
            }

            if (courseId != null)
            {
                query = query.Where(e => e.CourseId == courseId);
            }

            return Ok(query.ToArray());
        }

        [HttpGet("{id}")]
        public ActionResult<Enrollment> GetEnrollment(int id)
        {
            var enrollment = _context.Enrollments.FirstOrDefault(e => e.Id == id);

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

            bool courseExists = _context.Courses.Any(s => s.Id == enrollment.CourseId);

            if (!courseExists)
            {
                return BadRequest($"Course with ID {enrollment.CourseId} does not exist.");
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
            existingEnrollment.CourseId = enrollment.CourseId;
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
