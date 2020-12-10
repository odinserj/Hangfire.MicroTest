using Hangfire.Common;
using Hangfire.MicroTest.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hangfire.MicroTest.NewsletterService
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(config => config.UseApplicationConfiguration());
            services.AddHangfireServer(config => config.Queues = new [] { "newsletter", "default" });

            services.AddSingleton<NewsletterSender>();
            services.AddSingleton(provider => new HandlerRegistry(provider)
                .Register<NewsletterSender>("newsletter/send")
                /*.Register<SomeOtherType>("newsletter/generate")*/);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient client)
        {
            client.Enqueue<CustomJobDispatcher>(x => x.Execute("orders/submit", new CustomJob(
                new JobFilterAttribute[] {new QueueAttribute("orders")},
                12345,
                "Created")));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World from Newsletter Service!"); });
            });
        }
    }
}