using System;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Web.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base

        protected class JsonNetResult : JsonResult
        {
            public override void ExecuteResult(ControllerContext context)
            {
                if (context == null)
                    throw new ArgumentNullException("context");

                var response = context.HttpContext.Response;

                response.ContentType = !String.IsNullOrEmpty(ContentType)
                    ? ContentType
                    : "application/json";

                if (ContentEncoding != null)
                    response.ContentEncoding = ContentEncoding;

                var serializedObject = JsonConvert.SerializeObject(Data, Formatting.Indented,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                response.Write(serializedObject);
            }
        }

    }
}