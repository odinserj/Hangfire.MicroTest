using Hangfire.Common;
using Hangfire.MicroTest.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hangfire.MicroTest.OrdersService
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(config => config.UseApplicationConfiguration());
            services.AddHangfireServer(config => config.Queues = new [] { "orders", "default" });

            services.AddSingleton<OrderSubmitter>();
            services.AddSingleton(provider => new HandlerRegistry(provider)
                .Register<OrderSubmitter>("orders/submit")
                /*.Register<SomeOtherType>("orders/send")*/);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient client)
        {
            client.Enqueue<CustomJobDispatcher>(x => x.Execute("newsletter/send", new CustomJob(
                new JobFilterAttribute[] {new QueueAttribute("newsletter")},
                67890)));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World from Orders Service!"); });
            });
        }
    }
}