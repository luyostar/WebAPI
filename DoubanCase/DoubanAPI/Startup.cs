using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoubanAPI.Services;
using DoubanData;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace DoubanAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(setup=> {
                setup.ReturnHttpNotAcceptable = true;
            }).AddNewtonsoftJson(setup=> {
                setup.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }).AddXmlDataContractSerializerFormatters();  //3.0后有这个写法，同时添加输入和输出的xml格式

            //注册Auto Mapper服务，便于实体model和对外model的对应关系
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //services.AddDbContext<DataContext>();

            services.AddDbContext<DataContext>(opt =>
                     opt.UseMySql(Configuration.GetConnectionString("MySqlDB"), ServerVersion.Parse("8.0.24-mysql"))
            );

            //services.AddDbContext<DataContext>(opt =>
            //         opt.UseSqlServer(Configuration.GetConnectionString("SqlServerDB"))
            //);




            services.AddScoped<CommentRepository>();
            services.AddScoped<UsersRepository>();
            services.AddScoped<MoviesRepository>();

            // 增加跨域服务CORS，步骤:(1.增加服务，2.configure使用服务;3.控制器头部启用策略)
            //services.AddCors(option =>option.AddPolicy("Domain",
            //    builder=>builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials()
            //    ));
            //services.AddCors(options => options.AddPolicy("Domain1",
            //    builder => builder.WithOrigins("http://a.example.com", "http://c.example.com")
            //                      .WithMethods("GET","POST").AllowAnyHeader().AllowAnyOrigin().AllowCredentials()
            //    ));

            services.AddResponseCaching(); //注册缓存服务；
            //缓存强验证器ETag的使用
            services.AddHttpCacheHeaders(expire =>
            {
                expire.MaxAge = 70;
                expire.CacheLocation = CacheLocation.Private;
            }, validation =>
            {
                validation.MustRevalidate = true;  //响应过期必须验证

            }
             );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } else
            {
                app.UseExceptionHandler(appBuilder=>appBuilder.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("<h1 style='color:#ff0000;'>Unexpected Error(非开发环境的自定义错误)!</h1>");
                }
                    
                    )
                    );

            }

            

            app.UseHttpCacheHeaders(); //缓存强验证，ETag中间件Marvin.Cache.Headers

            //app.UseCors("Domain"); // 注册跨域策略
            //app.UseCors("Domain1");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
