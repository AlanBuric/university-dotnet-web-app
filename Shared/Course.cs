using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniversityWebApp.Shared
{
    [Table("Courses")]
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(127)")]
        [StringLength(127, MinimumLength = 1)]
        public string Name { get; set; }
    }
}
