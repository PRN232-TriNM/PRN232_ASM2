using EVCS.GraphQLWebAPI.TriNM.GraphQL;
using EVCS.TriNM.Services.Implements;
using EVCS.TriNM.Services.Interfaces;
using EVCS.TriNM.Repositories;
using EVCS.TriNM.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using HotChocolate.Types;
using EVCS.TriNM.Repositories.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AllowAnonymousFilter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
    { 
        Title = "EVCS GraphQL API", 
        Version = "v1" 
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EVChargingDBContext>(options =>
    options.UseSqlServer(connectionString),
    ServiceLifetime.Scoped);

builder.Services.AddScoped<IUnitOfWork>(sp =>
{
    var context = sp.GetRequiredService<EVChargingDBContext>();
    return new UnitOfWork(context);
});

builder.Services.AddScoped<IServiceProviders>(sp =>
{
    var unitOfWork = sp.GetRequiredService<IUnitOfWork>();
    var configuration = sp.GetRequiredService<IConfiguration>();
    return new ServiceProviders(unitOfWork, configuration);
});

builder.Services.AddScoped<Queries>();
builder.Services.AddScoped<Mutations>();

builder.Services.AddSignalR();

builder.Services.AddGraphQLServer()
    .AddQueryType<Queries>()
    .AddMutationType<Mutations>()
    .BindRuntimeType<DateTime, DateTimeType>()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "EVCS GraphQL API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseCors();


app.UseRouting();

app.MapGraphQL()
    .WithOptions(new HotChocolate.AspNetCore.GraphQLServerOptions
    {
        Tool = { Enable = true }
    })
    .AllowAnonymous();

app.MapHub<EVCS.GraphQLWebAPI.TriNM.Hubs.StationHub>("/stationHub");

app.MapControllers();

app.Run();
