using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DrinkStoreApp.Services
{
    public class ImageUploadService
    {
        private const string ApiKey = "64a9bbf5-9fcf-46f2-ac45-f85a9f153e25";
        public  async Task<string> UploadImageAsync(string filePath)
        {
            using (HttpClient client = new HttpClient())
            {
                // Đặt tiêu đề xác thực
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($":{ApiKey}")));

                // Đọc tệp để tải lên
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                ByteArrayContent fileContent = new ByteArrayContent(fileBytes);
                string fileExtension = System.IO.Path.GetExtension(filePath).ToLower();
                string contentType = fileExtension == ".png" ? "image/png" : "image/jpeg";
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

                // Tạo URL với tên tệp
                string fileName = System.IO.Path.GetFileName(filePath);
                string requestUri = $"https://pixeldrain.com/api/file/{fileName}";

                // Thực hiện yêu cầu PUT để tải lên tệp
                HttpResponseMessage response = await client.PutAsync(requestUri, fileContent);

                // Kiểm tra nếu phản hồi thành công
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                dynamic responseJson = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);
                string fileUrl = $"https://pixeldrain.com/api/file/{responseJson.id}";

                return fileUrl;
            }
        }
    }
}
