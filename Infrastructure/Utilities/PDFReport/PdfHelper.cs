using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Infrastructure.Utilities.PDFReport
{
    public class PdfHelper
    {
        private static object PutContentLocked = new object();
        /// <summary>
        /// 写入内容
        /// </summary>
        /// <param name="tempFilePath">模板路径</param>
        /// <param name="createdPdfPath">创建文件路径</param>
        /// <param name="parameters">参数</param>
        /// <param name="pdfImage">签名图片信息</param>
        public static void PutContent(string tempFilePath, string createdPdfPath, Dictionary<string, string> parameters, SignPdfImage pdfImage = null)
        {
            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;
            //  var fontPath = $"{Directory.GetCurrentDirectory()}//wwwroot/fonts/MSYH.TTC";
            try
            {
                pdfReader = tempFilePath.StartsWith("http") ? new PdfReader(new Uri(tempFilePath)) : new PdfReader(tempFilePath);
                using (FileStream fs = new FileStream(createdPdfPath, FileMode.OpenOrCreate))
                {
                    pdfStamper = new PdfStamper(pdfReader, fs);
                    pdfStamper.FormFlattening = true;

                    Image img = null;
                    if (pdfImage != null)
                    {
                        //如果使用网络路径则先将图片转化位绝对路径
                        if (pdfImage.ImageUrl.StartsWith("http"))
                        {
                            pdfImage.ImgBytes = UploadUtils.GetBytesFromUrl(pdfImage.ImageUrl);
                        }
                        if (pdfImage.ImgBytes != null)
                        {
                            img = Image.GetInstance(pdfImage.ImgBytes);
                        }
                        if (img != null)
                        {
                            if (pdfImage.IPageNum == 0) pdfImage.IPageNum = pdfReader.NumberOfPages;
                            if (pdfImage.IPageNum > pdfReader.NumberOfPages) pdfImage.IPageNum = pdfReader.NumberOfPages;//判断大于最大页

                            if (pdfImage.ScaleParent)
                            {
                                var containerRect = pdfImage.ContainerRect;

                                float percentage = 0.0f;
                                percentage = CanvasRectangle.GetPercentage(img.Width, img.Height, containerRect);
                                img.ScalePercent(percentage * 100);

                                pdfImage.AbsoluteX = (containerRect.RectWidth - img.Width * percentage) / 2 + containerRect.StartX;
                                pdfImage.AbsoluteY = (containerRect.RectHeight - img.Height * percentage) / 2 + containerRect.StartY;
                            }
                            else
                            {
                                img.ScaleToFit(pdfImage.FitWidth, pdfImage.FitHeight);
                                var fieldPositions = pdfReader.AcroFields.GetFieldPositions("%sign%");
                                if (fieldPositions != null)
                                {
                                    pdfImage.IPageNum = (int)fieldPositions[0];
                                    pdfImage.AbsoluteX = (int)Math.Ceiling(fieldPositions[1]);
                                    pdfImage.AbsoluteY = (int)Math.Ceiling(fieldPositions[2]);


                                }
                            }
                            var pdfContentByte = pdfStamper.GetOverContent(pdfImage.IPageNum);//获取PDF指定页面内容
                            img.SetAbsolutePosition(pdfImage.AbsoluteX, pdfImage.AbsoluteY);
                            pdfContentByte.AddImage(img);
                        }
                    }

                    if (parameters != null && parameters.Count > 0)
                    {
                        AcroFields pdfFormFields = pdfStamper.AcroFields;

                        #region 指定位置输出文本数据
                        //BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        //BaseFont simheiBase = BaseFont.CreateFont(prefixFont, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        //iTextSharp.text.Font font = new iTextSharp.text.Font(simheiBase, 10);
                        //Phrase p = new Phrase(pdfText.Text, font);
                        //ColumnText.ShowTextAligned(pdfContentByte, Element.ALIGN_CENTER, p, pdfText.AbsoluteX, pdfText.AbsoluteY, 0);
                        #endregion
                        BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        pdfFormFields.AddSubstitutionFont(baseFont);
                        foreach (KeyValuePair<string, string> parameter in parameters)
                        {
                            var key = $"%{parameter.Key}%";
                            if (pdfFormFields.Fields[key] != null)
                            {
                                pdfFormFields.SetField(key, parameter.Value);
                            }

                        }
                    }

                    pdfStamper.Close();
                    pdfReader.Close();
                }
            }
            catch (Exception)
            {
                // throw ex;
            }
            finally
            {
                if (pdfStamper != null)
                {
                    pdfStamper?.Close();
                }

                if (pdfReader != null)
                {
                    pdfReader?.Close();
                }
            }

        }
        /// <summary>
        /// 写入内容
        /// </summary>
        /// <param name="tempFilePath">模板路径</param>
        /// <param name="createdPdfPath">创建文件路径</param>
        /// <param name="parameters">参数</param>
        /// <param name="pdfImages">填充图片信息</param>
        public static void PutContentV2(string tempFilePath, string createdPdfPath, Dictionary<string, string> parameters, List<PdfImage> pdfImages = null)
        {
            lock (PutContentLocked)
            {
                PdfReader pdfReader = tempFilePath.StartsWith("http") ? new PdfReader(new Uri(tempFilePath)) : new PdfReader(tempFilePath);
                using (var fs = new FileStream(createdPdfPath, FileMode.OpenOrCreate))
                {
                    PdfStamper pdfStamper = new PdfStamper(pdfReader, fs);
                    pdfStamper.FormFlattening = true;

                    if (pdfImages != null && pdfImages.Count > 0)
                    {
                        Image img = null;
                        foreach (var pdfImage in pdfImages)
                        {
                            //try
                            //{
                            //如果使用网络路径则先将图片转化位绝对路径

                            if (pdfImage.ImageUrl.StartsWith("http"))
                            {
                                //img = Image.GetInstance(new Uri(pdfImage.ImageUrl));

                                pdfImage.ImgBytes = UploadUtils.GetBytesFromUrl(pdfImage.ImageUrl);
                                //  img = Image.GetInstance(pdfImage.ImgBytes);

                            }
                            if (pdfImage.ImgBytes != null)
                            {
                                img = Image.GetInstance(pdfImage.ImgBytes);
                            }
                            //}
                            //catch (Exception ex)
                            //{
                            //    //实例化图片失败
                            //    continue;
                            //}
                            if (img != null)
                            {
                                if (pdfImage.IPageNum == 0) pdfImage.IPageNum = pdfReader.NumberOfPages;
                                if (pdfImage.IPageNum > pdfReader.NumberOfPages) pdfImage.IPageNum = pdfReader.NumberOfPages;//判断大于最大页

                                if (pdfImage.IsField)
                                {
                                    img.ScaleToFit(pdfImage.FitWidth, pdfImage.FitHeight);
                                    var fieldPositions = pdfReader.AcroFields.GetFieldPositions($"%{pdfImage.FieldName}%");

                                    if (fieldPositions != null)
                                    {
                                        var skip = 5;
                                        var img_count = fieldPositions.Length / skip;
                                        var img_flag = 0;
                                        for (int i = 0; i < img_count; i++)
                                        {
                                            pdfImage.IPageNum = (int)fieldPositions[img_flag];
                                            pdfImage.AbsoluteX = (int)Math.Ceiling(fieldPositions[img_flag + 1]);
                                            pdfImage.AbsoluteY = (int)Math.Ceiling(fieldPositions[img_flag + 2]);

                                            var pdfContentByte = pdfStamper.GetOverContent(pdfImage.IPageNum);//获取PDF指定页面内容
                                            img.SetAbsolutePosition(pdfImage.AbsoluteX, pdfImage.AbsoluteY);
                                            pdfContentByte.AddImage(img);

                                            img_flag += skip;
                                        }
                                        //pdfImage.IPageNum = (int)fieldPositions[0];
                                        //pdfImage.AbsoluteX = (int)Math.Ceiling(fieldPositions[1]);
                                        //pdfImage.AbsoluteY = (int)Math.Ceiling(fieldPositions[2]);

                                        //var pdfContentByte = pdfStamper.GetOverContent(pdfImage.IPageNum);//获取PDF指定页面内容
                                        //img.SetAbsolutePosition(pdfImage.AbsoluteX, pdfImage.AbsoluteY);
                                        //pdfContentByte.AddImage(img);

                                    }
                                }
                                else
                                {
                                    var containerRect = pdfImage.ContainerRect;

                                    float percentage = 0.0f;
                                    percentage = CanvasRectangle.GetPercentage(img.Width, img.Height, containerRect);
                                    img.ScalePercent(percentage * 100);

                                    pdfImage.AbsoluteX = (containerRect.RectWidth - img.Width * percentage) / 2 + containerRect.StartX;
                                    pdfImage.AbsoluteY = (containerRect.RectHeight - img.Height * percentage) / 2 + containerRect.StartY;
                                    var pdfContentByte = pdfStamper.GetOverContent(pdfImage.IPageNum);//获取PDF指定页面内容
                                    img.SetAbsolutePosition(pdfImage.AbsoluteX, pdfImage.AbsoluteY);
                                    pdfContentByte.AddImage(img);
                                }

                            }
                        }
                    }

                    if (parameters != null && parameters.Count > 0)
                    {
                        AcroFields pdfFormFields = pdfStamper.AcroFields;
                        BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        pdfFormFields.AddSubstitutionFont(baseFont);
                        foreach (KeyValuePair<string, string> parameter in parameters)
                        {
                            var key = $"%{parameter.Key}%";
                            if (pdfFormFields.Fields[key] != null)
                            {
                                pdfFormFields.SetField(key, parameter.Value);
                            }

                        }
                    }
                    pdfStamper.Close();
                    pdfReader.Close();
                }
            }

        }
        public static void HtmlToPDF(string pdfFilePath,string text)
        {

            var stream = new FileStream(pdfFilePath, FileMode.Create);

            // create a StyleSheet
            var styles = new StyleSheet();
            // set the default font's properties
            styles.LoadTagStyle(HtmlTags.BODY, "encoding", "Identity-H");
            styles.LoadTagStyle(HtmlTags.BODY, HtmlTags.FONT, "Tahoma");
            styles.LoadTagStyle(HtmlTags.BODY, "size", "16pt");


            FontFactory.Register("");

            var unicodeFontProvider = FontFactoryImp.Instance;
            unicodeFontProvider.DefaultEmbedding = BaseFont.EMBEDDED;
            unicodeFontProvider.DefaultEncoding = BaseFont.IDENTITY_H;

            var props = new Hashtable
            {
                { "font_factory", unicodeFontProvider } // Always use Unicode fonts
            };

            // step 1
            var document = new Document();
            // step 2
            PdfWriter.GetInstance(document, stream);
            // step 3
            document.AddAuthor("");
            document.Open();
            // step 4
            var objects = HtmlWorker.ParseToList(
                new StringReader(text),
                styles,
                props
            );
            foreach (IElement element in objects)
            {
                document.Add(element);
            }

            document.Close();
            stream.Dispose();
        }
        /// <summary>
        /// 合成pdf文件
        /// </summary>
        /// <param name="fileList">文件名list</param>
        /// <param name="outMergeFile">输出路径</param>
        public static void MergePdf(List<string> fileList, string createdPdfPath)
        {
            iTextSharp.text.Document document = null;
            try
            {
                PdfReader reader;
                if (fileList.Count > 1)
                {
                    //此处将内容从文本提取至文件流中的目的是避免文件被占用,无法删除
                    FileStream fs1 = new FileStream(fileList[0], FileMode.Open);
                    byte[] bytes1 = new byte[(int)fs1.Length];
                    fs1.Read(bytes1, 0, bytes1.Length);
                    fs1.Close();
                    reader = new PdfReader(bytes1);
                    reader.GetPageSize(1);
                    iTextSharp.text.Rectangle rec = reader.GetPageSize(1);
                    document = new iTextSharp.text.Document(rec, 50, 50, 50, 50);
                    using (FileStream f = new FileStream(createdPdfPath, FileMode.OpenOrCreate))
                    {
                        PdfReader.AllowOpenWithFullPermissions = true;
                        PdfWriter writer = PdfWriter.GetInstance(document, f);
                        document.Open();
                        PdfContentByte cb = writer.DirectContent;
                        PdfImportedPage newPage;
                        for (int i = 0; i < fileList.Count; i++)
                        {
                            FileStream fs = new FileStream(fileList[i], FileMode.Open);
                            byte[] bytes = new byte[(int)fs.Length];
                            fs.Read(bytes, 0, bytes.Length);
                            fs.Close();
                            reader = new PdfReader(bytes);
                            int iPageNum = reader.NumberOfPages;
                            for (int j = 1; j <= iPageNum; j++)
                            {
                                document.NewPage();
                                newPage = writer.GetImportedPage(reader, j);
                                cb.AddTemplate(newPage, 0, 0);

                            }
                            // File.Delete(fileList[i]);
                        }
                        document.Close();
                    }
                }

            }
            catch (Exception)
            {

            }
            finally
            {
                if (document != null)
                {
                    document.Close();
                }
            }

        }
 

    }

}
