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
    /// pdf文本域操作类
    /// </summary>
    public class PdfText
    {


        #region  pdf模板文本域复制
        /// <summary>
        /// 指定pdf模板为其文本域赋值
        /// </summary>
        /// <param name="pdfTemplate">pdf模板路径</param>
        /// <param name="tempFilePath">pdf导出路径</param>
        /// <param name="parameters">pdf模板域键值</param>
        public static void PutText(string pdfTemplate, string tempFilePath, Dictionary<string, string> parameters)
        {
            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;
          //  var prefixFont = $"{Directory.GetCurrentDirectory()}//wwwroot/Fonts/SIMHEI.TTF";
            try
            {
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
                pdfReader = new PdfReader(pdfTemplate);
                using (FileStream fs = new FileStream(tempFilePath, FileMode.OpenOrCreate))
                {
                    pdfStamper = new PdfStamper(pdfReader, fs);
                    int iPageNum = pdfReader.NumberOfPages;
                    var pdfContentByte = pdfStamper.GetOverContent(iPageNum);//获取PDF指定页面内容
                    AcroFields pdfFormFields = pdfStamper.AcroFields;
                    pdfStamper.FormFlattening = true;

                    BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    pdfFormFields.AddSubstitutionFont(bf);

                    foreach (KeyValuePair<string, string> parameter in parameters)
                    {
                        var key = $"%{parameter.Key}%";
                        if (pdfFormFields.Fields[key] != null)
                        {
                            pdfFormFields.SetField(key, parameter.Value);
                        }
                    }

                    pdfStamper.Close();
                    pdfReader.Close();
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pdfStamper.Close();
                pdfReader.Close();
            }
        }
        #endregion
        /// <summary>
        /// 指定pdf模板为其文本域赋值
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="parameters"></param>
        public void PutText(string FilePath, Dictionary<string, string> parameters)
        {
            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;

            try
            {
                if (!File.Exists(FilePath))
                {
                    return;
                }
                pdfReader = new PdfReader(FilePath);
                pdfStamper = new PdfStamper(pdfReader, new FileStream(FilePath, FileMode.OpenOrCreate));

                AcroFields pdfFormFields = pdfStamper.AcroFields;
                pdfStamper.FormFlattening = true;

                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                BaseFont simheiBase = BaseFont.CreateFont(@"C:\Windows\Fonts\simhei.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                pdfFormFields.AddSubstitutionFont(simheiBase);

                foreach (KeyValuePair<string, string> parameter in parameters)
                {
                    if (pdfFormFields.Fields[parameter.Key] != null)
                    {
                        pdfFormFields.SetField(parameter.Key, parameter.Value);
                    }
                }

            }
            catch (Exception)
            {
            }
            finally
            {
                pdfStamper.Close();
                pdfReader.Close();
            }
        }
    }
}
