using Grpc.Core;
using Grpc.Net.Client;
using GrpcClient;


using Microsoft.Extensions.Configuration;
using Grpc.Core;
using Microsoft.Extensions.Primitives;


public static class  CustomCallOptions
{

    public static async Task<string> GetCustomToken()
    {

        var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

        var configuration = builder.Build();

        var clientCredentialAccessTokenClient
            = new ClientCredentialAccessTokenClient(configuration, new HttpClient());

        // 2. Get access token
        //var accessToken = await clientCredentialAccessTokenClient.GetAccessToken(
        //    configuration["OpenIDConnectSettings:ClientId"],
        //    "dataEventRecords",
        //    configuration["OpenIDConnectSettings:ClientSecret"]
        //);
        var accessToken = await clientCredentialAccessTokenClient.GetAccessToken(
            "CC",
            "dataEventRecords",
            "cc_secret"
       );





        return accessToken;
    }


    public static async Task<CallOptions> GetCustomCallOptions(string accessToken)
    {

        
        if (accessToken == null)
        {
            Console.WriteLine("no auth result... ");
            return default;
        }
        else
        {
            Console.WriteLine(accessToken);

            var tokenValue = "Bearer " + accessToken;
            var metadata = new Metadata();
   
            metadata.Add("Authorization", $"Bearer {accessToken}");
            //var handler = new HttpClientHandler();

            //var channel = GrpcChannel.ForAddress(
            //    configuration["ProtectedApiUrl"],
            //    new GrpcChannelOptions
            //    {
            //        HttpClient = new HttpClient(handler)

            //    });



            

            CallOptions callOptions = new(metadata);

           


            return callOptions;

            //var client = new Greeter.GreeterClient(channel);

            //var reply = await client.SayHelloAsync(
            //    new HelloRequest { Name = "GreeterClient" }, callOptions);

            //Console.WriteLine("Greeting: " + reply.Message);

            //Console.WriteLine("Press any key to exit...");
            //Console.ReadKey();
        }


    }


}


