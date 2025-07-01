﻿using System.Net;
using System.Text.Json;
using System.Threading.Tasks; 
using Microsoft.AspNetCore.Http; 
using Microsoft.Extensions.Logging; 
using System; 

namespace JobTracking.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = "Възникна вътрешна сървърна грешка.",
                Detailed = exception.Message 
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}