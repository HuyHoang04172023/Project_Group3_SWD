namespace A_LIÊM_SHOP.ViewModels
{
    public class ShippingOrderModel
    {
        public string to_name { get; set; }
        public string to_phone { get; set; }
        public string to_address { get; set; }
        public string to_ward_code { get; set; }
        public int to_district_id { get; set; }
        public float cod_amount { get; set; }
        public int service_id { get; set; }
        public string payment_method { get; set; }
    }
}
