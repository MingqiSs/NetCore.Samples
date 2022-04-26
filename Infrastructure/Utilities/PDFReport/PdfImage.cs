using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utilities.PDFReport
{
    /// <summary>
    /// pdf图片操作类
    /// </summary>
    public class PdfImage
    {
        /// <summary>
        /// 是否使用域名
        /// </summary>
        public bool IsField { get; set; }
        /// <summary>
        /// 域名
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 图片URL地址
        /// </summary>
        public string ImageUrl { get; set; } = string.Empty;
        /// <summary>
        /// 图片域宽
        /// </summary>
        public float FitWidth { get; set; }
        /// <summary>
        /// 图片域高
        /// </summary>
        public float FitHeight { get; set; }
        /// <summary>
        /// 绝对X坐标
        /// </summary>
        public float AbsoluteX { get; set; }
        /// <summary>
        /// 绝对Y坐标
        /// </summary>
        public float AbsoluteY { get; set; }
        /// <summary>
        /// Img内容
        /// </summary>
        public byte[] ImgBytes { get; set; }
        /// <summary>
        /// 是否缩放
        /// </summary>
        public bool ScaleParent { get; set; }
        /// <summary>
        /// 页面
        /// </summary>
        public int IPageNum { get; set; }
        /// <summary>
        /// 画布对象
        /// </summary>
        public CanvasRectangle ContainerRect { get; set; }
    }

    /// <summary>
    /// 签名图片
    /// </summary>
    public class SignPdfImage
    {
        /// <summary>
        /// 图片URL地址
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// 图片域宽
        /// </summary>
        public float FitWidth { get; set; }
        /// <summary>
        /// 图片域高
        /// </summary>
        public float FitHeight { get; set; }
        /// <summary>
        /// 绝对X坐标  横坐标，按页面尺寸的百分比计算，取值0.0 - 1.0。以左上角为原点  ps:比如按点计算 页面大小为800*800 ,签名位置为 200*400，则x:0.25(200/800) ,y:0.5(400:800)
        /// </summary>
        public float AbsoluteX { get; set; }
        /// <summary>
        /// 绝对Y坐标 纵坐标，同上
        /// </summary>
        public float AbsoluteY { get; set; }
        /// <summary>
        /// 输出页面 IPageNum=0 为输出最后一页
        /// </summary>
        public int IPageNum { get; set; } = 0;
        /// <summary>
        /// Img内容
        /// </summary>
        public byte[] ImgBytes { get; set; }
        /// <summary>
        /// 是否缩放
        /// </summary>
        public bool ScaleParent { get; set; }
        /// <summary>
        /// 画布
        /// </summary>
        public CanvasRectangle ContainerRect { get; set; }

    }
    public class ContractPdfText
    {

        /// <summary>
        /// 绝对X坐标
        /// </summary>
        public float AbsoluteX { get; set; }
        /// <summary>
        /// 绝对Y坐标
        /// </summary>
        public float AbsoluteY { get; set; }
        /// <summary>
        /// 文案
        /// </summary>
        public string Text { get; set; }
    }
}
