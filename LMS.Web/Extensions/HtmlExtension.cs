using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lms.Web.Extensions
{
    public static class HtmlExtension
    {
        public static MvcForm BeginFormActivity(
            this IHtmlHelper htmlHelper,
            string actionName,
            string controllerName,
            object routeValues,
            FormMethod method,
            object htmlAttributes)
        {
            if (htmlHelper == null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            return htmlHelper.BeginForm(
                actionName,
                controllerName,
                routeValues: routeValues,
                method: method,
                antiforgery: null,
                htmlAttributes: htmlAttributes);
        }
    }
}