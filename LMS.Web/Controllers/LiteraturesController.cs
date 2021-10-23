using Lms.Core.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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
            else
            {
                literatures = Enumerable.Empty<LiteratureViewModel>();

                ModelState.AddModelError(string.Empty, $"Server error at {nameof(Index)}. Status code: {responseMsg.StatusCode}.");
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
        public async Task<IActionResult> Create(LiteratureCreateViewModel viewModel)
        {
            if (viewModel.Authors.Count > 0 && viewModel.Authors[0].FirstName == null)
            {
                viewModel.Authors.Clear();
            }
            ViewBag.AuthorsCount = viewModel.Authors.Count();
            
            var client = clientFactory.CreateClient("LiteratureClient");

            var requestMsg = new HttpRequestMessage(HttpMethod.Post, "literatures");

            requestMsg.Content = JsonContent.Create(viewModel);

            var responseMsg = await client.SendAsync(requestMsg);
            if (responseMsg.IsSuccessStatusCode)
            {
                //ModelState.AddModelError(string.Empty, $"{responseMsg.Content}");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(string.Empty, $"Server error at {nameof(Create)}. Status code: {responseMsg.StatusCode}.");
            }
            return View(viewModel);
        }

        // Source: https://newbedev.com/can-json-net-serialize-deserialize-to-from-a-stream
        public static async Task<T> DeserializeAsync<T>(Stream stream)
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

        private static async Task SerializeAsync(object entity)
        {
            MemoryStream stream = new MemoryStream();
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
