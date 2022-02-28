using Application.Configuration;
using Application.Hubs;
using Application.Services;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);
const string localhostCorsPolicy = "LocalhostCorsPolicy";
ConfigureServices(builder);

var app = builder.Build();
ConfigureApplication(app);

app.Run();

void ApplyCors(IServiceCollection servies) {
  servies.AddCors(options => {
    options.AddPolicy(localhostCorsPolicy, builder => {
      builder.WithOrigins("http://localhost:4200")
             .AllowAnyMethod()
             .AllowAnyHeader()
             .AllowCredentials();
    });
  });
}

void ApplyConfiguration(WebApplicationBuilder builder) {
  var services = builder.Services;
  var configuration = builder.Configuration;

  services.Configure<FrontEndConfiguration>(configuration.GetRequiredSection("FrontEndConfiguration"));
}

void ConfigureServices(WebApplicationBuilder builder) {
  var services = builder.Services;

  ApplyCors(services);
  ApplyConfiguration(builder);

  services.AddControllersWithViews();
  services.AddInfrastructure(builder.Configuration);
  services.AddSignalR();
  services.AddSingleton<WebsocketManagementService>();
  services.AddScoped<DonationIncentiveHub>();
  services.AddHostedService(provider => provider.GetRequiredService<WebsocketManagementService>());
}

void ConfigureApplication(WebApplication app) {
  if (!app.Environment.IsDevelopment()) {
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
  }

  app.UseCors(localhostCorsPolicy);

  app.UseHttpsRedirection();
  app.UseStaticFiles();
  app.UseRouting();

  app.MapControllerRoute(
    "default",
    "{controller}/{action=Index}/{id?}");

  app.MapFallbackToFile("index.html");
  app.MapHub<DonationIncentiveHub>("/api/hub/donation-incentive");
}