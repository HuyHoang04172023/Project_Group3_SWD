using System.ComponentModel.DataAnnotations;

namespace A_LIÊM_SHOP.ViewModels
{
    public class OrderViewModel
    {
        [Required(ErrorMessage = "Please enter your name")]
        [StringLength(50)]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Please enter your address")]
        [StringLength(200)]
        public string? Address { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(0)\d{9}$", ErrorMessage = "Not a valid phone number: 0123123123")]
        public string? Phone { get; set; }
    }
}
