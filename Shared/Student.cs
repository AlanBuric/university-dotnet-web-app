using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniversityWebApp.Shared
{
    [Table("Students")]
    public class Student
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        [StringLength(50, MinimumLength = 1)]
        public string Surname { get; set; }

        [Required]
        [Column(TypeName = "char(10)")]
        [StringLength(10, MinimumLength = 10)]
        public string JMBAG { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; } = [];
    }
}
