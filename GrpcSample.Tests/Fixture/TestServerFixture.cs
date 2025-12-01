using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Net.Client;
using CalibrationSaaS.Application.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using CalibrationSaaS.Service;
using Grpc.Core;

namespace GrpcSample.Tests.Fixture
{
    public sealed class TestServerFixture : IDisposable
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private static string _token;
        public TestServerFixture()
        {
            _factory = new WebApplicationFactory<Startup>();
            var client = _factory.CreateDefaultClient(new ResponseVersionHandler());

               var credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                if (!string.IsNullOrEmpty(_token))
                {
                    metadata.Add("Authorization", $"Bearer {_token}");
                }
                return Task.CompletedTask;
            });

            GrpcChannel = GrpcChannel.ForAddress(client.BaseAddress, new GrpcChannelOptions
            {
                HttpClient = client,
                //Credentials=ChannelCredentials.Create(new SslCredentials(),credentials)   
            });
        }

        public GrpcChannel GrpcChannel { get; }

        public void Dispose()
        {
            _factory.Dispose();
        }

        private class ResponseVersionHandler : DelegatingHandler
        {
            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                var response = await base.SendAsync(request, cancellationToken);
                response.Version = request.Version;
                return response;
            }
        }
    }
}