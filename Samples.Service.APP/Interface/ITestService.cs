using Samples.Service.APP.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.Service.APP.Interface
{
    public interface ITestService
    {
        /// <summary>
        /// 
        /// </summary>
        void PdfDemo();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rq"></param>
        void TestAutoMapper(ChattingRetryAddCustomerParamDto rq);
        /// <summary>
        /// 
        /// </summary>
        void TestAutowired();
    }
}
