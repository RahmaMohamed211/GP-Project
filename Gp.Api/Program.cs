using Gp.Api.Extensions;
using Gp.Api.Hellpers;
using Gp.Api.Middlewares;
using GP.Core.Entities;
using GP.Core.Repositories;
using GP.Repository;
using GP.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Gp.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

           builder.Services.AddControllers();

            //builder.Services.AddControllers()
            //      .AddJsonOptions(options =>
            //          {
            //               options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            //               options.JsonSerializerOptions.WriteIndented = true;  
            //          });


            ////////////////////
            ///*------Database
            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddApplicationServices();


            builder.Services.AddSwaggerServices();
          

            var app = builder.Build();
            //Explicity
            var scope = app.Services.CreateScope();   //services scopped
            var Services = scope.ServiceProvider; //di
            //loggerFacotry
            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
            try
            {
                var dbContext = Services.GetRequiredService<StoreContext>(); //ask clr to create object from store context explicity
                await dbContext.Database.MigrateAsync(); //update-datebase
                await StoreContextSeed.SeedAsync(dbContext); 
            }
            catch (Exception ex)
            {
                var logger = LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error Occured during applay Migration");

            }
            #region Configure Kestrel Middlewares
            app.UseMiddleware<ExceptionMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
               
                app.UseSwaggerMiddlewares();
            }
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthorization();


            app.MapControllers();


            #endregion

            app.Run();
        }
    }
}
