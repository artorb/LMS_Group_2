using Lms.Core.Models.ViewModels;
using LmsApi.Core.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Web.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IHttpClientFactory clientFactory;

        public AuthorsController(IHttpClientFactory httpClientFactory)
        {
            clientFactory = httpClientFactory;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorListVM viewModel)
        {
            var locationUri = "";
            if (TempData.ContainsKey("LocationUriFromLiteratureCreate"))
            {
                locationUri = TempData["LocationUriFromLiteratureCreate"].ToString();
                TempData.Keep("LocationUriFromLiteratureCreate"); // Marks the specified key in the TempData for retention.
            }
            else
            {
                locationUri = TempData["RequestUriFromLiteratureEdit"].ToString();
                TempData.Keep("LocationUriFromLiteratureCreate"); // Marks the specified key in the TempData for retention.
            }

            if (ModelState.IsValid)
            {
                var client = clientFactory.CreateClient("BaseClient");
                client.BaseAddress = new Uri(locationUri + "/");

                var requestMsg = new HttpRequestMessage(HttpMethod.Post, "authors/collection");

                var tempList = new List<AuthorForCreateUpdateDto>();
                foreach (var author in viewModel.Authors)
                {
                    var tempObj = new AuthorForCreateUpdateDto
                    {
                        BirthDate = author.BirthDate,
                        FirstName = author.FirstName,
                        LastName = author.LastName
                    };
                    tempList.Add(tempObj);
                }
                var data = JsonConvert.SerializeObject(tempList);

                requestMsg.Content = new StringContent(data, Encoding.UTF8, "application/json");      

                var responseMsg = await client.SendAsync(requestMsg);
                if (responseMsg.IsSuccessStatusCode)
                {
                    TempData.Remove("LocationUriFromLiteratureCreate");
                    TempData.Remove("RequestUriFromLiteratureEdit");
                    return RedirectToAction("Index", "Literatures");
                }
                else
                {
                    ModelState.AddModelError("CreateAuthorResponseError", $"A server error occured at {nameof(Create)} with status code {responseMsg.StatusCode}.");
                }
            }
            return View(viewModel);
        }
    }
}
