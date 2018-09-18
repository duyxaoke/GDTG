using Core.DTO.Response;
using System.Reflection;
using System.Web.Http;

namespace Web.Helpers
{
    public static class ApiHelper
    {
        public static IHttpActionResult ReturnHttpAction<T>(T result, object parentClass)
        {
            var objApiController = (ApiController)parentClass;
            var statusCodePro = result.GetType().GetProperty("StatusCode");
            var errorMessagePro = result.GetType().GetProperty("ErrorMessage");
            var dataPro = result.GetType().GetProperty("Data");
            var statusCode = (CRUDStatusCodeRes)statusCodePro.GetValue(result, null);

            if (statusCode == CRUDStatusCodeRes.Success)
            {//0 = 200
                MethodInfo method = objApiController.GetType().GetMethod("CCOk");
                if (dataPro == null)//PagingResponse
                    return (IHttpActionResult)method.Invoke(objApiController, new object[] { result });
                else//CRUDResult
                    return (IHttpActionResult)method.Invoke(objApiController, new object[] { dataPro.GetValue(result, null) });
            }
            else if (statusCode == CRUDStatusCodeRes.ReturnWithData)
            {//7 = 201
                MethodInfo method = objApiController.GetType().GetMethod("CCCreated");
                var data = dataPro.GetValue(result, null);
                return (IHttpActionResult)method.Invoke(objApiController, new object[] { data });
            }
            else if (statusCode == CRUDStatusCodeRes.ResourceNotFound)
            {//1 = 204
                MethodInfo method = objApiController.GetType().GetMethod("CCNoContent");
                return (IHttpActionResult)method.Invoke(objApiController, new object[] { });
            }
            else if (statusCode == CRUDStatusCodeRes.InvalidData || statusCode == CRUDStatusCodeRes.ResetContent)
            {//6 = 406 (InvalidData = NotAcceptable) || 8 = 205
                //ModelStateDictionary modelStateDictionary = new ModelStateDictionary();
                //modelStateDictionary.AddModelError("ErrorMessage", (string)errorMessagePro.GetValue(result, null));
                var errorMessage = (string)errorMessagePro.GetValue(result, null);
                MethodInfo method = objApiController.GetType().GetMethod("CCNotAcceptable");
                return (IHttpActionResult)method.Invoke(objApiController, new object[] { errorMessage });
            }
            else
            {//500
                MethodInfo method = objApiController.GetType().GetMethod("CCInternalServerError", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                return (IHttpActionResult)method.Invoke(objApiController, new object[] { });
            }
        }
    }
}