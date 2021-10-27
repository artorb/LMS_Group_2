using Lms.Core.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Lms.Web.Service
{
    public class LitteratureSelectListService
    {
        private readonly IHttpClientFactory clientFactory;

        public LitteratureSelectListService(IHttpClientFactory httpClientFactory)
        {
            clientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<SelectListItem>> GetCategoriesToSelectListItem()
        {
            IEnumerable<SelectListItem> selectListItems = null;

            var client = clientFactory.CreateClient("BaseClient");

            var requestMsg = new HttpRequestMessage(HttpMethod.Get, "categories");

            var responseMsg = await client.SendAsync(requestMsg);
            if (responseMsg.IsSuccessStatusCode)
            {
                using (var responseStream = await responseMsg.Content.ReadAsStreamAsync())
                {
                    var categories = await DeserializeAsync<IEnumerable<LiteratureSelectListItemVM>>(responseStream);

                    selectListItems = categories.OrderBy(c => c.Text).Select(c =>
                                            new SelectListItem(c.Text, c.Value.ToString()));
                }
            }
            else
            {
                selectListItems = Enumerable.Empty<SelectListItem>();
            }
            return selectListItems;
        }

        public async Task<IEnumerable<SelectListItem>> GetSubjectsToSelectListItem()
        {
            IEnumerable<SelectListItem> selectListItems = null;

            var client = clientFactory.CreateClient("BaseClient");

            var requestMsg = new HttpRequestMessage(HttpMethod.Get, "subjects");

            var responseMsg = await client.SendAsync(requestMsg);
            if (responseMsg.IsSuccessStatusCode)
            {
                using (var responseStream = await responseMsg.Content.ReadAsStreamAsync())
                {
                    var subjects = await DeserializeAsync<IEnumerable<LiteratureSelectListItemVM>>(responseStream);

                    selectListItems = subjects.OrderBy(s => s.Text).Select(s =>
                                            new SelectListItem(s.Text, s.Value.ToString()));
                }
            }
            else
            {
                selectListItems = Enumerable.Empty<SelectListItem>();
            }
            return selectListItems;
        }

        public async Task<IEnumerable<SelectListItem>> GetLevelsToSelectListItem()
        {
            IEnumerable<SelectListItem> selectListItems = null;

            var client = clientFactory.CreateClient("BaseClient");

            var requestMsg = new HttpRequestMessage(HttpMethod.Get, "levels");

            var responseMsg = await client.SendAsync(requestMsg);
            if (responseMsg.IsSuccessStatusCode)
            {
                using (var responseStream = await responseMsg.Content.ReadAsStreamAsync())
                {
                    var levels = await DeserializeAsync<IEnumerable<LiteratureSelectListItemVM>>(responseStream);

                    selectListItems = levels.OrderBy(s => s.Value).Select(s =>
                                            new SelectListItem(s.Text, s.Value.ToString()));
                }
            }
            else
            {
                selectListItems = Enumerable.Empty<SelectListItem>();
            }
            return selectListItems;
        }

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

    }
}
