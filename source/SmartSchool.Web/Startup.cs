using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartSchool.Core.Contracts;
using SmartSchool.Persistence;
using System.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace SmartSchool.Web
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.Configure<CookiePolicyOptions>(options =>
      {
        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
      });


      services.AddRazorPages();
      services.AddControllers();

      services.AddScoped<IUnitOfWork, UnitOfWork>(serviceProvider =>
        new UnitOfWork());

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmartSchool API", Version = "v1" });
        var filePath = Path.Combine(System.AppContext.BaseDirectory, "SmartSchool.Web.xml");
        c.IncludeXmlComments(filePath);
      });

    }
    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
      }

      app.UseRouting();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapRazorPages();
        endpoints.MapControllers();
      });


      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseCookiePolicy();

      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartSchool API V1");
      });

    }
  }
}
