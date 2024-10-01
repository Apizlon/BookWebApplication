using Microsoft.AspNetCore.Authentication;
using System.IO;
using System.Text;

namespace ApiGateway
{
    public class Destination
    {
        public string Uri { get; set; }
        public bool RequiresAuthentication { get; set; }

        public Destination(string uri, bool requiresAuthentication)
        {
            Uri = uri;
            RequiresAuthentication = requiresAuthentication;
        }

        public Destination(string uri)
            : this(uri, false)
        {
        }

        private Destination()
        {
            Uri = "/";
            RequiresAuthentication = false;
        }

        private string CreateDestinationUri(HttpRequest request)
        {
            string requestPath = request.Path.ToString();
            string queryString = request.QueryString.ToString();

            string endpoint = "";
            string[] endpointSplit = requestPath.Substring(1).Split('/');

            if (endpointSplit.Length > 1)
                endpoint = endpointSplit[1];

            return Uri + endpoint + queryString;
        }

        public async Task<HttpResponseMessage> SendRequest(HttpRequest request)
        {
            string requestContent;
            using (Stream receiveStream = request.Body)
            {
                using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                {
                    requestContent = await readStream.ReadToEndAsync();
                }
            }

            HttpClient client = new HttpClient();
            HttpRequestMessage newRequest = new HttpRequestMessage(new HttpMethod(request.Method), CreateDestinationUri(request));
            HttpResponseMessage response = await client.SendAsync(newRequest);

            return response;
        }

        public async Task<HttpResponseMessage> SendAuthenticationRequest(string token)
        {
            HttpClient client = new HttpClient();

            // Создаем запрос на аутентификацию с токеном, используя базовый URI
            HttpRequestMessage authRequest = new HttpRequestMessage(HttpMethod.Get, this.Uri);
            authRequest.Headers.Add("token", token);

            HttpResponseMessage response = await client.SendAsync(authRequest);
            return response;
        }
    }
}

