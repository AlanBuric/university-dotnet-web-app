using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniversityWebApp.Shared
{
    [Table("Enrollments")]
    public class Enrollment
    {
        public int Id { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Points { get; set; }
    }
}
