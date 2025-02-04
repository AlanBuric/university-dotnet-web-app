using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UniversityWebApp.Config;
using UniversityWebApp.Database;
using UniversityWebApp.Shared;

namespace UniversityWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController(ApplicationDbContext context, IOptions<AppOptions> appOptions) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        [HttpGet]
        public ActionResult<IEnumerable<Course>> GetCourses([FromQuery] string? filter, [FromQuery] int? studentId)
        {
            var query = _context.Courses.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(c => c.Name.Contains(filter));
            }
            
            if (studentId != null)
            {
                query = query.Where(c => c.Id == studentId);
            }

            return Ok(query.Take(appOptions.Value.MaxResponseRowCount).ToArray());
        }


        [HttpGet("{id}")]
        public ActionResult<Course> GetCourse(int id)
        {
            var course = _context.Courses.Find(id);

            return course == null ? NotFound() : Ok(course);
        }

        [HttpPost]
        public ActionResult<Course> CreateCourse(Course course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
        }

        [HttpPut]
        public IActionResult UpdateCourse(Course course)
        {
            var existingCourse = _context.Courses.Find(course.Id);

            if (existingCourse == null)
            {
                return NotFound();
            }

            existingCourse.Name = course.Name;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCourse(int id)
        {
            var course = _context.Courses.Find(id);

            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
