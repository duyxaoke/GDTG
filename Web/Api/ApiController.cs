

using System.Net;
using System.Web.Http;

namespace Web.Api
{
    public class ApiControllerBase : ApiController
    {
        [NonAction]
        public IHttpActionResult CCOk(object content)
        {
            return Ok(content);
        }

        [NonAction]
        public IHttpActionResult CCCreated(object content)
        {
            return base.Created(string.Empty, content);
        }

        [NonAction]
        public IHttpActionResult CCNoContent()
        {
            //protobuf k cho truyền null
            return base.Content(HttpStatusCode.NoContent, "Resource Not Found");
        }

        [NonAction]
        public IHttpActionResult CCNotAcceptable(object content)
        {
            return base.Content(HttpStatusCode.NotAcceptable, content);
        }

        [NonAction]
        public IHttpActionResult CCResetContent(object content)
        {
            return base.Content(HttpStatusCode.ResetContent, content);
        }

        [NonAction]
        public IHttpActionResult CCInternalServerError()
        {
            return base.InternalServerError();
        }
    }
}
