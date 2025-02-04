using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniversityWebApp.Shared
{
    [Table("Enrollments")]
    public class Enrollment
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public int StudentId { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Points { get; set; }
    }
}
