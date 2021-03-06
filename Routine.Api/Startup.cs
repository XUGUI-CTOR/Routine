using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Routine.Api.Data;
using Routine.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api
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
            services.AddControllers(option =>
            {
                option.ReturnHttpNotAcceptable = true;
                //添加xml格式返回类型
                //option.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                //表示将xml格式设置为默认返回类型
                //option.OutputFormatters.Insert(0, new XmlDataContractSerializerOutputFormatter());
            }).AddNewtonsoftJson(setup => setup.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver())
                .AddXmlDataContractSerializerFormatters()//asp.net core 3.0新写法，添加xml解析
            .ConfigureApiBehaviorOptions(setup =>
            {//自定义效验错误返回状态
                setup.InvalidModelStateResponseFactory = (context) =>
                {
                    var probleDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Type = "http://www.baidu.com",
                        Title = "实体效验错误",
                        Status = StatusCodes.Status422UnprocessableEntity,
                        Detail = "请查看详细信息",
                        Instance = context.HttpContext.Request.Path,
                    };
                    probleDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);
                    return new UnprocessableEntityObjectResult(probleDetails)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            });
            //services.AddScoped<RoutineDbContext>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddDbContext<RoutineDbContext>(delegate (DbContextOptionsBuilder opt) { opt.UseSqlite("DataSource=routine.db"); });
            services.AddScoped<ICompanyRepository, CompanyRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appbuilder =>
                {
                    appbuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Unexcepted Error!");
                    });
                });
            }
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
