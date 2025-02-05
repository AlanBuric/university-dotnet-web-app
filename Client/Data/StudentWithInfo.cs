using UniversityWebApp.Shared;

namespace UniversityWebApp.Data
{
    public class StudentWithInfo
    {
        public Student Student { get; set; } = new Student();
        public EnrollmentResponse[]? Enrollments { get; set; } = null;
        public bool Expanded { get; set; } = false;
    }
}
