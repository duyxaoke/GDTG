using Core.Data;
using Service;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using Web.Helpers;
using Web.Infrastructure;
using WebApiThrottle;

namespace Web.Api
{
    [ApiAuthorizeAttribute]
    [RoutePrefix("api/configs")]
    public class ConfigsController : ApiControllerBase
    {
        private readonly IConfigServices _configServices;

        public ConfigsController(IConfigServices configServices)
        {
            _configServices = configServices;
        }
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            var result = _configServices.GetAll();
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var result = _configServices.GetById(id);
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpPost]
        [Route("Create")]
        [EnableThrottling(PerSecond = 1)]
        public IHttpActionResult Post([FromBody]Config model)
        {
            var result = _configServices.Create(model);
            _configServices.Save();
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpPut]
        [Route("Update")]
        [EnableThrottling(PerSecond = 1)]
        public IHttpActionResult Put([FromBody]Config model)
        {
            var result = _configServices.Update(model);
            _configServices.Save();
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpDelete]
        [EnableThrottling(PerSecond = 1)]
        public IHttpActionResult Delete(int id)
        {
            var result = _configServices.Delete(id);
            _configServices.Save();
            return ApiHelper.ReturnHttpAction(result, this);
        }

    }
}
