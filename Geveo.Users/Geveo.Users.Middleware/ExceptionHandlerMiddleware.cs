using Geveo.Users.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Net;

namespace Geveo.Users.Middleware
{
	public class ExceptionHandlerMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionHandlerMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context, ILoggerService loggerService)
		{
			try
			{
				await _next(context);
			}
			catch(DbUpdateException ex)
			{
				loggerService.LogError(ex.Message);
				await HandleDbUpdateExceptionAsync(context, loggerService, ex, HttpStatusCode.Conflict);
			}
			catch(UnauthorizedAccessException ex)
			{
				loggerService.LogError(ex.Message);
				await HandleExceptionAsync(context, loggerService, ex, HttpStatusCode.Unauthorized);
			}
			catch(Exception ex)
			{
				loggerService.LogError(ex.Message);
				await HandleExceptionAsync(context, loggerService, ex, HttpStatusCode.BadRequest);
			}
		}

		private async Task HandleDbUpdateExceptionAsync(HttpContext context, ILoggerService loggerService, Exception ex, HttpStatusCode statusCode)
		{
			if(ex is DbUpdateConcurrencyException)
			{
				statusCode = HttpStatusCode.Conflict;
			}
			else if(ex.InnerException != null && ex.InnerException is SqlException)
			{
				var sqlEx = ex.InnerException as SqlException;

				switch(sqlEx.Number)
				{
					case 2601:
						// duplicate value in input
						statusCode = HttpStatusCode.BadRequest; 
						break;
					case 547:
						// cannot delete this entry as other things are attached to it
						statusCode = HttpStatusCode.Conflict;
						break;
					case 24141:
					case 24201:
					case 24202:
						// there are problems with the input values
						statusCode = HttpStatusCode.BadRequest;
						break;
					default:
						break;
				}
			}

			await HandleExceptionAsync(context, loggerService, ex, statusCode);
		}

		private async Task HandleExceptionAsync(HttpContext context, ILoggerService loggerService, Exception exception, HttpStatusCode statusCode)
		{
			loggerService.LogError(exception.Message);
			HttpResponse response = context.Response;

			response.StatusCode = (int)statusCode;
			response.ContentType = "application/json";
			response.Headers["error"] = exception.Message;

			await Task.CompletedTask;
		}
	}
}
