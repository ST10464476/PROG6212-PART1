using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace RaceDay.Web.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiService(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddAuthenticationCookie()
        {
            var cookie = _httpContextAccessor.HttpContext?
                .Session.GetString("ApiCookie");

            if (!string.IsNullOrEmpty(cookie))
            {
                _httpClient.DefaultRequestHeaders.Remove("Cookie");
                _httpClient.DefaultRequestHeaders.Add("Cookie", cookie);
            }
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            AddAuthenticationCookie();
            return await _httpClient.GetFromJsonAsync<T>(endpoint);
        }

        public async Task<HttpResponseMessage> PostAsync<T>(
            string endpoint, T data)
        {
            AddAuthenticationCookie();
            return await _httpClient.PostAsJsonAsync(endpoint, data);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(
            string endpoint, T data)
        {
            AddAuthenticationCookie();
            return await _httpClient.PutAsJsonAsync(endpoint, data);
        }

        public async Task<HttpResponseMessage> DeleteAsync(
            string endpoint)
        {
            AddAuthenticationCookie();
            return await _httpClient.DeleteAsync(endpoint);
        }

        // Upload an image to the API
        public async Task<HttpResponseMessage> UploadFileAsync(
            string endpoint, IFormFile file)
        {
            AddAuthenticationCookie();

            using var content = new MultipartFormDataContent();
            using var stream = file.OpenReadStream();
            using var fileContent = new StreamContent(stream);

            fileContent.Headers.ContentType =
                new MediaTypeHeaderValue(file.ContentType);

            content.Add(fileContent, "image", file.FileName);

            return await _httpClient.PostAsync(endpoint, content);
        }
    }
}