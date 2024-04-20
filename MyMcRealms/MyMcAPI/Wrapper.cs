using MyMcRealms.MyMcAPI.Responses;

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

        public async void GetHello()
        {
            var response = await httpClient.GetFromJsonAsync<HelloResponse>("hello");
            Console.WriteLine(response.Message);
        }
    }
}
