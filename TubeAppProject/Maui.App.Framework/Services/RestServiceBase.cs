
using System.Text;

namespace Maui.App.Framework.Services
{
	public class RestServiceBase
	{
		private HttpClient httpClient;
		private IBarrel cachebarrel;
		private IConnectivity connectivity;

		public RestServiceBase(IBarrel cachebarrel, IConnectivity connectivity)
		{
			
			this.cachebarrel = cachebarrel;
			this.connectivity = connectivity;
		}

		protected void SetBaseUrl(string apiBaseUrl)
		{
			this.httpClient = new()
			{
				BaseAddress = new Uri(apiBaseUrl)
			};

			httpClient.DefaultRequestHeaders.Accept.Clear();
			httpClient.DefaultRequestHeaders.Accept.Add(
				new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
		}

		protected void AddHttpHeader(string key, string value)=>
			httpClient.DefaultRequestHeaders.Add(key, value);

		protected async Task<T> GetAsync<T>(string resource, int cacheDuration = 24)
		{ 
			//Get Json data (from Cache or Web)
			var json = await GetJsonAsync(resource, cacheDuration);

			//Return the result
			return JsonSerializer.Deserialize<T>(json);
		}

		private async Task<string> GetJsonAsync(string resource, int cacheDuration=24)
		{
			var cleanCacheKey = resource.CleanCacheKey();

			//Check if Cache Barrel is enabled
			if (cachebarrel is not null)
			{
				//Try Get data from Cache
				var cacheData = cachebarrel.Get<string>(cleanCacheKey);

				if (cacheDuration > 0 && cacheData is not null && !cachebarrel.IsExpired(cleanCacheKey))
					return cacheData;

				//Check for internet connection and return cached data  if possible
				if (connectivity.NetworkAccess != NetworkAccess.Internet)
				{
					return cacheData is not null ? cacheData : throw new InternetConnectionException();
				}

				
			}

			//No Cache Found, or Cached data was not required, or Internet connection is also available
			if (this.connectivity.NetworkAccess != NetworkAccess.Internet)
				throw new InternetConnectionException();
		

			//Extract response from URI
			var response = await httpClient.GetAsync(new Uri(httpClient.BaseAddress, resource));

			//Check for valid response
			response.EnsureSuccessStatusCode();

			//Read Response
			string json = await response.Content.ReadAsStringAsync();

			//Save to Cache if required
			if (cacheDuration > 0 && cachebarrel is not null)
			{
				try
				{
					cachebarrel.Add(cleanCacheKey, json, TimeSpan.FromHours(cacheDuration));
				}
				catch (Exception)
				{

				}
			}

			//Return the result
			return json;
		}

		protected async Task<HttpResponseMessage> PostAsync<T>(string uri, T payload)
		{ 
			var dataToPost = JsonSerializer.Serialize(payload);
			var content = new StringContent(dataToPost, Encoding.UTF8, "application/json");
			var response = await httpClient.PostAsync(new Uri(httpClient.BaseAddress, uri), content);

			response.EnsureSuccessStatusCode();
			return response;
		}

		protected async Task<HttpResponseMessage> PutAsync<T>(string uri, T payload)
		{
			var dataToPost = JsonSerializer.Serialize(payload);
			var content = new StringContent(dataToPost, Encoding.UTF8, "application/json");
			var response = await httpClient.PutAsync(new Uri(httpClient.BaseAddress, uri), content);

			response.EnsureSuccessStatusCode();
			return response;
		}

		protected async Task<HttpResponseMessage> DeleteAsync(string uri)
		{ 
			HttpResponseMessage response = await httpClient.DeleteAsync(new Uri(httpClient.BaseAddress, uri));

			response.EnsureSuccessStatusCode();

			return response;
		}
	}
}
