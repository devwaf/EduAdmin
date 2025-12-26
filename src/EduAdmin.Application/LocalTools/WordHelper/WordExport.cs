using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EduAdmin.LocalTools.WordHelper
{
    public class WordExport
    {
        /// <summary>
        /// docx文档替换
        /// </summary>
        /// <param name="tempFilePath">docx文件路径</param>
        /// <param name="outFilePath">输出文件路径</param>
        /// <param name="data">字典数据源</param>
        public static void Export(string tempFilePath, string outFilePath, Dictionary<string, string> data)
        {
            using (FileStream stream = File.OpenRead(tempFilePath))
            {
                XWPFDocument doc = new XWPFDocument(stream);

                //遍历段落                  
                foreach (var para in doc.Paragraphs)
                {
                    ReplaceWordKey(para, data);
                }
                //遍历表格      
                foreach (var table in doc.Tables)
                {
                    foreach (var row in table.Rows)
                    {
                        foreach (var cell in row.GetTableCells())
                        {
                            foreach (var para in cell.Paragraphs)
                            {
                                //ReplaceWordKey(para, data);
                                foreach (var key in data.Keys)
                                {
                                    //$$模板中数据占位符为$KEY$
                                    if (para.ParagraphText.Contains(key))
                                    {
                                        para.ReplaceText("$" + key + "$", data[key]);
                                        //text = text.Replace(key, data[key]);
                                    }
                                }
                                string pattern;
                                //上面会将有对应的字段替换掉 下面将没有对应字段的元素删除避免导出文件出现$projectname$之类的待替换字符
                                if (para.ParagraphText.Contains("&"))
                                    pattern = @"\&{1}\w+\&{1}";
                                else if (para.ParagraphText.Contains("$"))
                                    pattern = @"\${1}\w+\${1}";
                                else
                                    continue;
                                Regex rgx = new Regex(pattern);
                                foreach (Match match in rgx.Matches(para.ParagraphText))
                                {
                                    para.ReplaceText(match.Value, "");
                                }
                            }
                        }
                    }
                }
                using (var outFile = new FileStream(outFilePath, FileMode.Create))
                {
                    doc.Write(outFile);
                    //outFile.Close();
                }
                doc.Close();
            }
        }
        private static void ReplaceWordKey(XWPFParagraph para, Dictionary<string, string> data)
        {
            string text = "";
            var xp = para.CreateRun();
            foreach (var run in para.Runs)
            {
                text = run.ToString();

                foreach (var key in data.Keys)
                {
                    //$$模板中数据占位符为$KEY$
                    if (text.Contains(key))
                    {
                        if (key == "\u00A3" || key == "\u0052")
                        {
                            xp.SetFontFamily("Wingdings 2", FontCharRange.Ascii);
                            xp.SetText(data[key]);
                            text = text.Replace("$" + key + "$", "");
                        }
                        else
                        {
                            //run.SetFontFamily("Wingdings 2", FontCharRange.Ascii);
                            text = text.Replace("$" + key + "$", data[key]);
                        }
                    }
                    //if (text.StartsWith("$") && text.EndsWith("$"))
                    //{
                    //    text = text.Replace(key, data[key]);
                    //}
                }
                run.SetText(text, 0);
            }
        }
    }
}
