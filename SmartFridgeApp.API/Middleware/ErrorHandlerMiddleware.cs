using Microsoft.AspNetCore.Http;
using SmartFridgeApp.Core.Exceptions;
using SmartFridgeApp.Infrastructure.Validation;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartFridgeApp.API.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                string details = "DefaultException";

                switch (error)
                {
                    case CustomValidationException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        details = e.Details;
                        break;
                    case AmountValueException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        details = e.Details;
                        break;
                    case InvalidInputException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        details = e.Details;
                        break;
                    case InvalidFridgeException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        details = e.Details;
                        break;
                    case InvalidRecipeException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        details = e.Details;
                        break;
                    case InvalidFoodProductCategoryException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        details = e.Details;
                        break;
                    case FoodProductNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        details = e.Details;
                        break;
                    case FridgeItemNotExistException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        details = e.Details;
                        break;
                    case AppException e:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        details = e.Details;
                        break;
                    case DomainException e:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        details = e.Details;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new { message = error?.Message, details = details });
                await response.WriteAsync(result);
            }
        }
    }
}