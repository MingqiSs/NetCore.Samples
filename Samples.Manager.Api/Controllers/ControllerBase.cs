using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Samples.Manager.Api.Controllers
{
    public class ControllerBase : Controller
    {
        protected ControllerBase() { }

        /// <summary>
        /// 在调用操作方法后调用
        /// </summary>
        /// <param name="context">请求上下文</param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            //context.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            //context.HttpContext.Response.Headers.Add("Access-Control-Allow-Headers", "*");
            //context.HttpContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST,OPTIONS, PUT, DELETE");
        }

        
    }
}
