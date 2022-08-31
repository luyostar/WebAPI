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
            }).AddXmlDataContractSerializerFormatters();  //3.0�������д����ͬʱ�������������xml��ʽ

            //ע��Auto Mapper���񣬱���ʵ��model�Ͷ���model�Ķ�Ӧ��ϵ
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

            // ���ӿ������CORS������:(1.���ӷ���2.configureʹ�÷���;3.������ͷ�����ò���)
            //services.AddCors(option =>option.AddPolicy("Domain",
            //    builder=>builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials()
            //    ));
            //services.AddCors(options => options.AddPolicy("Domain1",
            //    builder => builder.WithOrigins("http://a.example.com", "http://c.example.com")
            //                      .WithMethods("GET","POST").AllowAnyHeader().AllowAnyOrigin().AllowCredentials()
            //    ));

            services.AddResponseCaching(); //ע�Ỻ�����
            //����ǿ��֤��ETag��ʹ��
            services.AddHttpCacheHeaders(expire =>
            {
                expire.MaxAge = 70;
                expire.CacheLocation = CacheLocation.Private;
            }, validation =>
            {
                validation.MustRevalidate = true;  //��Ӧ���ڱ�����֤

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
                    await context.Response.WriteAsync("<h1 style='color:#ff0000;'>Unexpected Error(�ǿ����������Զ������)!</h1>");
                }
                    
                    )
                    );

            }

            

            app.UseHttpCacheHeaders(); //����ǿ��֤��ETag�м��Marvin.Cache.Headers

            //app.UseCors("Domain"); // ע��������
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
