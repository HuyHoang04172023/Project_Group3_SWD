namespace Project_Group3_SWD.Models
{
    public class ShippingOrderViewModel
    {
        public string FromName { get; set; }
        public string FromPhone { get; set; }
        public string FromAddress { get; set; }
        public string ToName { get; set; }
        public string ToPhone { get; set; }
        public string ToAddress { get; set; }
        public string ToWardCode { get; set; }
        public int ToDistrictId { get; set; }
        public float CodAmount { get; set; }
        public int ServiceId { get; set; }
        public string PaymentMethod { get; set; }
        public List<ItemViewModel> Items { get; set; }
    }
}
