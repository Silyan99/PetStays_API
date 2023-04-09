using Newtonsoft.Json;
using PetStays_API.Exceptions;
using System.Net;

namespace PetStays_API.Helpers
{
    public class ErrorHandling
    {
        private readonly RequestDelegate _next;

        public ErrorHandling(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var errorCode = "Something went wrong";
            var statusCode = HttpStatusCode.InternalServerError;

            switch (ex)
            {
                case BadReqException e:
                    statusCode = HttpStatusCode.BadRequest;
                    errorCode = e.Message;
                    break;

                case NotFoundException e:
                    statusCode = HttpStatusCode.NotFound;
                    errorCode = e.Message;
                    break;

                case ConflictException e:
                    statusCode = HttpStatusCode.Conflict;
                    errorCode = e.Message;
                    break;

                case UnAuthorisedException e:
                    statusCode = HttpStatusCode.Unauthorized;
                    errorCode = e.Message;
                    break;

                case ServerErrorException e:
                    statusCode = HttpStatusCode.InternalServerError;
                    errorCode = e.Message;
                    break;
            }

            string result = JsonConvert.SerializeObject(new { code = statusCode, message = errorCode });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(result);
        }
    }
}
