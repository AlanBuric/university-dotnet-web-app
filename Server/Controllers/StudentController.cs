using Microsoft.AspNetCore.Mvc;
using UniversityWebApp.Database;
using UniversityWebApp.Shared;

namespace UniversityWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetStudents([FromQuery] string? filter)
        {
            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(s => s.FirstName.Contains(filter)
                                      || s.LastName.Contains(filter)
                                      || s.JMBAG.Contains(filter));
            }

            return Ok(query.ToArray());
        }

        [HttpGet("{id}")]
        public ActionResult<Student> GetStudent(int id)
        {
            var student = _context.Students.Find(id);

            return student == null ? NotFound() : Ok(student);
        }

        [HttpPost]
        public ActionResult<Student> CreateStudent(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
        }

        [HttpPut]
        public IActionResult UpdateStudent(Student student)
        {
            var existingStudent = _context.Students.Find(student.Id);

            if (existingStudent == null)
            {
                return NotFound();
            }

            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            existingStudent.JMBAG = student.JMBAG;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var student = _context.Students.Find(id);

            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
