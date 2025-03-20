using System.ComponentModel.DataAnnotations;

namespace Project_Group3_SWD.ViewModels
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
		public string PaymentTypeId { get; set; }
		public string Note { get; set; }
		public string RequiredNote { get; set; }
		public string FromName { get; set; }
		public string FromPhone { get; set; }
		public string FromAddress { get; set; }
		public string FromWardName { get; set; }
		public string FromDistrictName { get; set; }
		public string FromProvinceName { get; set; }
		public string ReturnPhone { get; set; }
		public string ReturnAddress { get; set; }
		public int? ReturnDistrictId { get; set; }
		public string ReturnWardCode { get; set; }
		public string ClientOrderCode { get; set; }
		public string ToName { get; set; }
		public string ToPhone { get; set; }
		public string ToAddress { get; set; }
		public string ToWardCode { get; set; }
		public string ToDistrictId { get; set; }
		public string CodAmount { get; set; }
		public string Content { get; set; }
		public string Weight { get; set; }
		public string Length { get; set; }
		public string Width { get; set; }
		public string Height { get; set; }
		public string PickStationId { get; set; }
		public string? DeliverStationId { get; set; }
		public string InsuranceValue { get; set; }
		public string ServiceId { get; set; }
		public string ServiceTypeId { get; set; }
		public string Coupon { get; set; }
		public List<string> PickShift { get; set; }
		public List<OrderItemViewModel> Items { get; set; }
	}

	public class OrderItemViewModel
	{
		public string Name { get; set; }
		public string Code { get; set; }
		public string Quantity { get; set; }
		public string Price { get; set; }
		public string Length { get; set; }
		public string Width { get; set; }
		public string Height { get; set; }
		public string Weight { get; set; }
		public ItemCategoryViewModel Category { get; set; }
	}

	public class ItemCategoryViewModel
	{
		public string Level1 { get; set; }
	}
}
