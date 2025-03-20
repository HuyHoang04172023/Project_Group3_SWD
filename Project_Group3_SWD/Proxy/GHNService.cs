using Project_Group3_SWD.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Project_Group3_SWD.ViewModels;
using System.Net;
using System.Text.Json;

namespace Project_Group3_SWD.Proxy
{
    public class GHNService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiToken = "fa19bc40-fc2a-11ef-82e7-a688a46b55a3";
        private readonly string _shopId = "196112";

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


		public async Task<List<OrderGHNViewModel>> GetAllOrders()
		{
			List<OrderGHNViewModel> results = new List<OrderGHNViewModel>();
			HttpResponseMessage response = await _httpClient.GetAsync("https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/search");
			if (response.StatusCode == HttpStatusCode.OK)
			{
				string result = await response.Content.ReadAsStringAsync();
				JsonElement doc = JsonDocument.Parse(result).RootElement;
				JsonElement data = doc.GetProperty("data");
				JsonElement orderList = data.GetProperty("data");
				foreach (JsonElement item in orderList.EnumerateArray())
				{
					string createDate = item.GetProperty("created_date").GetString() ?? "";
					DateTime createdDate = DateTime.Parse(createDate);
					string toDate = item.GetProperty("finish_date").GetString() ?? "";
					DateTime finishDate = DateTime.Parse(createDate);
					results.Add(new OrderGHNViewModel
					{
						OrderCode = item.TryGetProperty("order_code", out var orderCodeProp) && orderCodeProp.ValueKind != JsonValueKind.Undefined
				? orderCodeProp.GetString() ?? ""
				: "",

						CreateDate = createdDate,
						FinishDate = finishDate,

						ToName = item.TryGetProperty("to_name", out var toNameProp) && toNameProp.ValueKind != JsonValueKind.Undefined
			  ? toNameProp.GetString() ?? ""
			  : "",

						ToPhone = item.TryGetProperty("to_phone", out var toPhoneProp) && toPhoneProp.ValueKind != JsonValueKind.Undefined
			  ? toPhoneProp.GetString() ?? ""
			  : "",

						ToAddress = item.TryGetProperty("to_address", out var toAddressProp) && toAddressProp.ValueKind != JsonValueKind.Undefined
				? toAddressProp.GetString() ?? ""
				: "",

						Status = item.TryGetProperty("status", out var statusProp) && statusProp.ValueKind != JsonValueKind.Undefined
			 ? statusProp.ToString()
			 : "",

						CodAmount = item.TryGetProperty("cod_amount", out var codAmountProp) && codAmountProp.ValueKind != JsonValueKind.Undefined
				? codAmountProp.GetDecimal()
				: 0,

						RequiredNote = item.TryGetProperty("required_note", out var requiredNoteProp) && requiredNoteProp.ValueKind != JsonValueKind.Undefined
				   ? requiredNoteProp.GetString() ?? ""
				   : "",

						ProductGHNs = item.TryGetProperty("items", out var itemsProp) && itemsProp.ValueKind != JsonValueKind.Undefined
				  ? GetProducts(itemsProp) ?? new List<ProductGHNViewModel>()
				  : new List<ProductGHNViewModel>()
					});
				}
			}

			return results;
		}


		public async Task<OrderGHNViewModel> GetOrderDetailByOrderCode(string orderCode)
		{
			var requestData = new StringContent(JsonSerializer.Serialize(new
			{
				order_code = orderCode
			}), Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _httpClient.PostAsync("https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/detail", requestData);
			if (response.StatusCode == HttpStatusCode.OK)
			{
				string result = await response.Content.ReadAsStringAsync();
				JsonElement doc = JsonDocument.Parse(result).RootElement;
				JsonElement data = doc.GetProperty("data");
				string createDate = data.GetProperty("created_date").GetString() ?? "";
				DateTime createdDate = DateTime.Parse(createDate);
				string toDate = data.GetProperty("finish_date").GetString() ?? "";
				DateTime finishDate = DateTime.Parse(createDate);
				return new OrderGHNViewModel
				{
					CreateDate = createdDate,
					FinishDate = finishDate,
                    OrderCode = data.GetProperty("order_code").GetString() ?? "",
					ToName = data.GetProperty("to_name").GetString() ?? "",
					ToPhone = data.GetProperty("to_phone").GetString() ?? "",
					ToAddress = data.GetProperty("to_address").GetString() ?? "",
					Status = data.GetProperty("status").ToString(),
					CodAmount = data.GetProperty("cod_amount").GetDecimal(),
					RequiredNote = data.GetProperty("required_note").GetString() ?? "",
					ProductGHNs = GetProducts(data.GetProperty("items")) ?? new List<ProductGHNViewModel>()
				};
			}
			return new OrderGHNViewModel();
		}

		private List<ProductGHNViewModel> GetProducts(JsonElement data)
		{
			List<ProductGHNViewModel> results = new List<ProductGHNViewModel>();
			foreach (JsonElement item in data.EnumerateArray())
			{
				results.Add(new ProductGHNViewModel
				{
					Name = item.TryGetProperty("name", out var nameProp) && nameProp.ValueKind != JsonValueKind.Undefined
		   ? nameProp.GetString() ?? ""
		   : "",

					Quantity = item.TryGetProperty("quantity", out var quantityProp) && quantityProp.ValueKind != JsonValueKind.Undefined
			   ? quantityProp.GetInt32()
			   : 0,

					Code = item.TryGetProperty("code", out var codeProp) && codeProp.ValueKind != JsonValueKind.Undefined
		   ? codeProp.GetString() ?? ""
		   : "",

					Weight = item.TryGetProperty("weight", out var weightProp) && weightProp.ValueKind != JsonValueKind.Undefined
			 ? weightProp.GetInt32()
			 : 0
				});
			}
			return results;
		}

		public async Task UpdateOrderNoteById(string orderCode, string note)
		{
			var requestData = new StringContent(JsonSerializer.Serialize(new
			{
				note = note,
				order_code = orderCode

			}), Encoding.UTF8, "application/json");

			HttpResponseMessage response = await _httpClient.PostAsync("https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/update", requestData);
			if (response.StatusCode == HttpStatusCode.OK)
			{
				string result = await response.Content.ReadAsStringAsync();
				JsonElement doc = JsonDocument.Parse(result).RootElement;
			}
		}

		public async Task CancelOrderById(string orderCode)
		{
			var requestData = new StringContent(JsonSerializer.Serialize(new
			{
				order_codes = new[] { orderCode }
			}), Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _httpClient.PostAsync("https://dev-online-gateway.ghn.vn/shiip/public-api/v2/switch-status/cancel", requestData);
			if (response.StatusCode == HttpStatusCode.OK)
			{
				string result = await response.Content.ReadAsStringAsync();
				JsonElement doc = JsonDocument.Parse(result).RootElement;
			}
		}
	}
}
