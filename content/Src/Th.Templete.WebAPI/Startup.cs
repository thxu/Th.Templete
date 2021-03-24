using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.OpenApi.Models;
using SqlSugar;

namespace Th.Templete.WebAPI
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
            services.AddControllers();

            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "Th.Templete.WebAPI",
                    Description = "Th.Templete.WebAPI"
                });

                option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Th.Templete.WebAPI.xml"));
                option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Th.Templete.Application.Contract.xml"));
            });

            var conn = Configuration.GetSection("ConnectionStrings").Value;
            services.AddScoped<ISqlSugarClient>(o =>
            {
                return new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = conn,//必填, 数据库连接字符串
                    DbType = DbType.SqlServer,//必填, 数据库类型
                    IsAutoCloseConnection = true,//默认false, 时候知道关闭数据库连接, 设置为true无需使用using或者Close操作
                    InitKeyType = InitKeyType.SystemTable,//默认SystemTable, 字段信息读取, 如：该属性是不是主键，标识列等等信息
                    SlaveConnectionConfigs = new List<SlaveConnectionConfig>
                    {
                        new SlaveConnectionConfig() { HitRate=100, ConnectionString= conn }
                    },
                    MoreSettings = new ConnMoreSettings()
                    {
                        IsWithNoLockQuery = true,// 全局 with(nolock)
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint($"/swagger/v1/swagger.json", "default api");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(Th.Templete.Application.Demo.Impl.Demo).Assembly).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(Th.Templete.Repositories.DemoRepository).Assembly).AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
