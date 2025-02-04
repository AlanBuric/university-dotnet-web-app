using Microsoft.AspNetCore.Mvc;
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
        public ActionResult<IEnumerable<EnrollmentResponse>> GetEnrollments([FromQuery] int? studentId, [FromQuery] int? courseId)
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

            var enrollmentResponses = (from e in query
                                       join s in _context.Students on e.StudentId equals s.Id
                                       join c in _context.Courses on e.CourseId equals c.Id
                                       select new EnrollmentResponse
                                       {
                                           Id = e.Id,
                                           StudentId = e.StudentId,
                                           CourseId = e.CourseId,
                                           Points = e.Points,
                                           Student = s,
                                           Course = c
                                       }).ToArray();

            return Ok(enrollmentResponses);
        }

        [HttpGet("{id}")]
        public ActionResult<EnrollmentResponse> GetEnrollment(int id)
        {
            var enrollment = _context.Enrollments.Find(id);

            if (enrollment == null)
            {
                return NotFound();
            }

            var student = _context.Students.Find(enrollment.StudentId);
            var course = _context.Courses.Find(enrollment.CourseId);

            if (student == null || course == null)
            {
                return NotFound("Belonging student or course don't exist for this enrollment.");
            }

            var enrollmentResponse = new EnrollmentResponse
            {
                Id = enrollment.Id,
                StudentId = enrollment.StudentId,
                CourseId = enrollment.CourseId,
                Points = enrollment.Points,
                Student = student,
                Course = course
            };

            return Ok(enrollmentResponse);
        }

        [HttpPost]
        public IActionResult CreateEnrollment(Enrollment enrollment)
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

            return Created();
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

            return Ok();
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

            return Ok();
        }
    }
}
