using System.Text.Json;

namespace Infrastructure.Helpers
{
    public enum HttpMethodsTypes
    {
        Get,
        Post,
        Put,
        Patch,
        Delete
    }

    public class HttpRequestHelper
    {
        private HttpClient client { get; set; } = new HttpClient();
        private object? RequestContent { get; set; }
        private string URL { get; set; }
        private HttpMethodsTypes Type { get; set; }

        public HttpRequestHelper(string url, HttpMethodsTypes type, object? content = null)
        {
            RequestContent = content;
            URL = url;
            Type = type;
        }

        public async Task<HttpResponseMessage> ExecuteRequest()
        {
            client.DefaultRequestHeaders.ConnectionClose = true;

            StringContent payload = null;

            if(RequestContent != null)
            {
                var itemJson = JsonSerializer.Serialize(RequestContent);
                payload = new StringContent(itemJson, System.Text.Encoding.UTF8, "application/json");
            }

            HttpResponseMessage response = new HttpResponseMessage();

            switch (Type)
            {
                case HttpMethodsTypes.Get:
                    response = await client.GetAsync(URL);
                    break;
                case HttpMethodsTypes.Post:
                    response = await client.PostAsync(URL, payload);
                    break;
                case HttpMethodsTypes.Put:
                    response = await client.PutAsync(URL, payload);
                    break;
                case HttpMethodsTypes.Delete:
                    response = await client.DeleteAsync(URL);
                    break;
                case HttpMethodsTypes.Patch:
                    response = await client.PatchAsync(URL, payload);
                    break;
            }

            return response;
        }
    }
}
