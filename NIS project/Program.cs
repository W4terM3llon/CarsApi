using GraphQL.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NIS_project.Data;
using NIS_project.GraphQL.GraphQLQueries;
using NIS_project.GraphQL.GraphQLSchema;
using NIS_project.Models.Repositories;
using NIS_project.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<NIS_projectContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NIS_projectContext")));

// Add services to the container.
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IEngineRepository, EngineRepository>();
builder.Services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<AppSchema>();
builder.Services.AddScoped<AppQuery>();
builder.Services.AddScoped<AppMutation>();

builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = $"{builder.Configuration.GetValue<string>("RedisCache:Host")}:{builder.Configuration.GetValue<int>("RedisCache:Port")}";
});

builder.Services.AddGraphQL()
    .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true)
    .AddSystemTextJson()
    .AddGraphTypes(typeof(AppSchema), ServiceLifetime.Scoped);

builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseGraphQL<AppSchema>();
app.UseGraphQLPlayground();

app.MapControllers();

app.Run();
