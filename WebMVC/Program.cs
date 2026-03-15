namespace WebMVC
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder();

           

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            app.useMiddleware<MidldleWares.RequestLogMiddleWare>();



            app.Run(async (context) =>
            {
                if (context.Request.Path == "/")
                {
                    await context.Response.WriteAsync(" <h1>Welcome In Our Hospital </h1>");
                }
            });

        }
    }
}
