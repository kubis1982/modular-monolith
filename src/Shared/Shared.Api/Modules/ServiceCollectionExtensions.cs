﻿namespace ModularMonolith.Shared.Modules
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using ModularMonolith.Shared.Extensions;
    using System;
    using System.Linq;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddModules(this IServiceCollection services, IConfiguration configuration)
        {
            foreach (var module in AppDomain.CurrentDomain.GetSystemAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(AbstractModuleDefinition).IsAssignableFrom(x) && !x.IsAbstract)
                .OrderBy(x => x.Name)
                .Select(Activator.CreateInstance)
                .Cast<AbstractModuleDefinition>()
                .Distinct())
            {
                module.AddServices(services, configuration);
            }
            return services;
        }
    }
}
