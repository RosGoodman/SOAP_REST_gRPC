using ClinicService.Data.Context;
using ClinicService.Repositoryes.Impl;
using ClinicService.Repositoryes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpLogging;
using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using ClinicService.Services.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ClinicService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            /*4. ��� ���������������� ���������������� ���-�������, � ������ ������������ ����� �������
             * ���������� �������� ��������� � ����, �� �������� ����� ������������ �������� ���������
             */
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Listen(IPAddress.Any, 5001, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                    //���������� ��������� ��� ������ ������� "��������� ����� iis".
                    listenOptions.UseHttps(@"D:\CSharp\testCertificate.pfx", "12345");
                });
            });


            builder.Services.AddDbContext<ClinicServiceDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["Settings:DatabaseOptions:ConnectionString"]);
            });

            //��������� grpc ����������� � �������� ������������ �������� � ��������� asp.net core
            //�� ���� ������� ����������� ��������� 3.
            builder.Services.AddGrpc(); // 1. ����������� ������� grpc

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

            //������������ ��������������
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthenticateService.SecretKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };
                });

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

            // 3.����� ����� ��������� ������� � ������ grpc-����������, �� ��� ������� �� ����� �������������
            // ����������� �������� ������������ asp.net core
            // microsoft ������� ��������� � 7-� ������ .net 7
            app.UseWhen(
                ctx => ctx.Request.ContentType != "application/grpc",
                builder =>
                {
                    builder.UseHttpLogging();
                });


            app.MapControllers();
            app.UseRouting();

            app.UseAuthentication();
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