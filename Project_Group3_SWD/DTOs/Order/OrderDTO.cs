namespace Project_Group3_SWD.DTOs.Order
{
    public class OrderDTO
    {
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
        public List<OrderItemDTO> Items { get; set; }
    }

    public class OrderItemDTO
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public ItemCategoryDTO Category { get; set; }
    }

    public class ItemCategoryDTO
    {
        public string Level1 { get; set; }
    }
}
