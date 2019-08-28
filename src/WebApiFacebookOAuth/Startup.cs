﻿using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WebApiFacebookOAuth.Entities;
using WebApiFacebookOAuth.Models;
using WebApiFacebookOAuth.Services;

namespace WebApiFacebookOAuth
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
            var setting = Configuration.Get<AppSetting>();
            services.AddSingleton<AppSetting>(setting);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = setting.JwtAuthentication.Issuer,
                    ValidAudience = setting.JwtAuthentication.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(setting.JwtAuthentication.SecurityKey)),
                };
            });
            services.AddHttpContextAccessor();
            services.AddHttpClient();
            services.AddScoped<UtilService>();
            services.AddScoped<FacebookOAuthService>();
            services.AddScoped<AuthenticationService>();
            services.AddDbContextPool<MyDbContext>(optionBuilder => optionBuilder.UseNpgsql(setting.ConnectionString));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // var db = services.BuildServiceProvider().GetService<MyDbContext>();
            // db.Database.EnsureCreated();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
