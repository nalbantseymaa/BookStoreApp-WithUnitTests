using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WebApi.DBOperations;
using WebApi.Middlewares;
using WebApi.Services;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // dependency injection (bağımlılık enjeksiyonu) sistemine bir tanımlamalar yapılıyor
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
            });

            //  6-Startup.cs içerisinde ConfigureServices() içerisinde DbContext'in servis olarak eklenmesi

            services.AddDbContext<BookStoreDbContext>(options => options.UseInMemoryDatabase("BookStore"));
            // BookStoreDbContext sınıfını, IBookStoreDbContext arayüzü üzerinden Scoped (istek bazlı) olarak inject eder.
            // Yani bir sınıf IBookStoreDbContext isterse, DI konteyneri onun yerine BookStoreDbContext örneğini sağlar.
            services.AddScoped<IBookStoreDbContext>(provider => provider.GetService<BookStoreDbContext>());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //ConsoleLogger sınıfı ILoggerService interface'ini implement ettiği için burada kullanılabilir
            services.AddSingleton<ILoggerService, ConsoleLogger>();

            //services.AddSingleton<ILoggerService, DbLogger>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseCustomExceptionMiddle(); //endpointlere düşmeden önce pipelaine doğru noktadan middleware eklendi

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
