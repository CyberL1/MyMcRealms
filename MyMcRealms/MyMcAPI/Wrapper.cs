﻿using MyMcRealms.MyMcAPI.Responses;

namespace MyMcRealms.MyMcAPI
{
    public class MyMcAPI
    {
        private readonly string ApiUrl = "https://api.my-mc.link";
        private readonly HttpClient httpClient = new();

        public MyMcAPI(string apiKey)
        {
            httpClient.DefaultRequestHeaders.Add("x-my-mc-auth", apiKey);
            httpClient.BaseAddress = new Uri(ApiUrl);
        }

        public async Task<AllServersResponse?> GetAllServers()
        {
            AllServersResponse response = await httpClient.GetFromJsonAsync<AllServersResponse>($"list_all_servers/{Environment.GetEnvironmentVariable("MYMC_SERVER_LIST_KEY")}");
            
            if (response == null)
            {
                Console.WriteLine("error while doing GET /list_all_servers");
                return null;
            }

            return response;
        }

        public HttpResponseMessage? ExecuteCommand(string command)
        {
            string json = $"{{ \"command\": \"{command}\" }}";

            StringContent content = new(json, System.Text.Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync("console", content).Result;

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("error while doing POST /console");
                return null;
            }

            return response;
        }
    }
}
