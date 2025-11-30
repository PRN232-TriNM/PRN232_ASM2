using EVCS.BlazorWebApp.TriNM;
using EVCS.BlazorWebApp.TriNM.GraphQLClient;
using EVCS.BlazorWebApp.TriNM.Services;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<IGraphQLClient>(c =>
    new GraphQLHttpClient(builder.Configuration["GraphQLURI"], new NewtonsoftJsonSerializer())
);

builder.Services.AddScoped<GraphQLConsumer>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationService>();

builder.Services.AddSingleton<StationSignalRService>();

await builder.Build().RunAsync();
