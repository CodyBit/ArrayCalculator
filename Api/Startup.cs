using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using ArrayCalculator.Api.Filters;
using ArrayCalculator.Api.Models.ErrorModels;
using ArrayCalculator.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Filters;

namespace Api
{
    public class Startup
    {
        private static readonly string AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        private static readonly Version AssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                 .AddMvc(options =>
                 {
                     options.Filters.Add(typeof(ApiExceptionFilter), order: 1);
                     options.AllowEmptyInputInBodyModelBinding = true;
                 })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Formatting = Formatting.Indented;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });
            services.AddControllers();

            services.Configure<ApiBehaviorOptions>(apiBehaviorOptions =>
            {
                apiBehaviorOptions.InvalidModelStateResponseFactory = actionContext =>
                {
                    var response = new ErrorResponse(actionContext.HttpContext?.TraceIdentifier)
                    {
                        Errors = actionContext.ModelState.Where(m => m.Value.Errors.Count > 0).Select(m =>
                        {
                            var errorMsg = m.Value.Errors.FirstOrDefault();
                            return new ErrorMessageDetails
                            {
                                Code = StatusCodes.Status400BadRequest.ToString(CultureInfo.InvariantCulture),
                                Message = errorMsg != null
                                ? (string.IsNullOrWhiteSpace(errorMsg.ErrorMessage)
                                    ? $"{m.Key}: {errorMsg.Exception.GetBaseException().Message}"
                                    : $"{m.Key}: {errorMsg.ErrorMessage}")
                                : null
                            };
                        }).ToList()
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            ConfigureApiServices(services);
            ConfigureSwagger(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.DefaultModelsExpandDepth(-1);
                    c.RoutePrefix = string.Empty;
                    c.SwaggerEndpoint($"/swagger/{AssemblyVersion}/swagger.json", $"{AssemblyName}.v{AssemblyVersion?.Major}");
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void ConfigureApiServices(IServiceCollection services)
        {
            services.AddTransient<IProductService, ProductService>();        
        }

        private static void ConfigureSwagger(IServiceCollection services)
        {
            services
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc($"{AssemblyVersion}", new OpenApiInfo
                    {
                        Title = AssemblyName,
                        Version = $"v{AssemblyVersion?.Major}",
                        Description = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyDescriptionAttribute>().Description,
                        Contact = new OpenApiContact
                        {
                            Name = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyCompanyAttribute>().Company,
                            Url = new Uri("https://github.com/CodyBit")
                        },
                        License = new OpenApiLicense
                        {
                            Name = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright,
                            Url = new Uri("https://github.com/CodyBit/ArrayCalculator/blob/master/LICENSE.md")
                        }
                    });
                    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{AssemblyName}.xml"));
                    c.ExampleFilters();
                })
                .AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
        }
    }
}
