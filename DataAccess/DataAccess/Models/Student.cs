using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace DataAccess.Models {
    public class Student {
        public int StudentID { get; set; }
        [Required (ErrorMessage = "Enter Given Name")]
        [Display(Name = "Given Name")]
        public string FirstName { get; set; }
        [Required (ErrorMessage = "Enter Sur Name")]
        [Display(Name = "Sur Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Father/Guardian Name")]
        public string FatherName { get; set; }
        [Required]
        [Display(Name = "School Name")]
        public string SchoolName { get; set; }

        [Required]
        [Display(Name = "Date Of Birth")]        
        public DateTime? DateOfBirth { get; set; }
        
        [Required]
        [StringLength(10)]
        [Display(Name = "Mobile Number")]
        public string Mobile { get; set; }
        
        
        [Required]
        [Display(Name = "Email ID")]
        public string EmailID { get; set; }

        [Required]        
        [Helper.FileType("jpeg", ErrorMessage = "Please upload a valid .jpeg file.")]
        public HttpPostedFileBase Photo { get; set; }
    }
}
