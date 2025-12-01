using IdentityModel.Client;
using Microsoft.Extensions.Configuration;

namespace GrpcClient;

public class ClientCredentialAccessTokenClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ClientCredentialAccessTokenClient(
        IConfiguration configuration,
        HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task<string> GetAccessToken(
        string api_name, string api_scope, string secret,string username="",string pass="")
    {
        try
        {


            var conf = _configuration["OpenIDConnectSettings:Authority"];

            var disco = await HttpClientDiscoveryExtensions.GetDiscoveryDocumentAsync(
                _httpClient, conf
                );

            if (disco.IsError)
            {
                Console.WriteLine($"disco error Status code: {disco.IsError}, Error: {disco.Error}");
                throw new ApplicationException($"Status code: {disco.IsError}, Error: {disco.Error}");
            }


            var cl = new ClientCredentialsTokenRequest
            {
                Scope = api_scope,
                ClientSecret = secret,
                Address = disco.TokenEndpoint,
                ClientId = api_name,
                


            };

            var cl2 = new PasswordTokenRequest
            {
                Scope = api_scope,
                ClientSecret = secret,
                Address = disco.TokenEndpoint,
                ClientId = api_name,
                 Password="Pass123$",
                 UserName="Yuliana"
            };

            var tokenResponse = await HttpClientTokenRequestExtensions.RequestClientCredentialsTokenAsync(_httpClient, cl);

            //var tokenResponse = await HttpClientTokenRequestExtensions.RequestPasswordTokenAsync(_httpClient, cl2);

            

            if (tokenResponse.IsError)
            {
                Console.WriteLine($"tokenResponse.IsError Status code: {tokenResponse.IsError}, Error: {tokenResponse.Error}");
                throw new ApplicationException($"Status code: {tokenResponse.IsError}, Error: {tokenResponse.Error}");
            }

            return tokenResponse.AccessToken;

        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception {e}");
            throw new ApplicationException($"Exception {e}");
        }
    }
}