using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityWebApp.Shared
{
    public class EnrollmentResponse : Enrollment
    {
        [NotMapped]
        public Student Student { get; set; } = default!;

        [NotMapped]
        public Course Course { get; set; } = default!;
    }
}
