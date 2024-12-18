using Microsoft.EntityFrameworkCore;
using PrsWeb.Models;

namespace PrsWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<PrsdbContext>(
            options => options.UseSqlServer(builder.Configuration.GetConnectionString("prsdbConnectionString")));

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
