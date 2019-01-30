using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BSM.Api.middleware
{
    public class JsonExceptionMiddleware
    {
        private IHostingEnvironment _env;
        public JsonExceptionMiddleware(IHostingEnvironment env)
        {
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
            if (ex == null) return;

            var error = new {
                Message = ex.Message,
                Detail = ex.Message
            };

            if (_env.IsDevelopment())
            {
                error = new
                {
                    Message = ex.Message,
                    Detail = ex.StackTrace
                };
            }

            context.Response.ContentType = "application/json";

            using (var writer = new StreamWriter(context.Response.Body))
            {
                new JsonSerializer().Serialize(writer, error);
                await writer.FlushAsync().ConfigureAwait(false);
            }
        }
    }
}
