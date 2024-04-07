﻿using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Gp.Api.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A Bad Request, you have made",
                401 => "Unauthorized",
                404 => "Resource was not found",
                500 => "Errors are the path to dark side. Errors lead to anger. Anger leads to hate",
                _ => null,
            };

        }
    }
}

