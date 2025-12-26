using Aspose.Words;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
namespace EduAdmin.LocalTools
{
    public class OfficeConvert
    {
        private static string getLibreOfficePath()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    return "/usr/bin/soffice";
                case PlatformID.Win32NT:
                    string binaryDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    return binaryDirectory + "\\Windows\\program\\soffice.exe";
                default:
                    throw new PlatformNotSupportedException("你的系统暂不支持！");
            }
        }
        /// <summary>
        /// 转成PDF
        /// </summary>
        /// <param name="officePath">待转换文件路径</param>
        /// <param name="outPutPath">导出文件地址</param>
        /// <return>返回本地路径</return>
        public static string ToPdf(string officePath, string outPutPath)
        {
            if (!Directory.Exists(outPutPath))
                Directory.CreateDirectory(outPutPath);
            //获取libreoffice命令的路径
            string libreOfficePath = getLibreOfficePath();
            ProcessStartInfo procStartInfo = new ProcessStartInfo(libreOfficePath, string.Format("--convert-to pdf --outdir {0} --nologo {1}", outPutPath, officePath));
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;
            procStartInfo.WorkingDirectory = Environment.CurrentDirectory;

            //开启线程
            Process process = new Process() { StartInfo = procStartInfo, };
            process.Start();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new LibreOfficeFailedException(process.ExitCode);
            }
            return Path.Combine(outPutPath, Path.GetFileNameWithoutExtension(officePath) + ".pdf");
        }
        /// <summary>
        /// 判断文件是否已经存在转换的PDF文件
        /// </summary>
        /// <param name="officePath">待转换文件路径</param>
        /// <param name="outPutPath">导出文件地址</param>
        /// <returns></returns>
        public static bool PdfExists(string officePath, string outPutPath)
        {
            return File.Exists(Path.Combine(outPutPath, Path.GetFileNameWithoutExtension(officePath) + ".pdf"));
        }
        /// <summary>
        /// （弃用）word文件转成PDF(有 Aspose 官方的水印)
        /// </summary>
        /// <param name="wordPath">待转换word文件路径</param>
        /// <param name="outPutPath">导出的word文件路径</param>
        private static void WordToPdf(string wordPath, string outPutPath)
        {
            Document doc = new Document(wordPath);
            doc.Save(outPutPath, SaveFormat.Pdf);//还可以改成其它格式
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="pdfPath"></param>
        /// <param name="localPath"></param>
        /// <param name="webPath"></param>
        /// <returns></returns>
        public static string GetFilePdf(string file, string pdfPath, string localPath, string webPath)
        {
            file = file.Replace(webPath, localPath);
            if (File.Exists(file))
            {
                //如果转过了就可以直接返回
                string pdfurl = Path.Combine(pdfPath, Path.GetFileNameWithoutExtension(file) + ".pdf");
                if (File.Exists(pdfurl))
                {
                    pdfurl = pdfurl.Replace(localPath, webPath);
                    return pdfurl;
                }
                var extension = Path.GetExtension(file);
                if (extension == ".doc" || extension == ".docx" || extension == ".xls" || extension == ".xlsx")
                 {
                    ToPdf(file, pdfPath);
                    pdfPath = Path.Combine(pdfPath, Path.GetFileNameWithoutExtension(file) + ".pdf");
                    pdfPath = pdfPath.Replace(localPath, webPath);
                    return pdfPath;
                }
            }
            return "";
        }

        public class LibreOfficeFailedException : Exception
        {
            public LibreOfficeFailedException(int exitCode)
                : base(string.Format("LibreOffice错误 {0}", exitCode))
            { }
        }
    }
}
