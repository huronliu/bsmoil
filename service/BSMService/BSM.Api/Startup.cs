using System;
using BSM.Common;
using BSM.Common.DB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Serilog;
using BSM.Common.service;
using BSM.Api.middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BSM.DataServer.websocket;
using System.Net.WebSockets;
using BSM.DataServer;

namespace BSM.Api
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
            services.AddDbContext<BSMContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("BSMDatabase"));
            });
            
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                var secret = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("SecretKey"));
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secret),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = new JsonExceptionMiddleware(env).Invoke
            });

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            
            app.UseAuthentication();
            app.UseMvc();

            //Websocket 
            WebSocketManager wsmanager = WebSocketManager.Instance;
            
            var websocketOptions = new WebSocketOptions()
            {
                ReceiveBufferSize = 4 * 1024
            };
            app.UseWebSockets(websocketOptions);
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.HasValue && context.Request.Path.Value.Contains("/ws"))
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        WebSocket ws = await context.WebSockets.AcceptWebSocketAsync();
                        await wsmanager.NewClientConnected(context, ws);
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }
            });

            Log.Information("Web server is started on: http://{0}:{1}", Config.WebHost, Config.WebPort);
            Log.Information("WebSocket server is started on: ws://{0}:{1}/ws", Config.WebHost, Config.WebPort);
        }
    }
}
