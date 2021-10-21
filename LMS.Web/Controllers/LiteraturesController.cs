using Lms.Core.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Lms.Web.Controllers
{
    public class LiteraturesController : Controller
    {
        private readonly IHttpClientFactory clientFactory;

        public LiteraturesController(IHttpClientFactory httpClientFactory)
        {
            clientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<LiteratureViewModel> literatures = null;

            var client = clientFactory.CreateClient("LiteratureClient"); // The name is definied in Startup.cs
            
            var requestMsg = new HttpRequestMessage(HttpMethod.Get, "literatures?include=true");

            var responseMsg = await client.SendAsync(requestMsg);
            if (responseMsg.IsSuccessStatusCode)
            {
                using (var responseStream = await responseMsg.Content.ReadAsStreamAsync()) 
                {
                    literatures = await DeserializeAsync<IEnumerable<LiteratureViewModel>>(responseStream);
                }
            }
            return View(literatures);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            LiteratureViewModel literature = null;

            var client = clientFactory.CreateClient("LiteratureClient");

            var requestMsg = new HttpRequestMessage(HttpMethod.Get, $"literatures/{id}?include=true");

            var responseMsg = await client.SendAsync(requestMsg);
            if (responseMsg.IsSuccessStatusCode)
            {
                using (var responseStream = await responseMsg.Content.ReadAsStreamAsync())
                {
                    literature = await DeserializeAsync<LiteratureViewModel>(responseStream);
                }
            }
            return View(literature);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LiteratureViewModel model)
        {
            var client = clientFactory.CreateClient("LiteratureClient");

            var requestMsg = new HttpRequestMessage(HttpMethod.Post, "literatures");

            //var serializedModel = Serialize

            var responseMsg = await client.SendAsync(requestMsg);
            if (responseMsg.IsSuccessStatusCode)
            {
                using (var responseStream = await responseMsg.Content.ReadAsStreamAsync())
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        // Source: https://newbedev.com/can-json-net-serialize-deserialize-to-from-a-stream
        private static async Task<T> DeserializeAsync<T>(Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            {
                using (var jsonReader = new JsonTextReader(streamReader))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    return await Task.Run(() => serializer.Deserialize<T>(jsonReader));
                }
            }
        }

        private static async Task Serialize<T>(T entity, Stream stream)
        {
            using (var streamWriter = new StreamWriter(stream))
            {
                using (var jsonWriter = new JsonTextWriter(streamWriter))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    await Task.Run(() => serializer.Serialize(jsonWriter, entity));
                    jsonWriter.Flush();
                }
            }
        }

    }
}
