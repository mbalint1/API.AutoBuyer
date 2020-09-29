using System.Text;
using AutoBuyer.API.Core.Utilities;
using AutoBuyer.API.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace AutoBuyer.API.POC
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IAuthProvider, AuthProvider>();
            services.AddScoped<ITransactionProvider, TransactionProvider>();
            services.AddScoped<IPlayerProvider, PlayerProvider>();
            services.AddScoped<ISessionProvider, SessionProvider>();
            services.AddScoped<IMessageProvider, MessageProvider>();

            services.AddCors();
            services.AddMvc(option => option.EnableEndpointRouting = false);

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ConnectionUtility.GetSharedSecret())),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
