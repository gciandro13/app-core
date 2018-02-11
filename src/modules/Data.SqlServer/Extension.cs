﻿using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace core.Extensions.Data.SqlServer
{
    public class Extension : Base.Extension
    {
        private Options _options => GetOptions<Options>();

        public override void Execute(IServiceCollection serviceCollection, IServiceProvider serviceProvider)
        {
            base.Execute(serviceCollection, serviceProvider);

            var connections = _options?.Connections;
            if (connections != null && connections.Any())
            {
                serviceCollection.Configure<Options>(_ =>
                {
                    _.Connections = connections;
                });
                serviceCollection.AddDbContext<AppDbContext>(_ => _.UseSqlServer(connections.FirstOrDefault().ConnectionString));
                serviceCollection.TryAddTransient(typeof(IRepository<>), typeof(Repository.SqlServer<>));
            }
        }
    }
}