namespace Product.API.Middleware;

public interface IRestClientService
{
    Task<T> PostAsync<T, U>(string url, U payload, IDictionary<string, string> headers, [CallerMemberName] string caller = "");
}

public class RestClientService : IRestClientService
{
    private readonly IHttpClientFactory _clientFactory;

    public RestClientService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }
    /// <summary>
    /// Generic Async rest client operation with Post method
    /// </summary>
    /// <typeparam name="T">Expected return data type</typeparam>
    /// <typeparam name="U">Data type of Payload</typeparam>
    /// <param name="url">Full absolute URL path</param>
    /// <param name="payload">Paylod body</param>
    /// <param name="headers">Optional - Dictionary of headers</param>
    /// <returns></returns>
    public async Task<T> PostAsync<T, U>(string url, U payload, IDictionary<string, string> headers, [CallerMemberName] string caller = "")
    {
        T objResp = default(T);
        RestResponse response = new();
        try
        {
            var options = new RestClientOptions()
            {
                //MaxTimeout = -1,
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true,
            };
            var request = new RestRequest(url, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    if (header.Key.Trim() == "Authorization")
                    {
                        options.Authenticator = new JwtAuthenticator(header.Value);
                        request?.AddOrUpdateHeader(header.Key, value: header.Value);
                        continue;
                    }
                    request.AddOrUpdateHeader(header.Key, value: header.Value);
                }
            }
            var client = new RestClient(options);
            var body = JsonConvert.SerializeObject(payload);
            request.AddStringBody(body, DataFormat.Json);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12;
            response = await client.ExecuteAsync(request);
            objResp = JsonConvert.DeserializeObject<T>(response.Content);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n=====>\n{nameof(PostAsync)} \n Url:::{url} \n {caller} \n Payload:::  {JsonConvert.SerializeObject(payload)}\n HttpResponse::: {JsonConvert.SerializeObject(response)} \n");
        }
        return objResp;
    }

    public T Deserialize<T>(string input) where T : new()
    {
        if (string.IsNullOrEmpty(input))
        {
            return new T();
        }
        else
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }

        }
    }

}