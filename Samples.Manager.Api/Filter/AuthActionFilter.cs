using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;

namespace Samples.Manager.Api.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthActionFilter : ActionFilterAttribute
    {
        public AuthActionFilter()
        {
        }
        /// <summary>
        /// 方法拦截器
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // 判断请求的控制器和方法有没有加上NoPermissionRequiredAttribute（不需要权限）
            var isDefined = false;
            // 获取请求进来的控制器与方法
            var controllerActionDescriptor = filterContext.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                isDefined = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                  .Any(a => a.GetType().Equals(typeof(AllowAnonymousAttribute)));
            }
            if (isDefined) return;
            // 获取设置的操作码（如果没设置操作码，默认不验证权限）
            var actionCode = (PermissionCodeAttribute)controllerActionDescriptor.MethodInfo
                .GetCustomAttributes(inherit: true)
                .FirstOrDefault(t => t.GetType().Equals(typeof(PermissionCodeAttribute)));
            if (actionCode != null && actionCode.Codes.Count > 0)
            {
                // 权限业务代码 验证是否通过()
                //var list = _authAppService.VerifyPermissions(actionCode.Codes);
                var list = new List<string>() { "Get" };

                bool isVerify = false;
                actionCode.Codes.ForEach(t =>
                {
                    isVerify = list.Where(q => q.Equals(t)).Any();
                    if (isVerify) return;
                });
                if (!isVerify)
                {
                    var data = new
                    {
                        Res = 0,
                        Ec = 0,
                        Dt = 0,
                        Msg = "没有权限，请联系管理员"
                    };
                    filterContext.Result = new OkObjectResult(data);
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
