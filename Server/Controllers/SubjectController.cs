using Microsoft.AspNetCore.Mvc;
using UniversityWebApp.Database;
using UniversityWebApp.Shared;

namespace UniversityWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        [HttpGet]
        public ActionResult<IEnumerable<Subject>> GetSubjects() => Ok(_context.Subjects.ToList());

        [HttpGet("{id}")]
        public ActionResult<Subject> GetSubject(int id)
        {
            var subject = _context.Subjects.Find(id);

            return subject == null ? NotFound() : Ok(subject);
        }

        [HttpPost]
        public ActionResult<Subject> CreateSubject(Subject subject)
        {
            _context.Subjects.Add(subject);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetSubject), new { id = subject.Id }, subject);
        }

        [HttpPut]
        public IActionResult UpdateSubject(Subject subject)
        {
            var existingSubject = _context.Subjects.Find(subject.Id);

            if (existingSubject == null)
            {
                return NotFound();
            }

            existingSubject.Name = subject.Name;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSubject(int id)
        {
            var subject = _context.Subjects.Find(id);

            if (subject == null)
            {
                return NotFound();
            }

            _context.Subjects.Remove(subject);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
