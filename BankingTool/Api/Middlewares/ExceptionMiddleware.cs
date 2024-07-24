
using Serilog;
using System.Net;

namespace Api.Middlewares
{
	public class ExceptionMiddleware : IMiddleware
	{
		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			try
			{
				await next.Invoke(context);
			}
			catch (Exception ex)
			{
				Log.Error($@"Error occured: {ex}");
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				await context.Response.WriteAsync("Something went wrong. We are working on it");
			}
		}
	}
}
