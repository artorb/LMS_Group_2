using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lms.Web.Extensions
{
    public static class RequestExtension
    {
        public static bool isAjax(this HttpRequest request) {
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";               
        }
    }
}
