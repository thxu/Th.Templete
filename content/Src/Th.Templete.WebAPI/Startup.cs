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
                    ConnectionString = conn,//����, ���ݿ������ַ���
                    DbType = DbType.SqlServer,//����, ���ݿ�����
                    IsAutoCloseConnection = true,//Ĭ��false, ʱ��֪���ر����ݿ�����, ����Ϊtrue����ʹ��using����Close����
                    InitKeyType = InitKeyType.SystemTable,//Ĭ��SystemTable, �ֶ���Ϣ��ȡ, �磺�������ǲ�����������ʶ�еȵ���Ϣ
                    SlaveConnectionConfigs = new List<SlaveConnectionConfig>
                    {
                        new SlaveConnectionConfig() { HitRate=100, ConnectionString= conn }
                    },
                    MoreSettings = new ConnMoreSettings()
                    {
                        IsWithNoLockQuery = true,// ȫ�� with(nolock)
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
