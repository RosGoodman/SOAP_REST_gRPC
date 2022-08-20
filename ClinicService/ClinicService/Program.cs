using ClinicService.Data.Context;
using ClinicService.Repositoryes.Impl;
using ClinicService.Repositoryes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpLogging;
using ClinicService.Services;
using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace ClinicService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            /*4. дл€ конфигурировани€ соответствующего веб-сервера, в рамках конфигурации этого сервера
             * необходимо добавить настройки и порт, по которому будут передаватьс€ бинарные сооющени€
             */
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Listen(IPAddress.Any, 5001, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                });
            });


            builder.Services.AddDbContext<ClinicServiceDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["Settings:DatabaseOptions:ConnectionString"]);
            });

            //фреймворк grpc несовместим с системой логгировани€ запросов в контексте asp.net core
            //по этой причине добавл€етс€ настройка 3.
            builder.Services.AddGrpc(); // 1. инсталл€ци€ сервиса grpc

            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.All | HttpLoggingFields.RequestQuery;
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
                logging.RequestHeaders.Add("Authorization");
                logging.RequestHeaders.Add("X-Real-IP");
                logging.RequestHeaders.Add("X-Forwarded-For");
            });

            #region Configure Repository Services

            builder.Services.AddScoped<IPetRepository, PetRepository>();
            builder.Services.AddScoped<IConsultationRepository, ConsultationRepository>();
            builder.Services.AddScoped<IClientRepository, ClientRepository>();

            #endregion

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpLogging();

            // 3.когда будут приходить запросы в рамках grpc-фреймворка, то эти запросы не будут логгироватьс€
            // стандартной системой логгировани€ asp.net core
            // microsoft обещают исправить в 7-й версии .net 7
            app.UseWhen(
                ctx => ctx.Request.ContentType != "application/grpc",
                builder =>
                {
                    builder.UseHttpLogging();
                });

            


            app.MapControllers();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>   // 2.grpc
            {
                // Communication with gRPC endpoints must be made through a gRPC client.
                // To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909
                endpoints.MapGrpcService<ClientService>();
                endpoints.MapGrpcService<PetService>();
                endpoints.MapGrpcService<ConsultationService>();
            });

            app.Run();
        }
    }
}