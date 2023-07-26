using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient();

/*builder.Services.AddSwaggerGen();*/
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Exchange Rates & Weather", Version = "v1" });

    c.MapType<string>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = new List<IOpenApiAny>
        {
            new OpenApiString("IDR"),
            new OpenApiString("USD"),
            new OpenApiString("JPY"),
            new OpenApiString("EUR"),
            new OpenApiString("SGD"),
            new OpenApiString("MYR"),
            new OpenApiString("THB"),
            new OpenApiString("CAD"),
            new OpenApiString("AUD"),
            new OpenApiString("GBP"),
            new OpenApiString("CNY"),
            new OpenApiString("INR"),
            new OpenApiString("NZD"),
            new OpenApiString("CHF"),
            new OpenApiString("KRW"),
            new OpenApiString("BRL"),
            new OpenApiString("RUB"),
            new OpenApiString("ZAR"),
            new OpenApiString("SEK"),
            new OpenApiString("AED"),
            new OpenApiString("ARS"),
            new OpenApiString("MXN")
        }
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
