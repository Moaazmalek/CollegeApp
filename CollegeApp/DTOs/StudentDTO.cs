using CollegeApp.Validators;
using System.ComponentModel.DataAnnotations;

namespace CollegeApp.DTOs
{
    public class StudentDTO 
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Student name is required")]
        [StringLength(30)]
        public required  string Name { get; set; }
        [EmailAddress(ErrorMessage ="Please enter valid email address")]
        public required string Email { get; set; }
        public required string Address { get; set; }
        //[DateCheck]
        //public DateTime AdmissionDate { get; set; }
        public  DateTime DOB { get; set; }
    }
}
   