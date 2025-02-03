using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniversityWebApp.Shared
{
    [Table("Subjects")]
    public class Subject
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(127)")]
        [StringLength(127, MinimumLength = 1)]
        public string Name { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; } = [];
    }
}
