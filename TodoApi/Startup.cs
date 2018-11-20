using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(option =>
                    option.UseSqlite(Configuration.GetConnectionString("DefaultConnection"))
            );
            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddIdentity<ApplicationUser, ApplicationRole>(
                option => {
                    option.Password.RequireDigit = false;
                    option.Password.RequiredLength = 6;
                    option.Password.RequireNonAlphanumeric = false;
                    option.Password.RequireUppercase = false;
                    option.Password.RequireLowercase = false;
                }
                
            )
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddAuthentication(option => {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["Jwt:Site"],
                    ValidIssuer = Configuration["Jwt:Site"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SigningKey"]))
                };
            });
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                    }
                );
            });
        }


        public void Configure(IApplicationBuilder app,
                      IHostingEnvironment env,
                      ApplicationDbContext context,
                      RoleManager<ApplicationRole> roleManager,
                      UserManager<ApplicationUser> userManager
                    )
        {
            app.UseAuthentication();
            app.UseMvc();
            DummyData.Initialize(context, userManager, roleManager).Wait();
        }
    }
}