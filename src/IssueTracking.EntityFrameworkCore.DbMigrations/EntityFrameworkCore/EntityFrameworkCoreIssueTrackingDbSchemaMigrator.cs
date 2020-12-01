﻿using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using IssueTracking.Data;
using Volo.Abp.DependencyInjection;

namespace IssueTracking.EntityFrameworkCore
{
    public class EntityFrameworkCoreIssueTrackingDbSchemaMigrator
        : IIssueTrackingDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreIssueTrackingDbSchemaMigrator(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            /* We intentionally resolving the IssueTrackingMigrationsDbContext
             * from IServiceProvider (instead of directly injecting it)
             * to properly get the connection string of the current tenant in the
             * current scope.
             */

            await _serviceProvider
                .GetRequiredService<IssueTrackingMigrationsDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}