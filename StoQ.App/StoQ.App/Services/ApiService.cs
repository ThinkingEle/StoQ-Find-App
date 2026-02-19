using StoQ.App.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace StoQ.App.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        // 안드로이드 에뮬레이터 접속 주소
        private readonly string _baseUrl = DeviceInfo.Platform == DevicePlatform.Android
                                         ? "http://10.0.2.2:8000" : "http://localhost:8000";

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<NodeModel>> GetNodesAsync(string parentId = null)
        {
            var url = string.IsNullOrEmpty(parentId) ? $"{_baseUrl}/nodes" : $"{_baseUrl}/nodes?parent_id={parentId}";
            return await _httpClient.GetFromJsonAsync<List<NodeModel>>(url);
        }
    }
}
