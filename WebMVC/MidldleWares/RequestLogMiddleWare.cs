using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebMVC.MidldleWares
{
    public class RequestLogMiddleWare
    {
        public RequestDelegate Next { get; set; }

        public RequestLogMiddleWare(RequestDelegate request)
        {
            Next = request;
        }

        public async Task InvokeAsync(HttpContext context)
        {
           
            string path = context.Request.Path;
            Console.WriteLine($"[Hospital Logger] Incoming Request to: {path} at {DateTime.Now}");
            await Next(context);
        }





    }
}
