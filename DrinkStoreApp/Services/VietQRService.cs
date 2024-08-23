using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Windows.Media.Imaging;

namespace DrinkStoreApp.Services
{
    class VietQRService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "09f6e68f-ea7b-4b86-a406-80bb2c9a64e3"; // Your API Key

        public VietQRService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<BitmapImage> GenerateQRCode(decimal amount, string transactionId)
        {
            var apiUrl = "https://api.vietqr.io/v2/generate";
            var requestBody = new
            {
                accountNo = "9704229207219153644",
                accountName = "TRAN THANH BINH",
                addInfo = "Drink Store Payment",
                amount = amount,
                transactionId = transactionId,
                template = "i4KbPGL"
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var qrData = JsonConvert.DeserializeObject<VietQRResponse>(responseJson);

                // Convert the QR code image URL to BitmapImage
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(qrData.data.qrImageUrl);
                bitmapImage.EndInit();

                return bitmapImage;
            }
            else
            {
                throw new Exception("Failed to generate QR code.");
            }
        }
    }
    public class VietQRResponse
    {
        public VietQRData data { get; set; }
    }

    public class VietQRData
    {
        public string qrImageUrl { get; set; }
    }
}
