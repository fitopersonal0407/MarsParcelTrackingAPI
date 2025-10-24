﻿using MarsParcelTracking.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MarsParcelTracking.IntegrationTest
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // ConfigureServices is used to modify the Dependency Injection (DI) container.
            builder.ConfigureServices(services =>
            {
                // Locate the existing DbContext registration in the service collection.
                // This is usually registered with SQL Server in the main app, which we want to replace.
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ParcelContext>));

                // If a registration for ParcelContext exists, remove it.
                // This prevents conflicts between the real SQL Server provider and InMemory provider.
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Create a new ServiceProvider that includes the EF Core InMemory database services.
                // This service provider will be shared internally by all DbContext instances to ensure they use the same in-memory database.
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                // Register ParcelContext with the InMemory database provider.
                // The database is named "IntegrationTestDb" to create a shared in-memory database instance.
                // Using UseInternalServiceProvider ensures all DbContext instances share the same internal EF services and in-memory store.
                services.AddDbContext<ParcelContext>(options =>
                {
                    options.UseInMemoryDatabase("IntegrationTestDb");
                    options.UseInternalServiceProvider(serviceProvider);
                });

                // Build the complete service provider from the updated services collection.
                // This service provider is used to create scopes and resolve services.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain scoped services (like DbContext) for database initialization.
                using (var scope = sp.CreateScope())
                {
                    // Get the scoped service provider for resolving dependencies.
                    var scopedServices = scope.ServiceProvider;

                    // Resolve the ParcelContext instance from the DI container.
                    var db = scopedServices.GetRequiredService<ParcelContext>();

                    // Ensure the in-memory database is created. This sets up the schema based on the EF Core model.
                    db.Database.EnsureCreated();

                    // Seed initial test data into the database to guarantee known state before tests run.
                    // Utilities.InitializeDbForTests contains methods to insert products, customers, etc.
                    Utilities.InitializeDbForTests(db);
                }
            });
        }

    }
}
