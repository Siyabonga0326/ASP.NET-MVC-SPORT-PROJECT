using System.ComponentModel.DataAnnotations;

namespace SportSelect.Models
{
    public class Validate
    {
        [Key]
        [StringLength(50)]
        public string UserID { get; set; }

       
        [Required(ErrorMessage = "Age is required")]
        [Range(17, 25, ErrorMessage = "Age  must be between 17 and 25")]
        [Display(Name = "Age")]
        public int Age { get; set; } 
        
        [Required(ErrorMessage = "Weight is required")]
        [Display(Name = "Weight")]
        public string Weight { get; set; }

        [Required(ErrorMessage = "Strength Rate is required")]
        [Range(1, 10, ErrorMessage = "Strength Rate must be between 1 and 10")]
        [Display(Name = "Strength Rate (/10)")]
        public int Strength { get; set; }

        [Required(ErrorMessage = "Endurance Rate is required")]
        [Range(1, 10, ErrorMessage = "Endurance Rate must be between 1 and 10")]
        [Display(Name = "Endurance Rate (/10)")]
        public int Endurance { get; set; }

        [Required(ErrorMessage = "Passion Rate is required")]
        [Range(1, 10, ErrorMessage = "Passion Rate must be between 1 and 10")]
        [Display(Name = "Passion Rate (/10)")]
        public int Passion { get; set; }

        [Required(ErrorMessage = "Determination Rate is required")]
        [Range(1, 10, ErrorMessage = "Determination Rate must be between 1 and 10")]
        [Display(Name = "Determination Rate (/10)")]
        public int Determination { get; set; }

        [Required(ErrorMessage = "Team Rate is required")]
        [Range(1, 10, ErrorMessage = "Team Rate must be between 1 and 10")]
        [Display(Name = "Team Rate (/10)")]
        public int TeamWork { get; set; }

        [Display(Name = "Validation Status")]
        public string ValidationStatus { get; set; }
    }
}
