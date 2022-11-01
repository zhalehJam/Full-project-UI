using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Framework.Core.Repository;

namespace Framework.Repository
{
    public abstract class RepositoryBase : IRepository
    {
        private readonly HttpClient _httpClient;

        protected RepositoryBase(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        protected async Task<T> GetList<T>(string requestUrl)
        {
            var response = await _httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<T>();

                return result;
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {


                }
                throw new Exception();
            }

        }
        protected async void Post<T>(string requestUrl, T Command)
        {
            var response = await _httpClient.PostAsJsonAsync(requestUrl, Command);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {


                }
                throw new Exception();
            }

        }


    }
}
