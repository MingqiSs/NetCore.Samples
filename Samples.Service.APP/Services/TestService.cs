using AutoMapper;
using Infrastructure.Utilities.PDFReport;
using Samples.Service.APP.AutoMapper;
using Samples.Service.APP.BaseProvider;
using Samples.Service.APP.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.Service.APP.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class TestService : BaseSerivce, ITestService
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly IMapper _mapper;
        public ICommonService _commonService { get; set; }
        public TestService(IMapper mapper)
        {
            _mapper = mapper;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        public void PdfDemo()
        {
            string tempFilePath = @"D:\demo\test1.pdf";
            string createdPdfPath = @"D:\demo\test2.pdf";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            PdfHelper.PutContentV2(tempFilePath, createdPdfPath, parameters);
        }
        /// <summary>
        /// 测试AutoMapper
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        public  void TestAutoMapper(ChattingRetryAddCustomerParamDto rq)
        {
            var customerDto = _mapper.Map<ChattingRetryAddCustomerDto>(rq);
        }
        /// <summary>
        /// 测试AutoMapper
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        public void TestAutowired()
        {
            string ermsg = "";
            _commonService.CheckValidationCode("", "", "", out ermsg);
        }
    }
}
