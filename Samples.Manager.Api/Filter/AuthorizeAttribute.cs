using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Samples.Manager.Api.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public class JwtAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public JwtAuthorizeAttribute() : base()
        {

        }
    }
    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class PermissionCodeAttribute : Attribute
    {
        public List<string> Codes = new List<string>();
        PermissionCodeAttribute() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codes"></param>
        public PermissionCodeAttribute(params string[] codes)
        {
            Codes.AddRange(codes.ToList());
        }

    }
}
