using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using BooksApi.Models;
using BooksApi.Services;
using Microsoft.Extensions.Options;

/* Mongo DB 에서 Cosmos DB 로 마이그레이션이 끝나면 appsettings.json 에서 아래와 같이 ConnectionString 을 바꿔준다.
"ConnectionString": "mongodb://mingyucosmos:2TqDDRstKz5KKAsynreEq8hvAG6B8vLpq8mvBqckEcM8SyXGnnPoDFCgtiGrIJntdpDpDyWCcCVTvw5BWCWPsQ==@mingyucosmos.documents.azure.com:10255/?ssl=true&replicaSet=globaldb",
*/

namespace BooksApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<BookstoreDatabaseSettings>(
                Configuration.GetSection(nameof(BookstoreDatabaseSettings)));

            services.AddSingleton<IBookstoreDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<BookstoreDatabaseSettings>>().Value);

            services.AddSingleton<BookService>();
            services.AddControllers().AddNewtonsoftJson(options => options.UseMemberCasing());
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
