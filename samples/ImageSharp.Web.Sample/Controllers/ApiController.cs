using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp;

namespace ImageSharp.Web.Sample.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
            private static readonly HttpClient _httpClient = new HttpClient();

        [HttpGet("resize")]
        public async Task<ActionResult> GetResize(string url)
        {
            var path = "http://localhost:2647/imagesharp-logo.png?width=";
            Task[] tasks = Enumerable.Range(1, 25).Select(i => Task.Run(async () =>
              {
                  Random rnd = new Random();
                  var command = path + rnd.Next(100, 1000);
                //   command = path + i;
                  using HttpResponseMessage response = await _httpClient.GetAsync(command);
              })).ToArray();

            var all = Task.WhenAll(tasks);
            await all;


            Configuration.Default.MemoryAllocator.ReleaseRetainedResources();



            return Ok();
        }

        [HttpGet("release")]
        public ActionResult GetRelease()
        {
            Configuration.Default.MemoryAllocator.ReleaseRetainedResources();



            return Ok();
        }        
    }
}