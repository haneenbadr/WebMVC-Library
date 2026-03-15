using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebMVC.MidldleWares
{
    public class BookDetailsMiddleware
    {
        public RequestDelegate Next { get; set; }
        public BookDetailsMiddleware(RequestDelegate request)
        {
            Next = request;
        }

        
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == "/products")
            {
                LibraryDB dBContext = new LibraryDB();
                var list = dBContext.books.ToList();
                StringBuilder str = new StringBuilder();

                foreach (var book in list)
                {
                    str.Append($"<p> {book.title} : {book.Author.name}$ </p>");
                }

                await context.Response.WriteAsync(str.ToString());
            }

        }
    }
}