using Project_Group3_SWD.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Project_Group3_SWD.Proxy
{
    public class GHNService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiToken = "acda664f-f3f6-11ef-bb13-2a342a4da1fb";
        private readonly string _shopId = "196050";

        public GHNService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Token", _apiToken);
            _httpClient.DefaultRequestHeaders.Add("ShopId", _shopId);
        }

        public async Task<JObject> GetProvincesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/province");
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                return JObject.Parse(jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting provinces: {ex.Message}");
                return new JObject();
            }
        }

        public async Task<JObject> GetDistrictsAsync(int provinceId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/district?province_id={provinceId}");
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                return JObject.Parse(jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting districts: {ex.Message}");
                return new JObject();
            }
        }

        public async Task<JObject> GetWardsAsync(int districtId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/ward?district_id={districtId}");
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                return JObject.Parse(jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting wards: {ex.Message}");
                return new JObject();
            }
        }

        public async Task<JObject> GetShopsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shop/all");
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                return JObject.Parse(jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting shops: {ex.Message}");
                return new JObject();
            }
        }

        public async Task<JObject> GetAvailableServicesAsync(int shopId, int fromDistrictId, int toDistrictId)
        {
            try
            {
                var requestData = new
                {
                    shop_id = shopId,
                    from_district = fromDistrictId,
                    to_district = toDistrictId
                };

                var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/available-services", content);
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                return JObject.Parse(jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting available services: {ex.Message}");
                return new JObject();
            }
        }

        public async Task<JObject> GetShippingFeeAsync(int shopId, int fromDistrictId, string toWardCode, int toDistrictId, int serviceId)
        {
            try
            {
                var requestData = new
                {
                    shop_id = shopId,
                    from_district_id = fromDistrictId,
                    to_district_id = toDistrictId,
                    service_id = serviceId,
                    to_ward_code = toWardCode,
                    weight = 200,
                    items = new[]
                    {
                        new
                        {
                            name = "Product 1",
                            quantity = 1,
                            height = 200,
                            weight = 200,
                            length = 200,
                            width = 200
                        }
                    },
                };

                var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee", content);
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                return JObject.Parse(jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting shipping fee: {ex.Message}");
                return new JObject();
            }
        }

        public async Task<JObject> CreateShippingOrderAsync(string from_name, string from_phone, 
            string from_address, string to_name, 
            string to_phone, string to_address, string to_ward_code, 
            int to_district_id, int cod_amount, int service_id, List<ItemViewModel> items)
        {

            try
            {
                var requestData = new
                {
                    payment_type_id = 2, // 1: COD, 2: Prepaid
                    required_note = "CHOXEMHANGKHONGTHU",
                    from_name = from_name,
                    from_phone = from_phone,
                    from_address = from_address,
                    to_name = to_name,
                    to_phone = to_phone,
                    to_address = to_address,
                    to_ward_code = to_ward_code,
                    to_district_id = to_district_id,
                    cod_amount = cod_amount,
                    weight = 200,
                    service_id = service_id,
                    service_type_id = 2,
                    items = items.Select(item => new
                    {
                        name = item.Name,
                        quantity = item.Quantity,
                        price = item.Price,
                        weight = item.Weight 
                    }).ToArray(),
                };

                var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/create", content);
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                return JObject.Parse(jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new JObject();
            }
        }
    }
}
