using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using ProtoBuf.Grpc.Client;
using System.Net.Http;

namespace CalibrationSaaS.Infraestructure.Blazor.Helper
{
    public static class GrpcService
    {
        public static T StablishConnection<T>() where T : class
        {
            var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
            var channel = Grpc.Net.Client.GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions { HttpClient = httpClient });
            return channel.CreateGrpcService<T>();
        }

        public static GrpcChannel StablishConnection()
        {
            var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
            using var channel = Grpc.Net.Client.GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions { HttpClient = httpClient });
            return channel;
        }
    }
}
