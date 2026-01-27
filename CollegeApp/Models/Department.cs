namespace CollegeApp.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentDescription { get; set; }

        public virtual ICollection<Student> Students { get; set; }



    }
}
