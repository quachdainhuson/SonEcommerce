using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Caching;
using Volo.Abp;
using Volo.Abp.Modularity;
using SonEcommerce.EntityFrameworkCore;
using SonEcommerce.BackgroundWorkers.MailCampaigns;
using Microsoft.AspNetCore.DataProtection;

namespace SonEcommerce.BackgroundWorkers
{
    [DependsOn(
      typeof(AbpAutofacModule),
      typeof(AbpBackgroundWorkersModule),
      typeof(SonEcommerceEntityFrameworkCoreModule),
      typeof(AbpCachingStackExchangeRedisModule)
  )]
    public class SonEcommerceBackgroundWorkersModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostEnvironment = context.Services.GetSingletonInstance<IHostEnvironment>();

            context.Services.AddHostedService<SonEcommerceBackgroundWorkersHostedService>();

            Configure<AbpDistributedCacheOptions>(options =>
            {
                options.KeyPrefix = "SON:";
            });
            var dataProtectionBuilder = context.Services.AddDataProtection().SetApplicationName("SON");
            if (!hostEnvironment.IsDevelopment())
            {
                var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
                dataProtectionBuilder.PersistKeysToStackExchangeRedis(redis, "SON-Protection-Keys");
            }

        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
            {
                context.AddBackgroundWorkerAsync<EmailMarketingWorker>();
            }
        
    }
}
