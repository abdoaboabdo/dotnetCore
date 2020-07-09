using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vega.Core;
using Vega.Core.Models;
using Vega.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;

namespace Vega
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
            services.Configure<PhotoSettings>(Configuration.GetSection("PhotoSettings"));
            services.AddScoped<IVehicleRepository,VehicleRepository>();
            services.AddScoped<IPhotoRepository,PhotoRepository>();
            services.AddScoped<IMakeRepository,MakeRepository>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddDbContext<VegaDbContext>(options=>options.UseSqlServer(Configuration.GetConnectionString("Default")));
            
            services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<VegaDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddApiAuthorization<User, VegaDbContext>();

            // services.AddAuthentication()
            //     .AddJwtBearer();

            SetupJWTServices(services);

            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddAutoMapper(typeof(Startup));
        }



        private void SetupJWTServices(IServiceCollection services)  
        {  
            string key = Configuration.GetSection("JwtConfig").GetSection("secret").Value; //this should be same which is used while creating token      
            var issuer = "https://localhost:5001";  //this should be same which is used while creating token  
  
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)  
                .AddJwtBearer(options =>  
                {  
                    options.TokenValidationParameters = new TokenValidationParameters  
                    {  
                        ValidateIssuer = true,  
                        ValidateAudience = true,  
                        ValidateIssuerSigningKey = true,  
                        ValidIssuer = issuer,  
                        ValidAudience = issuer,  
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))  
                    };  

                    options.Events = new JwtBearerEvents  
                    {  
                        OnAuthenticationFailed = context =>  
                        {  
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))  
                            {  
                                context.Response.Headers.Add("Token-Expired", "true");  
                            }  
                            return Task.CompletedTask;  
                        }  
                    };  
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
