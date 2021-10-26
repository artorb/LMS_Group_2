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

        public async Task<IActionResult> Filter(string searchQuery, int selectId)
        {
            IEnumerable<LiteratureViewModel> literatures = null;

            var client = clientFactory.CreateClient("BaseClient"); // The name is definied in Startup.cs

            var requestMsg = new HttpRequestMessage(HttpMethod.Get, $"literatures/filter?include=true&searchQuery={searchQuery}&selectId={selectId}");

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
                ModelState.AddModelError(string.Empty, $"A server error occured at {nameof(Filter)} with status code {responseMsg.StatusCode}.");
            }
            return View(nameof(Index), literatures);
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<LiteratureViewModel> literatures = null;

            var client = clientFactory.CreateClient("BaseClient"); // The name is definied in Startup.cs
            
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
                ModelState.AddModelError(string.Empty, $"A server error occured at {nameof(Index)} with status code {responseMsg.StatusCode}.");
            }
            return View(literatures);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            LiteratureViewModel literature = null;

            var client = clientFactory.CreateClient("BaseClient");

            var requestMsg = new HttpRequestMessage(HttpMethod.Get, $"literatures/{id}?include=true");

            var responseMsg = await client.SendAsync(requestMsg);
            if (responseMsg.IsSuccessStatusCode)
            {
                using (var responseStream = await responseMsg.Content.ReadAsStreamAsync())
                {
                    literature = await DeserializeAsync<LiteratureViewModel>(responseStream);
                }
            }
            else
            {
                literature = null;
                ModelState.AddModelError(string.Empty, $"A server error occured at {nameof(Details)} with status code {responseMsg.StatusCode}.");
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
            if (ModelState.IsValid)
            {
                var client = clientFactory.CreateClient("BaseClient");

                var requestMsg = new HttpRequestMessage(HttpMethod.Post, "literatures");
                requestMsg.Content = JsonContent.Create(viewModel);

                var responseMsg = await client.SendAsync(requestMsg);
                if (responseMsg.IsSuccessStatusCode)
                {
                    TempData["LocationUri"] = responseMsg.Headers.Location.AbsoluteUri; // used to store the received location uri
                    return RedirectToAction("Create", "Authors");
                }
                else
                {
                    ModelState.AddModelError("CreateResponseError", $"A server error occured at {nameof(Create)} with status code {responseMsg.StatusCode}.");
                }
            }
            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            LiteratureEditViewModel literature = null;

            var client = clientFactory.CreateClient("BaseClient");

            var requestMsg = new HttpRequestMessage(HttpMethod.Get, $"literatures/{id}?include=true");

            var responseMsg = await client.SendAsync(requestMsg);
            if (responseMsg.IsSuccessStatusCode)
            {
                using (var responseStream = await responseMsg.Content.ReadAsStreamAsync())
                {
                    literature = await DeserializeAsync<LiteratureEditViewModel>(responseStream);
                }
            }
            else
            {
                literature = null;
                ModelState.AddModelError(string.Empty, $"A server error occured at {nameof(Edit)} with status code {responseMsg.StatusCode}.");
            }
            
            return View(literature);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LiteratureEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var client = clientFactory.CreateClient("BaseClient");

                var requestMsg = new HttpRequestMessage(HttpMethod.Put, $"literatures/{id}");
                requestMsg.Content = JsonContent.Create(viewModel);

                var responseMsg = await client.SendAsync(requestMsg);
                if (responseMsg.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("EditResponseError", $"A server error occured at {nameof(Edit)} with status code {responseMsg.StatusCode}.");
                }
            }
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            LiteratureViewModel literature = null;

            var client = clientFactory.CreateClient("BaseClient");

            var requestMsg = new HttpRequestMessage(HttpMethod.Get, $"literatures/{id}?include=true");

            var responseMsg = await client.SendAsync(requestMsg);
            if (responseMsg.IsSuccessStatusCode)
            {
                using (var responseStream = await responseMsg.Content.ReadAsStreamAsync())
                {
                    literature = await DeserializeAsync<LiteratureViewModel>(responseStream);
                }
            }
            else
            {
                literature = null;
                ModelState.AddModelError(string.Empty, $"A server error occured at {nameof(Delete)} with status code {responseMsg.StatusCode}.");
            }
            return View(literature);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, LiteratureViewModel viewModel)
        {
            var client = clientFactory.CreateClient("BaseClient");

            var requestMsg = new HttpRequestMessage(HttpMethod.Delete, $"literatures/{id}");
            requestMsg.Content = JsonContent.Create(viewModel);

            var responseMsg = await client.SendAsync(requestMsg);
            if (!responseMsg.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, $"A server error occured at {nameof(Delete)} with status code {responseMsg.StatusCode}.");
            }
            return RedirectToAction(nameof(Index));
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
