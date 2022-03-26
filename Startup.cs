using AutoMapper;
using CashGen.DBContexts;
using CashGen.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;


namespace CashGen
{
    internal class Startup : FunctionsStartup
    {/*
        public virtual void Configure(IFunctionsHostBuilder builder)
        {
            ServiceCollectionExtensions.AddAutoMapper(builder.Services, AppDomain.CurrentDomain.GetAssemblies());
            ServiceCollectionServiceExtensions.AddScoped<ICashGenRepository, CashGenRepository>(builder.Services);
            string SqlConnection = Environment.GetEnvironmentVariable("SqlConnectionString");
            EntityFrameworkServiceCollectionExtensions.AddDbContext<CashGenContext>(builder.Services, (Action<DbContextOptionsBuilder>)(options => SqlServerDbContextOptionsExtensions.UseSqlServer(options, SqlConnection, (Action<SqlServerDbContextOptionsBuilder>)null)), (ServiceLifetime)1, (ServiceLifetime)1);
        }
*/
        public override void Configure(IFunctionsHostBuilder builder)
        {
            ServiceCollectionExtensions.AddAutoMapper(builder.Services, AppDomain.CurrentDomain.GetAssemblies());
            ServiceCollectionServiceExtensions.AddScoped<ICashGenRepository, CashGenRepository>(builder.Services);
            string SqlConnection = Environment.GetEnvironmentVariable("SqlConnectionString");
            EntityFrameworkServiceCollectionExtensions.AddDbContext<CashGenContext>(builder.Services, (Action<DbContextOptionsBuilder>)(options => SqlServerDbContextOptionsExtensions.UseSqlServer(options, SqlConnection, (Action<SqlServerDbContextOptionsBuilder>)null)), (ServiceLifetime)1, (ServiceLifetime)1);
        }
    }
}
