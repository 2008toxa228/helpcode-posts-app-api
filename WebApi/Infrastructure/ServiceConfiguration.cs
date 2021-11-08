using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Services;
using WebApi.Services.Interfaces;
//using Core.Interfaces.Services;
//using Core.Services;
//using Data.UnitOfWork;

namespace WebApi.Infrastructure
{
    /// <summary>
    /// Конфигурация зависимостей проектов.
    /// </summary>
    internal static class ServiceConfiguration
    {
        /// <summary>
        /// Задать зависимости проектов.
        /// </summary>
        /// <param name="services">Набор сервисов.</param>
        /// <param name="configuration">Конфигурация проекта.</param>
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //services.AddSingleton((serviceProvider) => configuration);
            //services.AddTransient<IUserService, UserService>();
        }
    }
}
