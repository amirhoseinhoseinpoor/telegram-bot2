public class CryptoPriceFetcher
{

    private static readonly HttpClient HttpClient = new HttpClient();
    private static readonly string ApiUrl = "https://min-api.cryptocompare.com/data/price?fsym=BTC&tsyms=USD,JPY,EUR";

    public static async Task<(double usdPrice, double jpyPrice, double eurPrice)> GetBitcoinPricesAsync()
    {
        HttpResponseMessage response = await HttpClient.GetAsync(ApiUrl);

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResponse);

            double usdPrice = data?.USD;
            double jpyPrice = data?.JPY;
            double eurPrice = data?.EUR;

            return (usdPrice, jpyPrice, eurPrice);
        }
        else
        {
            Console.WriteLine("Error fetching data from the API");
            return (0, 0, 0);
        }
    }
}