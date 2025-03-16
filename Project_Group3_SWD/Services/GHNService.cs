using System.Text;
using Project_Group3_SWD.DTOs.Order;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using Project_Group3_SWD.ViewModels;
using System.Net;
using System.Net.Http.Headers;

namespace Project_Group3_SWD.Services
{
	public class GHNService : IGHNService
	{
		private readonly HttpClient _client;

		public GHNService(HttpClient client)
		{
			_client = client;
			_client.DefaultRequestHeaders.Add("token", "fa19bc40-fc2a-11ef-82e7-a688a46b55a3");
		}

		public async Task<List<OrderGHNViewModel>> GetAllOrders()
		{
			List<OrderGHNViewModel> results = new List<OrderGHNViewModel>();
			HttpResponseMessage response = await _client.GetAsync("https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/search");
			if(response.StatusCode == HttpStatusCode.OK)
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
			var requestData = new StringContent(JsonSerializer.Serialize(new { 
				order_code = orderCode 
			}), Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _client.PostAsync("https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/detail", requestData);
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
					CreateDate= createdDate,
					FinishDate = finishDate,
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
	}
}
