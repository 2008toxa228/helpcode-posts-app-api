using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebApi.Infrastructure.Jwt;
using Microsoft.IdentityModel.Tokens;
using WebApi.Models.Configuration;
using WebApi.Services.Interfaces;
using WebApi.Services;
using Microsoft.Extensions.WebEncoders;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using WebApi.DataBaseProvider;
using WebApi.Infrastructure.Hash;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IUserService WebApiService { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
            });

            //Разрешаем корсы для всего вообще, разумеется для dev мода.
            services.AddCors(/*options => options.AddPolicy("AllowSpecificOrigin",
                builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                })*/);
            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });

            // Подключаем поддержку параметров.
            services.AddOptions();

            // Создаем объект Config по ключам из конфигурации.
            services.Configure<WebApiConfiguration>(Configuration.GetSection("Config"));
            services.AddSingleton(Configuration);

            // ToD refactor this part.
            // Создаем сервис с реализацией методов апи.
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IDataBaseService, DataBaseService>();

            // Depedency Injection и конфигурация сервисов.
            //Infrastructure.ServiceConfiguration.ConfigureServices(services, Configuration);

            // Конфигурация Hash-менеджера.
            PasswordManager.SetConfig(Configuration);

            // Конфигурация JWT-менеджера.
            JwtManager.SetConfig(Configuration);

            // Подключаем поддержку JWT-токенов.
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(jwt => 
                {
                    jwt.SaveToken = true;
                    jwt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = JwtManager.Issuer,
                        ValidateAudience = true,
                        ValidAudience = JwtManager.Audience,
                        ValidateLifetime = true,
                        LifetimeValidator = JwtManager.ValidateTime,
                        IssuerSigningKey = JwtManager.GetKey(),
                        ValidateIssuerSigningKey = true
                    };
                });

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            //{
            //    options.RequireHttpsMetadata = true; // Передача токена по SSL.
            //    options.TokenValidationParameters = new TokenValidationParameters()
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = JwtManager.GetKey(),
            //        ValidateIssuer = true,
            //        ValidIssuer = JwtManager.Issuer,
            //        ValidateAudience = true,
            //        ValidAudience = JwtManager.Audience,
            //        ValidateLifetime = true,
            //        LifetimeValidator = JwtManager.ValidateTime,
            //    };
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
