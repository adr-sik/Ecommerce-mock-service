using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Shared.Models.DTOs;

namespace Client.Services
{
    //TODO Consider setting methods here to protected
    public abstract class ServiceBase<T>
    {
        protected readonly HttpClient _http;
        protected readonly JsonSerializerOptions _jsonOptions;

        protected abstract string Endpoint { get; }

        protected ServiceBase(HttpClient http, JsonSerializerOptions jsonOptions)
        {
            _http = http;
            _jsonOptions = jsonOptions;
        }

        protected async Task<List<T>> GetAllAsync(string? query = "", string? sort = "")
        {
            try
            {
                var items = await _http.GetFromJsonAsync<List<T>>($"{Endpoint}{query}{sort}", _jsonOptions);
                Console.WriteLine($"Fetched {items?.Count ?? 0} items from {Endpoint}");
                return items ?? new List<T>();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching items from {Endpoint}: {ex.Message}");
                return new List<T>();
            }
        }

        protected async Task<T?> GetByIdAsync(int id)
        {
            try
            {
                var response = await _http.GetAsync($"{Endpoint}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
                }
                return default;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching item {id} from {Endpoint}: {ex.Message}");
                return default;
            }
        }

        protected async Task<T?> PutAsync(int id, T item)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"{Endpoint}/{id}", item);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<T>();
                }
                return default;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error updating item {id} from {Endpoint}: {ex.Message}");
                return default;
            }
        }

        protected async Task<T?> PostAsync(T item)
        {
            try
            {
                var response = await _http.PostAsJsonAsync($"{Endpoint}", item);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<T>();
                }
                return default;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating item at {Endpoint}: {ex.Message}");
                return default;
            }
        }

        protected async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var response = await _http.DeleteAsync($"{Endpoint}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error deleting item {id} at {Endpoint}: {ex.Message}");
                return false;
            }
        }
    }
}
