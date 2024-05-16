using MyMcRealms.MyMcAPI.Responses;

namespace MyMcRealms.MyMcAPI
{
    public class Wrapper
    {
        private readonly string ApiUrl = "https://api.my-mc.link";
        private readonly HttpClient httpClient = new();

        public Wrapper(string apiKey)
        {
            httpClient.DefaultRequestHeaders.Add("x-my-mc-auth", apiKey);
            httpClient.BaseAddress = new Uri(ApiUrl);
        }

        public async Task<AllServersResponse?> GetAllServers()
        {
            try
            {
                AllServersResponse? response = await httpClient.GetFromJsonAsync<AllServersResponse>($"list_all_servers/{Environment.GetEnvironmentVariable("MYMC_SERVER_LIST_KEY")}");

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("error while doing GET /list_all_servers");
                Console.WriteLine(e.Message);
                return null;
            }
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
