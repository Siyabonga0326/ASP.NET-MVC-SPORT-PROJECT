using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SportSelect.Models
{
    public class Student
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }


        [Required(ErrorMessage = "First Name is required")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "First Name must be between 3 and 25 characters")]
        [Display(Name = "First Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Last Name must be between 3 and 25 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(1, 99, ErrorMessage = "Age must be between 1 and 99")]
        [RegularExpression(@"^\d{1,2}$", ErrorMessage = "Age must be 1 or 2 digits")]
        [Display(Name = "Age")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be exactly 10 digits")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Phone number must contain only numeric characters")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please select a sport")]
        [Display(Name = "Select Sport")]
        public string SportSelected { get; set; }

        [Required(ErrorMessage = "Experience is required")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Experience must be between 3 and 255 characters")]
        [Display(Name = "Experience")]
        public string Experience { get; set; }

        // Add ApplicationStatus property

        [Display(Name = "Application Status")]
        public string ApplicationStatus { get; set; } = "Pending"; // Default value is "Pending"

        [Display(Name = "Validation Status")]
        public string ValidationStatus { get; set; } = "None";

        [StringLength(50)]
        [ForeignKey("Validates")]
        public string UserID { get; set; }
        public Validate Validates { get; set; }
    }
}
