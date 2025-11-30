using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;

namespace EVCS.BlazorWebApp.TriNM.Services
{
    public class StationSignalRService : IAsyncDisposable
    {
        private HubConnection? _hubConnection;
        private readonly string _hubUrl;

        public event Action? StationCreated;
        public event Action<int>? StationUpdated;
        public event Action<int>? StationDeleted;

        public StationSignalRService(IConfiguration configuration)
        {
            var graphQLUri = configuration["GraphQLURI"] ?? "https://localhost:7053/graphql";
            var baseUri = graphQLUri.Replace("/graphql", "");
            _hubUrl = $"{baseUri}/stationHub";
        }

        public async Task StartAsync()
        {
            if (_hubConnection == null)
            {
                _hubConnection = new HubConnectionBuilder()
                    .WithUrl(_hubUrl)
                    .WithAutomaticReconnect()
                    .Build();

                _hubConnection.On<object>("StationCreated", (station) =>
                {
                    StationCreated?.Invoke();
                });

                _hubConnection.On<object>("StationUpdated", (station) =>
                {
                    try
                    {
                        if (station is System.Text.Json.JsonElement jsonElement)
                        {
                            if (jsonElement.TryGetProperty("stationTriNmid", out var idElement))
                            {
                                var id = idElement.GetInt32();
                                StationUpdated?.Invoke(id);
                            }
                        }
                    }
                    catch
                    {
                        StationUpdated?.Invoke(0);
                    }
                });

                _hubConnection.On<int>("StationDeleted", (id) =>
                {
                    StationDeleted?.Invoke(id);
                });

                try
                {
                    await _hubConnection.StartAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"SignalR connection failed: {ex.Message}");
                }
            }
        }

        public async Task StopAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.StopAsync();
            }
        }

        public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.DisposeAsync();
            }
        }
    }
}

