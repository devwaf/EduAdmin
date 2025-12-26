using Abp.Dependency;
using EduAdmin.AppService.Outlines.Dto;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using PictureType = NPOI.XWPF.UserModel.PictureType;


namespace EduAdmin.LocalTools
{
    public class WordHelp : ISingletonDependency
    {

        public WordHelp()
        {
        }
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
                                    if (para.ParagraphText.Contains("$" + key + "$"))
                                    {
                                        if (key.Contains("Wingdings"))
                                        {
                                            var xp = para.CreateRun();
                                            xp.FontFamily = "Wingdings 2";
                                            //xp.SetFontFamily("Wingdings 2", FontCharRange.Ascii);
                                            //xp.SetText(Convert.ToChar(Int32.Parse(data[key])).ToString() +"吗");
                                            xp.Paragraph.ReplaceText("$" + key + "$", Convert.ToChar(Int32.Parse(data[key])).ToString());
                                            var reptext = xp.Paragraph.Text;
                                            para.ReplaceText(para.Text, "");
                                            if (reptext.Contains("A卷") || reptext.Contains("B卷"))
                                            {
                                                reptext = reptext.Replace("A卷", "第一卷");
                                                reptext = reptext.Replace("B卷", "第二卷");
                                            }
                                            xp.SetText(reptext);
                                            CT_RPr rpr = xp.GetCTR().AddNewRPr(); 
                                            CT_Fonts rfonts = rpr.AddNewRFonts(); 
                                            rfonts.ascii = "微软雅黑"; rfonts.eastAsia = "微软雅黑";
                                            rpr.AddNewSz().val = (ulong)21;//5号字体 rpr.AddNewSzCs().val = (ulong)21;
                                            
                                            //if (reptext.Contains("第一卷") || reptext.Contains("第二卷"))
                                            //{
                                            //    para.ReplaceText("第一卷", "A卷");
                                            //    para.ReplaceText("第二卷", "B卷");
                                            //}
                                        }
                                        else
                                        {
                                            para.ReplaceText("$" + key + "$", data[key]);
                                        }
                                        // para.ReplaceText(key, data[key]);
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
                var dire = Path.GetDirectoryName(outFilePath);
                if (!Directory.Exists(dire))
                {
                    Directory.CreateDirectory(dire);
                }
                using (var outFile = new FileStream(outFilePath, FileMode.Create))
                {
                    doc.Write(outFile);
                    //outFile.Close();
                }
                doc.Close();
            }
        }
        /// <summary>
        /// docx文档替换
        /// </summary>
        /// <param name="tempFilePath">docx文件路径</param>
        /// <param name="outFilePath">输出文件路径</param>
        /// <param name="data">字典数据源</param>
        public static void ExportExam(string tempFilePath, string outFilePath, Dictionary<string, string> data, List<ExamTableDto> ExamTable1, List<ExamTableDto> ExamTable2)
        {
            using (FileStream stream = File.OpenRead(tempFilePath))
            {
                XWPFTableCell xWPFTableCell1 = null;
                XWPFTableCell xWPFTableCell2 = null;
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
                                    if (para.ParagraphText.Contains("$" + key + "$"))
                                    {
                                        if (key.Contains("ExamTable1"))
                                        {
                                            xWPFTableCell1 = cell;
                                        }
                                        if (key.Contains("ExamTable2"))
                                        {
                                            xWPFTableCell2 = cell;
                                        }
                                        if (key.Contains("Wingdings"))
                                        {
                                            var aa = para.CreateRun();//段落需要按顺序创建，用来定位特殊符号转换
                                            var xp = para.CreateRun();
                                            var bb = para.CreateRun();
                                            var xm = para.CreateRun();
                                            var cc = para.CreateRun();
                                            aa.FontSize = 9;
                                            xp.FontSize = 9;
                                            bb.FontSize = 9;
                                            xm.FontSize = 9;
                                            cc.FontSize = 9;
                                            xp.FontFamily = "Wingdings 2";
                                            xm.FontFamily = "Wingdings 2";
                                            //xp.SetFontFamily("Wingdings 2", FontCharRange.Ascii);
                                            //xp.SetText(Convert.ToChar(Int32.Parse(data[key])).ToString() +"吗");
                                            xp.Paragraph.ReplaceText("$" + key + "$", Convert.ToChar(Int32.Parse(data[key])).ToString());
                                            var reptext = xp.Paragraph.Text;
                                            para.ReplaceText(para.Text, "");
                                            if (reptext.Contains("A卷") || reptext.Contains("B卷"))
                                            {
                                                int index = 0;
                                                int index1 = 0;
                                                string str1 = null;
                                                if (reptext.Contains("WingdingsIsA2") || reptext.Contains("WingdingsIsAS2")){
                                                    index = reptext.IndexOf(Convert.ToChar(Int32.Parse(data[key])).ToString());
                                                    if(data[key] == "82")
                                                    {
                                                        str1 = Convert.ToChar(163).ToString();
                                                    }
                                                    else
                                                    {
                                                        str1 = Convert.ToChar(82).ToString();
                                                    }
                                                    index1 = reptext.IndexOf(str1);
                                                    aa.SetText(reptext.Substring(0, index));
                                                    xp.SetText(Convert.ToChar(Int32.Parse(data[key])).ToString());
                                                    cc.SetText(reptext.Substring(index + 1, reptext.Length - index - 1));
                                                }
                                                else
                                                {
                                                    index1 = reptext.IndexOf(Convert.ToChar(Int32.Parse(data[key])).ToString());
                                                    if (data[key] == "82")
                                                    {
                                                        str1 = Convert.ToChar(163).ToString();
                                                    }
                                                    else
                                                    {
                                                        str1 = Convert.ToChar(82).ToString();
                                                    }
                                                    index = reptext.IndexOf(str1);
                                                    aa.SetText(reptext.Substring(0, index));
                                                    xp.SetText(Convert.ToChar(Int32.Parse(data[key])).ToString());
                                                    bb.SetText(reptext.Substring(index + 1, index1- index-1));
                                                    xm.SetText(str1);
                                                    cc.SetText(reptext.Substring(index1 + 1, reptext.Length - index1 - 1));
                                                }
                                                //xp.SetText(reptext);
                                                
                                                continue;
                                                //reptext = reptext.Replace("A卷", "第一卷");
                                                //reptext = reptext.Replace("B卷", "第二卷");
                                            }
                                            xp.SetText(reptext);
                                            //CT_RPr rpr = xp.GetCTR().AddNewRPr();
                                            //CT_Fonts rfonts = rpr.AddNewRFonts();
                                            //rfonts.ascii = "微软雅黑"; rfonts.eastAsia = "微软雅黑";
                                            //rpr.AddNewSz().val = (ulong)21;//5号字体 rpr.AddNewSzCs().val = (ulong)21;

                                            //if (reptext.Contains("第一卷") || reptext.Contains("第二卷"))
                                            //{
                                            //    para.ReplaceText("第一卷", "A卷");
                                            //    para.ReplaceText("第二卷", "B卷");
                                            //}
                                        }
                                        else
                                        {
                                            para.ReplaceText("$" + key + "$", data[key]);
                                        }
                                        // para.ReplaceText(key, data[key]);
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
                //导入表格
                var tableRow = ExamTable1.Count()+1;
                XWPFTable nestedXwpfTable = CreateNestedTableFromTableCell(xWPFTableCell1, 5100, tableRow, 3);
                xWPFTableCell1.RemoveParagraph(0);
                nestedXwpfTable.SetCellMargins(200, 0, 200, 0);
                //xWPFTableCell1.Tables[0].SetCellMargins(200, 200, 200, 200);
                xWPFTableCell1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);
                //nestedXwpfTable.SetColumnWidth(0, 1200);
                EditXWPFTableRow(nestedXwpfTable, 0, 400, r1 =>
                {
                    EditXwpfTableCell(r1, 0, "1200", h1 =>
                    {

                        CreateCellText1(h1, "课程目标");

                        h1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

                    });
                    EditXwpfTableCell(r1, 1, "1200", h1 =>
                    {

                        CreateCellText1(h1, "试题及分值");

                        h1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

                    });
                    EditXwpfTableCell(r1, 2, "1200", h1 =>
                    {

                        CreateCellText1(h1, "是否与教学大纲一致");

                        h1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

                    });
                });
                for (int i = 0; i < ExamTable1.Count(); i++)
                {
                    EditXWPFTableRow(nestedXwpfTable, i+1, 400, r1 =>
                    {
                        EditXwpfTableCell(r1, 0, "1200", h1 =>
                        {

                            CreateCellText1(h1, ExamTable1[i].ObjContent,ParagraphAlignment.LEFT);

                            h1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

                        });
                        EditXwpfTableCell(r1, 1, "1200", h1 =>
                        {
                            h1.RemoveParagraph(0);
                            XWPFParagraph p2 = h1.AddParagraph();
                            var run = p2.CreateRun();
                            p2.Alignment = ParagraphAlignment.CENTER;
                            string content = null;
                            foreach (var ite in ExamTable1[i].QuestionScore)
                            {
                                content = content + ite;
                                content = content + (char)12;
                            }
                            run.FontFamily = "宋体";
                            run.FontSize = 9;
                            run.SetText(content);
                            h1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

                        });
                        EditXwpfTableCell(r1, 2, "1200", h1 =>
                        {
                            foreach(var para in h1.Paragraphs)
                            {
                                var xp = para.CreateRun();
                                xp.FontFamily = "Wingdings 2";
                                xp.FontSize = 9;
                                para.FontAlignment = 2;
                                if(ExamTable1[i].IsPass == 1)
                                {
                                    xp.SetText(Convert.ToChar(82).ToString() + "是" + "   " + Convert.ToChar(163).ToString() + "否");
                                }
                                else if(ExamTable1[i].IsPass == 2)
                                {
                                    xp.SetText(Convert.ToChar(163).ToString() + "是" + "   " + Convert.ToChar(82).ToString() + "否");
                                }
                                else
                                {
                                    xp.SetText(Convert.ToChar(163).ToString() + "是" + "   " + Convert.ToChar(163).ToString() + "否");
                                }
                            }
                            h1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);
                        });
                    });
                }
                //表2
                var tableRow1 = ExamTable2.Count() + 1;
                XWPFTable nestedXwpfTable2 = CreateNestedTableFromTableCell(xWPFTableCell2, 5100, tableRow1, 3);
                nestedXwpfTable2.SetCellMargins(200, 0, 200, 0);
                xWPFTableCell2.RemoveParagraph(0);
                //nestedXwpfTable.SetColumnWidth(0, 1200);
                EditXWPFTableRow(nestedXwpfTable2, 0, 400, r1 =>
                {
                    EditXwpfTableCell(r1, 0, "1200", h1 =>
                    {

                        CreateCellText1(h1, "课程目标");

                        h1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

                    });
                    EditXwpfTableCell(r1, 1, "1200", h1 =>
                    {

                        CreateCellText1(h1, "试题及分值");

                        h1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

                    });
                    EditXwpfTableCell(r1, 2, "1200", h1 =>
                    {

                        CreateCellText1(h1, "是否与教学大纲一致");

                        h1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

                    });
                });
                for (int i = 0; i < ExamTable2.Count(); i++)
                {
                    EditXWPFTableRow(nestedXwpfTable2, i+1, 400, r1 =>
                    {
                        EditXwpfTableCell(r1, 0, "1200", h1 =>
                        {

                            CreateCellText1(h1, ExamTable2[i].ObjContent,ParagraphAlignment.LEFT);

                            h1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

                        });
                        EditXwpfTableCell(r1, 1, "1200", h1 =>
                        {
                            h1.RemoveParagraph(0);
                            XWPFParagraph p2 = h1.AddParagraph();
                            var run = p2.CreateRun();
                            p2.Alignment = ParagraphAlignment.CENTER;
                            string content = null;
                            foreach (var ite in ExamTable2[i].QuestionScore)
                            {
                                content = content + ite;
                                content = content + (char)12;
                            }
                            run.FontFamily = "宋体";
                            run.FontSize = 9;
                            run.SetText(content);
                            h1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

                        });
                        EditXwpfTableCell(r1, 2, "1200", h1 =>
                        {
                            foreach (var para in h1.Paragraphs)
                            {
                                var xp = para.CreateRun();
                                xp.FontFamily = "Wingdings 2";
                                xp.FontSize = 9;
                                para.FontAlignment = 2;
                                if (ExamTable2[i].IsPass == 1)
                                {
                                    xp.SetText(Convert.ToChar(82).ToString() + "是" + "   " + Convert.ToChar(163).ToString() + "否");
                                }
                                else if (ExamTable2[i].IsPass == 2)
                                {
                                    xp.SetText(Convert.ToChar(163).ToString() + "是" + "   " + Convert.ToChar(82).ToString() + "否");
                                }
                                else
                                {
                                    xp.SetText(Convert.ToChar(163).ToString() + "是" + "   " + Convert.ToChar(163).ToString() + "否");
                                }
                            }
                            h1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);
                        });
                    });
                }
                var dire = Path.GetDirectoryName(outFilePath);
                if (!Directory.Exists(dire))
                {
                    Directory.CreateDirectory(dire);
                }
                using (var outFile = new FileStream(outFilePath, FileMode.Create))
                {
                    doc.Write(outFile);
                    //outFile.Close();
                }
                doc.Close();
            }
        }
        /// <summary>
        /// docx文档图片，表格，特殊字符导出（高级）目前只适用达成度
        /// </summary>
        /// <param name="tempFilePath">docx文件路径</param>
        /// <param name="outFilePath">输出文件路径</param>
        /// <param name="data">字典数据源</param>
        public static void ExportChartWord(string tempFilePath, string outFilePath, Dictionary<string, string> data, List<CourObjCreateTableDto> tableList, List<CourObjCreateTableForMajor> tableMajor)
        {
            using (FileStream stream = File.OpenRead(tempFilePath))
            {
                XWPFDocument doc = new XWPFDocument(stream);
                List<string> imgPathList = new List<string>();
                XWPFParagraph paraList = null;
                string imgPath = null;
                XWPFParagraph xpara = null;
                XWPFTableCell xWPFTableCell = null;
                XWPFTableRow xWPFTableRow = null;
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
                                    if (para.ParagraphText.Contains("$" + key + "$"))
                                    {
                                        if (key.Contains("Table"))
                                        {
                                            xWPFTableCell = cell;
                                            xWPFTableRow = row;
                                        }
                                        if (key.Contains("221A"))
                                        {
                                            var xp = para.CreateRun();
                                            xp.SetFontFamily("仿宋", FontCharRange.Ascii);
                                            //xp.SetText(data[key]);
                                            para.ReplaceText("$" + key + "$", data[key]);
                                        }
                                        else if (key.Contains("SAnalEvaluaChart"))
                                        {
                                            imgPath = data[key];
                                            xpara = para;
                                        }else if(key.Contains("StudentAnalEvaluaChart")){
                                            imgPathList = data[key].Split(",").ToList();
                                            paraList = para;
                                        }
                                        else
                                            para.ReplaceText("$" + key + "$", data[key]);
                                        // para.ReplaceText(key, data[key]);
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
                //专业目标达成度
                if(tableMajor != null)
                {
                    var tableRow = tableMajor.Count();
                    var tableColumn = tableMajor[0].ClassAchive;
                    XWPFTable nestedXwpfTable = CreateNestedTableFromTableCell(xWPFTableCell, 5100, tableRow + 1, tableColumn.Count()+1);
                    xWPFTableCell.RemoveParagraph(0);
                    xWPFTableCell.RemoveParagraph(0);
                    nestedXwpfTable.SetColumnWidth(0, 1200);
                    EditXWPFTableRow(nestedXwpfTable, 0, 1200, r1 =>
                    {
                        EditXwpfTableCell(r1, 0, "1200", h1 =>
                        {

                            CreateCellText(h1,"课程目标");

                            h1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

                        });
                        for (int i = 1;i<= tableColumn.Count();i++)
                        {
                            EditXwpfTableCell(r1, i, "1200", h1 =>
                            {

                                CreateCellText(h1, tableColumn[i-1].ClassName + "课程目标达成值");

                                h1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

                            });
                        }
                    });
                    for(int j = 1; j <= tableRow; j++)
                    {
                        EditXWPFTableRow(nestedXwpfTable, j, 1200, r1 =>
                        {
                            EditXwpfTableCell(r1, 0, "1200", h1 =>
                            {

                                CreateCellText(h1, tableMajor[j-1].CourObjContent, ParagraphAlignment.LEFT);

                                h1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

                            });
                            for (int i = 0; i < tableColumn.Count(); i++)
                            {
                                EditXwpfTableCell(r1, i+1, "1200", h1 =>
                                {

                                    CreateCellText(h1, tableMajor[j-1].ClassAchive[i].AchiveScore);

                                    h1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

                                });
                            }
                        });
                    }
                }
                //班级达成度
                if (tableList != null)
                {
                    var tableRow = tableList.Select(c => c.Count).Sum();
                    //导入表格
                    XWPFTable nestedXwpfTable = CreateNestedTableFromTableCell(xWPFTableCell, 5100, tableRow + 1, 6);
                    xWPFTableCell.RemoveParagraph(0);
                    xWPFTableCell.RemoveParagraph(0);
                    xWPFTableCell.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.TOP);
                    //nestedXwpfTable.SetCellMargins(200, 0, 230, 0);
                    nestedXwpfTable.SetColumnWidth(0, 1200);
                    nestedXwpfTable.SetColumnWidth(1, 600);
                    nestedXwpfTable.SetColumnWidth(2, 300);
                    nestedXwpfTable.SetColumnWidth(3, 300);
                    nestedXwpfTable.SetColumnWidth(4, 300);
                    nestedXwpfTable.SetColumnWidth(5, 600);
                    EditXWPFTableRow(nestedXwpfTable, 0, 600, r1 =>
                    {
                        EditXwpfTableCell(r1, 0, "1200", h1 =>
                        {

                            CreateCellText(h1, "课程目标");

                            h1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

                        });
                        EditXwpfTableCell(r1, 1, "600", h1 =>
                        {

                            CreateCellText(h1, "考试/考核方式及占比", ParagraphAlignment.CENTER);

                            h1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

                        });

                        EditXwpfTableCell(r1, 2, "300", h1 =>
                        {
                            CreateCellText(h1, "考试/考核的目标分值", ParagraphAlignment.CENTER);

                            h1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

                        });

                        EditXwpfTableCell(r1, 3, "300", h1 =>
                        {
                            XWPFParagraph paragraph = CreateCellText(h1, "考试/考核的平均成绩", ParagraphAlignment.CENTER);
                        //paragraph.IndentationRight = 140;
                        //paragraph.IndentationLeft = 140;
                        h1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);


                        });


                        EditXwpfTableCell(r1, 4, "300", h1 =>
                        {

                            CreateCellText(h1, "考试/考核的达成值", ParagraphAlignment.CENTER);

                            h1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);


                        });


                        EditXwpfTableCell(r1, 5, "600", h1 =>
                        {
                            CreateCellText(h1, "课程目标达成值", ParagraphAlignment.CENTER);
                            h1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

                        });
                    });
                    int rowNum = 1;
                    foreach (var item in tableList)
                    {
                        var start = rowNum;
                        EditXWPFTableRow(nestedXwpfTable, rowNum, 600, r1 =>
                        {
                            EditXwpfTableCell(r1, 0, "600", h1 =>
                            {
                                CreateCellText(h1, item.CourObjContent, ParagraphAlignment.LEFT);
                                h1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

                            });
                        });
                        EditXWPFTableRow(nestedXwpfTable, rowNum, 600, r1 =>
                        {
                            EditXwpfTableCell(r1, 5, "600", h1 =>
                            {
                                CreateCellText(h1, item.AllAchiveScore, ParagraphAlignment.CENTER);
                                h1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

                            });
                        });
                        foreach (var ite in item.TableSplit)
                        {
                            EditXWPFTableRow(nestedXwpfTable, rowNum, 600, r1 =>
                            {
                                EditXwpfTableCell(r1, 1, "600", h1 =>
                                {
                                    CreateCellText(h1, ite.Rate, ParagraphAlignment.LEFT);
                                    h1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

                                });
                                EditXwpfTableCell(r1, 2, "600", h1 =>
                                {
                                    CreateCellText(h1, ite.ObjScore, ParagraphAlignment.LEFT);
                                    h1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

                                });
                                EditXwpfTableCell(r1, 3, "600", h1 =>
                                {
                                    CreateCellText(h1, ite.AvgScore, ParagraphAlignment.LEFT);
                                    h1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

                                });
                                EditXwpfTableCell(r1, 4, "600", h1 =>
                                {
                                    CreateCellText(h1, ite.AchiveScore, ParagraphAlignment.LEFT);
                                    h1.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.CENTER);

                                });
                            });
                            rowNum++;
                        }
                        MYMergeCells(nestedXwpfTable, 0, 0, start, rowNum - 1);
                        MYMergeCells(nestedXwpfTable, 5, 5, start, rowNum - 1);
                    }
                }
                //插入图片
                using (var img = new FileStream(imgPath, FileMode.Open, FileAccess.Read))
                {
                    var cW = 6000;
                    var cH = 3000;
                    XWPFParagraph p = xpara;
                    XWPFRun run = p.CreateRun();
                    var widthPic = (int)((double)cW / 587 * 38.4 * 9525);
                    var heightPic = (int)((double)cH / 587 * 38.4 * 9525);
                    run.AddPicture(img, (int)PictureType.JPEG, "11.png", widthPic, heightPic);
                }
                int n = 1;
                foreach(var item in imgPathList)
                {
                    var cW = 10000;
                    var cH = 3000;
                    using (var img = new FileStream(item, FileMode.Open, FileAccess.Read))
                    {
                        XWPFParagraph p = paraList;
                        XWPFRun run = p.CreateRun();
                        var widthPic = (int)((double)cW / 587 * 38.4 * 9525);
                        var heightPic = (int)((double)cH / 587 * 38.4 * 9525);
                        run.AddPicture(img, (int)PictureType.JPEG, "11.png", widthPic, heightPic);
                        run.FontSize = 9;
                        run.AddCarriageReturn();
                        run.SetFontFamily("仿宋", FontCharRange.None);
                        run.SetText("图" + (n+1) + "学生个体对课程目标" + n + "达成情况分析",0);
                    }
                    n++;
                }
                var dire = Path.GetDirectoryName(outFilePath);
                if (!Directory.Exists(dire))
                {
                    Directory.CreateDirectory(dire);
                }
                using (var outFile = new FileStream(outFilePath, FileMode.Create))
                {
                    doc.Write(outFile);
                    //outFile.Close();
                }
                doc.Close();
            }
        }
        /// <summary>
        /// 给格子加文字（宋体）
        /// </summary>
        /// <param name="h"></param>
        /// <param name="txt"></param>
        /// <param name="alignment"></param>
        /// <param name="FontSize"></param>
        /// <param name="FontFamily"></param>
        /// <returns></returns>
        public static XWPFParagraph CreateCellText1(XWPFTableCell h, string txt, ParagraphAlignment alignment = ParagraphAlignment.CENTER,
            int FontSize = 9, string FontFamily = "宋体")
        {
            h.RemoveParagraph(0);
            XWPFParagraph p2 = h.AddParagraph();
            //p2.IsWordWrap = IsWordWrap;
            p2.Alignment = alignment;
            XWPFRun r2 = p2.CreateRun();
            r2.IsBold = false;
            r2.SetText(txt);
            r2.FontFamily = FontFamily;//设置雅黑字体
            r2.FontSize = FontSize;
            return p2;
        }
        /// <summary>
        /// 给格子加文字
        /// </summary>
        /// <param name="h"></param>
        /// <param name="txt"></param>
        /// <param name="alignment"></param>
        /// <param name="FontSize"></param>
        /// <param name="FontFamily"></param>
        /// <returns></returns>
        public static XWPFParagraph CreateCellText(XWPFTableCell h, string txt, ParagraphAlignment alignment = ParagraphAlignment.CENTER,
            int FontSize = 9, string FontFamily = "仿宋")
        {
            h.RemoveParagraph(0);
            XWPFParagraph p2 = h.AddParagraph();
            //p2.IsWordWrap = IsWordWrap;
            p2.Alignment = alignment;
            XWPFRun r2 = p2.CreateRun();
            r2.IsBold = false;
            r2.SetText(txt);
            r2.FontFamily = FontFamily;//设置雅黑字体
            r2.FontSize = FontSize;
            return p2;
        }
        /// <summary>
        /// 编辑行
        /// </summary>
        /// <param name="table"></param>
        /// <param name="i"></param>
        /// <param name="Height"></param>
        /// <param name="RowAction"></param>
        public static void EditXWPFTableRow(XWPFTable table, int i, ulong Height, Action<XWPFTableRow> RowAction)
        {

            XWPFTableRow xwpfTableRow = table.GetRow(i);

            xwpfTableRow.GetCTRow().AddNewTrPr().AddNewTrHeight().val = Height;

            RowAction(xwpfTableRow);
        }
        /// <summary>
        /// 编辑单元格
        /// </summary>
        /// <param name="xwpfTableRow"></param>
        /// <param name="i"></param>
        /// <param name="width"></param>
        /// <param name="funcAction"></param>
        /// <returns></returns>
        public static XWPFTableCell EditXwpfTableCell(XWPFTableRow xwpfTableRow, int i, string width, Action<XWPFTableCell> funcAction)
        {

            XWPFTableCell tableCell = xwpfTableRow.GetCell(i);

            CT_TcPr m_Pr = tableCell.GetCTTc().AddNewTcPr();
            m_Pr.tcW = new CT_TblWidth();
            m_Pr.tcW.w = width.ToString();//单元格宽
            m_Pr.tcW.type = ST_TblWidth.dxa;
            funcAction(tableCell);
            return tableCell;
        }
        /// <summary>
        /// 创建单元格
        /// </summary>
        /// <param name="xwpfTableRow"></param>
        /// <param name="width"></param>
        /// <param name="funcAction"></param>
        /// <returns></returns>
        public static XWPFTableCell CreateXwpfTableCell(XWPFTableRow xwpfTableRow, int width, Action<XWPFTableCell> funcAction)
        {

            XWPFTableCell tableCell = xwpfTableRow.CreateCell();
            CT_TcPr m_Pr = tableCell.GetCTTc().AddNewTcPr();

            m_Pr.tcW = new CT_TblWidth();

            m_Pr.tcW.w = width.ToString();//单元格宽

            m_Pr.tcW.type = ST_TblWidth.pct;
            funcAction(tableCell);
            return tableCell;
        }

        /// <summary>
        /// 创建word表格
        /// </summary>
        /// <param name="h"></param>
        /// <param name="width"></param>
        /// <param name="rs"></param>
        /// <param name="cs"></param>
        /// <returns></returns>
        public static XWPFTable CreateNestedTableFromTableCell(XWPFTableCell h, int width, int rs, int cs)
        {

            CT_Tbl ctx = new CT_Tbl();

            h.GetCTTc().Items.Add(ctx);
            h.GetCTTc().ItemsElementName.Add(ItemsChoiceTableCellType.tbl);

            h.GetCTTc().GetTblList().Add(ctx);
            h.GetCTTc().AddNewP();
            XWPFTable NestedTable = new XWPFTable(ctx, h, rs, cs);

            h.InsertTable(0, NestedTable);
            ctx.AddNewTblPr().AddNewTblW().w = width.ToString();//表宽度

            ctx.AddNewTblPr().AddNewTblW().type = ST_TblWidth.pct;

            ctx.AddNewTblPr().jc = new CT_Jc();
            ctx.AddNewTblPr().jc.val = ST_Jc.center;
            return NestedTable;
        }

        public static void MYMergeCells(XWPFTable table, int fromCol, int toCol, int fromRow, int toRow)
        {
            for (int rowIndex = fromRow; rowIndex <= toRow; rowIndex++)
            {
                if (fromCol < toCol)
                {
                    table.GetRow(rowIndex).MergeCells(fromCol, toCol);
                }
                XWPFTableCell rowcell = table.GetRow(rowIndex).GetCell(fromCol);
                CT_Tc cttc = rowcell.GetCTTc();
                if (cttc.tcPr == null)
                {
                    cttc.AddNewTcPr();
                }
                if (rowIndex == fromRow)
                {
                    // The first merged cell is set with RESTART merge value  
                    rowcell.GetCTTc().tcPr.AddNewVMerge().val = ST_Merge.restart;
                }
                else
                {
                    // Cells which join (merge) the first one, are set with CONTINUE  
                    rowcell.GetCTTc().tcPr.AddNewVMerge().val = ST_Merge.@continue;
                }
            }
        }
            public static string CreateSummaryWord(Dictionary<string, string> data, IHostingEnvironment env)
        {
            var outputFileName = string.Empty;
            using (FileStream stream = File.OpenRead(env.WebRootPath + "/Files/周报模版.docx"))
            {
                XWPFDocument doc = new XWPFDocument(stream);

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
                                        //text = text.Replace(key, data[key]);
                                        if (key == "$TaskInfos$" || key == "$ExecuteInfos$")
                                        {
                                            para.ReplaceText(key, "");
                                            var run = para.CreateRun();
                                            run.SetText("");
                                            run.SetFontFamily("宋体", FontCharRange.CS);
                                            run.FontSize = 12;
                                            run.AddCarriageReturn();
                                            var infoString = data[key].Split('；');
                                            var i = 0;
                                            foreach (var text in infoString)
                                            {
                                                run.AppendText(text + (i < infoString.Length - 1 ? "；" : ""));
                                                run.AddCarriageReturn();
                                                i++;
                                            }
                                        }
                                        else if (key.Equals("$TaskImages$") || key.Equals("$ExecuteImages$"))
                                        {
                                            para.ReplaceText(key, "");
                                            var run = para.CreateRun();
                                            run.SetText("");
                                            run.SetFontFamily("宋体", FontCharRange.CS);
                                            run.FontSize = 12;
                                            run.AddCarriageReturn();
                                            var imageInfo = data[(key.Equals("$TaskImages$") ? "$TaskInfos$" : "$ExecuteInfos$")].Split("；");
                                            var imageString = data[key].Split(";").ToList();
                                            imageString.Remove("");
                                            var i = 0;
                                            foreach (var text in imageString)
                                            {
                                                if (text.Equals("无图片"))
                                                {
                                                    run.AppendText(text);
                                                }
                                                else
                                                {
                                                    var filePath = env.WebRootPath + "/" + text;
                                                    var imageFile = File.Open(filePath, FileMode.Open, FileAccess.Read);
                                                    run.AddPicture(
                                                        imageFile,
                                                        (int)PictureType.PNG,
                                                        "111.jpg",
                                                        (int)(400.0 * 9525),
                                                        (int)(300.0 * 9525));
                                                    imageFile.Close();
                                                }
                                                run.AddCarriageReturn();
                                                run.AppendText(imageInfo[i] + (i < imageInfo.Length - 1 ? "；" : ""));
                                                run.AddCarriageReturn();
                                                run.AddCarriageReturn();
                                                i++;
                                            }
                                        }
                                        else if (key.Equals("$ExecuteImages$"))
                                        {

                                        }
                                        else
                                            para.ReplaceText(key, data[key]);
                                    }
                                }
                            }
                        }
                    }
                }


                outputFileName = LocalTool.GetTimeStamp(DateTime.Now) + ".docx";
                var outFile = new FileStream(env.WebRootPath + "/Files/" + outputFileName, FileMode.Create);
                doc.Write(outFile);
                doc.Close();
            }

            return outputFileName;
        }


        #region 工具方法
        private static void ReplaceKey(XWPFParagraph para, Dictionary<string, string> data)
        {
            string text = "";
            foreach (var key in data.Keys)
            {
                //$$模板中数据占位符为$KEY$
                if (para.ParagraphText.Contains(key))
                {
                    para.ReplaceText("$" + key + "$", data[key]);
                    //text = text.Replace(key, data[key]);
                }
            }
        }
        private static void ReplaceWordKey(XWPFParagraph para, Dictionary<string, string> data)
        {
            string text = "";
            foreach (var run in para.Runs)
            {
                text = run.ToString();
                foreach (var key in data.Keys)
                {
                    //$$模板中数据占位符为$KEY$
                    if (text.Contains(key))
                    {
                        text = text.Replace("$" + key + "$", data[key]);
                    }
                    //if (text.StartsWith("$") && text.EndsWith("$"))
                    //{
                    //    text = text.Replace(key, data[key]);
                    //}
                }
                run.SetText(text, 0);
            }
        }
        /// <summary>
        /// 给图片加水印
        /// </summary>
        /// <param name="imgPath"></param>
        /// <param name="sImgPath"></param>
        /// <param name="userName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AddWaterMark(string imgPath, string sImgPath, string userName, string key)
        {
            //水印文字
            string text = key + "---" + userName + "---" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            // Add watermark
            var watermarkedStream = new MemoryStream();
            using (var img = Image.FromFile(imgPath))
            {
                using (var graphic = Graphics.FromImage(img))
                {
                    int size = 0;
                    int height = 0;
                    if (img.Width > 1500)
                    {
                        size = 70;
                        height = Convert.ToInt32(Convert.ToDouble(img.Height) - img.Height * 0.1);
                    }
                    if (img.Width > 1000 && img.Width < 1500)
                    {
                        size = 50;
                        height = Convert.ToInt32(Convert.ToDouble(img.Height) - img.Height * 0.1);
                    }
                    else
                    {
                        size = 20;
                        height = Convert.ToInt32(Convert.ToDouble(img.Height) - img.Height * 0.05);

                    }
                    var font = new Font(System.Drawing.FontFamily.GenericSansSerif, size, FontStyle.Bold, GraphicsUnit.Pixel);
                    var color = Color.FromArgb(128, 255, 255, 255);
                    var brush = new SolidBrush(color);
                    //var point = new Point(img.Width - 1300, img.Height - 80);
                    var point = new Point(Convert.ToInt32(Convert.ToDouble(img.Width) - img.Width * 0.9), height);

                    graphic.DrawString(text, font, brush, point);
                    img.Save(watermarkedStream, ImageFormat.Png);
                }
                img.Save(sImgPath);
            }
            //using (Image image = Image.FromFile(imgPath))
            {
                //    try
                //    {
                //        Bitmap bitmap = new Bitmap(image);

                //        int width = bitmap.Width, height = bitmap.Height;
                //        //水印文字
                //        string text = key+"---"+userName+"---"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                //        Graphics g = Graphics.FromImage(bitmap);

                //        g.DrawImage(bitmap, 0, 0);
                //        //获取或设置与此 Graphics 关联的插补模式
                //        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                //        //获取或设置此 Graphics 的呈现质量
                //        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //        //在指定的位置使用原始物理大小绘制指定的 Image。
                //        g.DrawImage(image, new Rectangle(0, 0, width, height), 0, 0, width, height, GraphicsUnit.Pixel);

                //        Font crFont = new Font("微软雅黑", 30, FontStyle.Bold);
                //        SizeF crSize = new SizeF();
                //        crSize = g.MeasureString(text, crFont);

                //        //背景位置(去掉了. 如果想用可以自己调一调 位置.)
                //        //g.FillRectangle(new SolidBrush(Color.FromArgb(200, 255, 255, 255)), (width - crSize.Width) / 2, (height - crSize.Height) / 2, crSize.Width, crSize.Height);

                //        SolidBrush semiTransBrush = new SolidBrush(Color.Red);

                //        //将原点移动 到图片中点
                //        g.TranslateTransform(0, height-15);
                //        //以原点为中心 转 -45度
                //        //g.RotateTransform(-45);
                //        //在指定位置并且用指定的 Brush 和 Font 对象绘制指定的文本字符串
                //        g.DrawString(text, crFont, semiTransBrush, new PointF(0, 0));

                //        //保存文件
                //        bitmap.Save(sImgPath, System.Drawing.Imaging.ImageFormat.Jpeg);

                //    }
                //    catch (Exception e)
                //    {
                //        return e.Message;
                //    }
            }

            return sImgPath;
        }
        #endregion
        #region 示例
        public static string Test1()
        {
            //List<string> remark = new List<string>();
            //Dictionary<string, string> data = new Dictionary<string, string>();
            //data.Add("ProjectName", "测试项目");
            //string pathid = Guid.NewGuid().ToString("N");//随机生成文件名
            //string Outputpath = _env.WebRootPath + @"/" + task.ProjectName + "/PdfOutput/" + pathid + ".docx"; //替换后的word文档路径
            //Export(@"G:\测试前.docx", @"G:\测试后.docx", data);//替换word中的字段生成文件Outputpath
            OfficeConvert.ToPdf(@"D:\测试后.doc", @"D:\测试\"); //转pdf
            //OfficeConvert.WordToPdf(@"G:\测试前.docx", @"G:\测试\测试后.pdf"); //转pdf
            // string OutputPdf = _env.WebRootPath + @" / image/PdfOutput/Output/pathid"+".pdf"; //得到pdf文件路径
            // string OutputPdf = LocalTool.GetAppSettings("Path") + @"/" + task.ProjectName + "/PdfOutput/Output/" + pathid + ".pdf";
            return null;
        }
        /// <summary>
        /// 输出模板docx文档(使用字典)
        /// </summary>
        /// <param name="tempFilePath">docx文件路径</param>
        /// <param name="outPath">输出文件路径</param>
        /// <param name="userName"></param>
        /// <param name="data">字典数据源</param>
        /// <param name="imageList"></param>
        /// <param name="videoList"></param>
        public static void ExportWord(string tempFilePath, string outPath, string userName, Dictionary<string, string> data, Dictionary<string, string> imageList, Dictionary<string, string> videoList, string rootPath)
        {

            using (FileStream stream = File.OpenRead(tempFilePath))
            {
                XWPFDocument doc = new XWPFDocument(stream);
                //遍历段落                  
                foreach (var para in doc.Paragraphs)
                {
                    ReplaceKey(para, data);
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
                                        para.ReplaceText(key, data[key]);
                                        //text = text.Replace(key, data[key]);
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (var key in imageList.Keys)
                {
                    var localhostPath = imageList[key].Replace(LocalTool.GetAppSettings("Path"), rootPath);
                    //给word中导入数据
                    //创建新的一行
                    XWPFRun r2 = doc.CreateParagraph().CreateRun();
                    var newPath = Path.GetDirectoryName(localhostPath) + "\\" + System.IO.Path.GetFileNameWithoutExtension(localhostPath) + DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(localhostPath);
                    //给上传的图片加上上传者时间备注作为水印
                    var newimage = AddWaterMark(localhostPath, newPath, userName, key);
                    var img = Image.FromFile(newimage);

                    var widthEmus = (int)(400.0 * 9525);
                    var heightEmus = (int)(300.0 * 9525);

                    using (FileStream picData = new FileStream(newimage, FileMode.Open, FileAccess.Read))
                    {
                        r2.AddPicture(picData, (int)NPOI.XWPF.UserModel.PictureType.PNG, System.IO.Path.GetFileName(localhostPath), widthEmus, heightEmus);
                    }
                }
                foreach (var key in videoList.Keys)
                {
                    //给word中导入数据
                    //创建新的一行
                    XWPFRun r2 = doc.CreateParagraph().CreateRun();
                    r2.SetText(key + ":=========" + videoList[key]);
                }
                using (var outFile = new FileStream(outPath, FileMode.Create))
                {
                    doc.Write(outFile);
                    //outFile.Close();
                }
                doc.Close();
                //写文件
                //FileStream outFile = new FileStream(outPath, FileMode.Create);

            }
        }

        public static void MergeWord(string MergeFile, List<string> Files)
        {
            using (FileStream stream = File.OpenRead(Files.First()))
            {
                XWPFDocument doc = new XWPFDocument(stream);
                using (FileStream stream1 = File.OpenRead(Files.Last()))
                {
                    XWPFDocument doc2 = new XWPFDocument(stream1);
                    int i = 1;
                    foreach (var para in doc2.Paragraphs)
                    {
                        XWPFDocument document = doc.CreateParagraph().Document;
                        document.SetParagraph(para, i);
                        i++;
                    }
                    foreach (var para in doc2.Tables)
                    {
                        XWPFDocument document = doc.CreateParagraph().Document;
                        document.SetTable(i, para);
                        i++;
                    }
                    doc2.Close();
                }
                using (var outFile = new FileStream(MergeFile, FileMode.Create))
                {
                    doc.Write(outFile);
                    //outFile.Close();
                }
                doc.Close();
            }
        }
        #endregion

        /// <summary>
        /// 替换Word 模板内容
        /// </summary>
        /// <param name="templatePath">模板路径</param>
        /// <param name="savePath">文件保存路径,全路径</param>
        /// <param name="dicKeyword">字典</param>
        public static void ReplaceKeyword(string templatePath, string savePath, Dictionary<string, string> dicKeyword)
        {
            using (FileStream stream = File.OpenRead(templatePath))
            {
                FileStream fs = new FileStream(templatePath, FileMode.Open, FileAccess.Read);
                XWPFDocument doc = new XWPFDocument(fs);

                //遍历段落                  
                foreach (var para in doc.Paragraphs)
                {
                    string oldText = para.ParagraphText;
                    if (!string.IsNullOrEmpty(oldText))
                    {
                        string tempText = para.ParagraphText;

                        foreach (KeyValuePair<string, string> kvp in dicKeyword)
                        {
                            if (tempText.Contains(kvp.Key))
                            {
                                tempText = tempText.Replace(kvp.Key, kvp.Value);

                                para.ReplaceText(oldText, tempText);
                            }
                        }

                    }
                }

                //遍历表格      
                var tables = doc.Tables;
                foreach (var table in tables)
                {
                    foreach (var row in table.Rows)
                    {
                        foreach (var cell in row.GetTableCells())
                        {
                            foreach (var para in cell.Paragraphs)
                            {
                                string oldText = para.ParagraphText;
                                if (!string.IsNullOrEmpty(oldText))
                                {
                                    //记录段落文本
                                    string tempText = para.ParagraphText;
                                    foreach (KeyValuePair<string, string> kvp in dicKeyword)
                                    {
                                        if (tempText.Contains(kvp.Key))
                                        {
                                            tempText = tempText.Replace(kvp.Key, kvp.Value);

                                            //替换内容
                                            para.ReplaceText(oldText, tempText);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //生成指定文件
                FileStream output = new FileStream(savePath, FileMode.Create);
                //将文档信息写入文件
                doc.Write(output);

                //一些列关闭释放操作
                fs.Close();
                fs.Dispose();
                output.Close();
                output.Dispose();
            }
        }
    }
}
