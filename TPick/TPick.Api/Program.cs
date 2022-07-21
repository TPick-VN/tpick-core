using CsMicro;
using CsMicro.InversionOfControl;
using TPick.App.Services;
using TPick.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
// builder.Host.UseOverrideServiceProviderFactory().UseDefaultServiceProvider(options => { options.ValidateScopes = false; });
builder.Host.UseOverrideServiceProviderFactory();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IShopDetailsCrawler, ShopDetailsCrawler>();
builder.Services.AddHttpClient();
builder.Services
    .AddCsMicro()
    .AddInfrastructure()
    .Build();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options =>
{
    options.AllowAnyMethod();
    options.SetIsOriginAllowed(_ => true);
    options.AllowCredentials();
    options.AllowAnyHeader();
});
app.UseCsMicro();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();