using Core.Data;
using Service;
using Shared.Models;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using Web.Helpers;
using Web.Infrastructure;
using WebApiThrottle;

namespace Web.Api
{
    [ApiAuthorizeAttribute]
    [RoutePrefix("api/Menus")]
    public class MenusController : ApiControllerBase
    {
        private readonly IMenuServices _menuServices;

        public MenusController(IMenuServices menuServices)
        {
            _menuServices = menuServices;
        }
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            var result = _menuServices.GetAll();
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var result = _menuServices.GetById(id);
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpPost]
        [Route("Create")]
        [EnableThrottling(PerSecond = 1)]
        public IHttpActionResult Post([FromBody]MenuViewModel model)
        {
            var result = _menuServices.Add(model);
            _menuServices.Save();
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpPut]
        [Route("Update")]
        [EnableThrottling(PerSecond = 1)]
        public IHttpActionResult Put([FromBody]MenuViewModel model)
        {
            var result = _menuServices.Update(model);
            _menuServices.Save();
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpDelete]
        [EnableThrottling(PerSecond = 1)]
        public IHttpActionResult Delete(int id)
        {
            var result = _menuServices.Delete(id);
            _menuServices.Save();
            return ApiHelper.ReturnHttpAction(result, this);
        }

    }
}
