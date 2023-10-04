using OTP.Domain;
using SimpleInjector;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var container = new Container();
        builder.Services.AddSimpleInjector(container, options =>
        {
            options.AddAspNetCore()
                // Ensure activation of a specific framework type to be created by
                // Simple Injector instead of the built-in configuration system.
                // All calls are optional. You can enable what you need. For instance,
                // ViewComponents, PageModels, and TagHelpers are not needed when you
                // build a Web API.
                .AddControllerActivation()
                .AddViewComponentActivation()
                .AddPageModelActivation()
                .AddTagHelperActivation();

            options.AddLogging();
        });

        container.Register<OtpSettings>(() => builder.Configuration.GetSection(nameof(OtpSettings)).Get<OtpSettings>());
        container.Register<IHashService, Sha256HashService>();
        container.Register<IOtpRepository, InMemoryOtpRepository>(Lifestyle.Singleton);
        container.Register<IOtpService, NaiveOtpService>();

        var app = builder.Build();

        app.Services.UseSimpleInjector(container);

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            //app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        else
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Totp}/{action=RequestTotp}");

        container.Verify();

        app.Run();
    }
}