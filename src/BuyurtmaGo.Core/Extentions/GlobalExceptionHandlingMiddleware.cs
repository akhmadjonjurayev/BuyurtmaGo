using BuyurtmaGo.Core.Authentications.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BuyurtmaGo.Core.Extentions
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DbUpdateException ex)
            {
                await WriteResponse(context, StatusCodes.Status500InternalServerError, new ErrorModel("DatabaseError", ex.Message));
            }
            catch (Exception ex)
            {
                await WriteResponse(context, StatusCodes.Status500InternalServerError, new ErrorModel("InternalServerError", ex.Message));
            }
        }

        private async ValueTask WriteResponse(HttpContext context, int statusCode, ErrorModel error)
        {
            context.Response.StatusCode = statusCode;
            var json = JsonSerializer.Serialize(error);
            await context.Response.WriteAsync(json);
        }
    }

    public static class GlobalExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        }
    }
}
