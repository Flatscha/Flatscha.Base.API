using Microsoft.AspNetCore.Http;

namespace Flatscha.Base.API.Extensions
{
    public static class ResultExtensions
    {
        public static async Task<IResult> GetFileStreamResult(this Task<Stream> streamTask, string contentType = "application/pdf")
        {
            var stream = await streamTask;

            if (stream is null) { return Results.NotFound(); }

            return Results.File(stream, contentType);
        }
    }
}
