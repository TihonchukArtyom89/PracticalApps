using Microsoft.AspNetCore.Mvc.Formatters;//IOutputFormatter,OutputFormatter
using Packt.Shared;//AddNorthwindContext extension method
using Northwind.WebApi.Repositories;//ICustomerRepository, CustomerRepository
using Swashbuckle.AspNetCore.SwaggerUI;//SubmitMethod
using Microsoft.AspNetCore.HttpLogging;//HttpLoggingFields
using System.Net.Http.Headers;//MedaTypeWithQualityHeaderValue
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddNorthwindContext();
builder.Services.AddControllers(options =>
{
    WriteLine("Default output formatters:");
    foreach (IOutputFormatter formatter in options.OutputFormatters)
    {
        OutputFormatter? mediaFormatter = formatter as OutputFormatter;
        if (mediaFormatter is null)
        {
            WriteLine($"    {formatter.GetType().Name}");
        }
        else //OutputFormatter has SupportedMediaTypes
        {
            WriteLine("    {0}, Media types: {1}", arg0: mediaFormatter.GetType().Name, string.Join(" ,", mediaFormatter.SupportedMediaTypes));
        }
    }
}
).AddXmlDataContractSerializerFormatters().AddXmlSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddHttpLogging(options => 
{
    options.LoggingFields=HttpLoggingFields.All;
    options.RequestBodyLogLimit = 4096;//default 32k
    options.ResponseBodyLogLimit = 4096;//default 32k
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Norhtwind Service API Version 1"); c.SupportedSubmitMethods(new[] { SubmitMethod.Get, SubmitMethod.Post, SubmitMethod.Put, SubmitMethod.Delete }); });
}

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<SecurityHeaders>();
app.MapControllers();

app.Run();
