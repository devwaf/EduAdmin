using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.LocalTools
{
    public class ExcelHelper
    {
        /// <summary>
        /// 导出表头
        /// </summary>
        /// <param name="list"></param>
        /// <param name="savePath"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string ExportExcel(List<string> list, string savePath, string filename)
        {
            //List<Dictionary<string, string>> data = list.ToListStringDictionary();
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("sheet");
            ICell cell = null;
            #region 样式部分
            //自定义颜色
            //HSSFPalette palette = workbook.GetCustomPalette(); //调色板实例
            //palette.SetColorAtIndex(序号, R, G, B);
            //palette.SetColorAtIndex(56, 231, 227, 231);
            //palette.SetColorAtIndex(57, 255, 193, 180);

            //样式一  普通样式
            ICellStyle style1 = workbook.CreateCellStyle();//创建样式对象
            IFont font1 = workbook.CreateFont(); //创建一个字体样式对象
            font1.FontName = "宋体"; //和excel里面的字体对应
            //font1.Color = new HSSFColor.Black().Indexed;//颜色
            //font.IsItalic = true; //斜体
            font1.FontHeightInPoints = 11;//字体大小
            //font.Boldweight = short.MaxValue;//字体加粗
            style1.Alignment = HorizontalAlignment.Center;//水平居中
            style1.VerticalAlignment = VerticalAlignment.Center;//垂直居中
            style1.BorderBottom = BorderStyle.Thin;
            style1.BorderLeft = BorderStyle.Thin;
            style1.BorderRight = BorderStyle.Thin;
            style1.BorderTop = BorderStyle.Thin;
            //style1.FillForegroundColor = palette.FindColor(231, 227, 231).Indexed;
            //style1.FillPattern = FillPattern.SolidForeground;
            style1.SetFont(font1); //将字体样式赋给样式对象
            //样式二 标题样式
            ICellStyle style2 = workbook.CreateCellStyle();//创建样式对象
            IFont font2 = workbook.CreateFont(); //创建一个字体样式对象
            font2.FontName = "宋体"; //和excel里面的字体对应
            font2.IsBold = true;//字体加粗
            font2.FontHeightInPoints = 14;//字体大小
            style2.WrapText = true;
            style2.Alignment = HorizontalAlignment.Center;//水平居中
            style2.VerticalAlignment = VerticalAlignment.Center;//垂直居中
            style2.SetFont(font2); //将字体样式赋给样式对象

            //样式三 小标题 加粗
            ICellStyle style3 = workbook.CreateCellStyle();//创建样式对象
            IFont font3 = workbook.CreateFont(); //创建一个字体样式对象
            font3.FontName = "宋体"; //和excel里面的字体对应
            font3.IsBold = true;//字体加粗
            font3.FontHeightInPoints = 11;//字体大小
            style3.WrapText = true;
            style3.Alignment = HorizontalAlignment.Center;//水平居中
            style3.VerticalAlignment = VerticalAlignment.Center;//垂直居中
            style3.BorderBottom = BorderStyle.Thin;
            style3.BorderLeft = BorderStyle.Thin;
            style3.BorderRight = BorderStyle.Thin;
            style3.BorderTop = BorderStyle.Thin;
            style3.SetFont(font3); //将字体样式赋给样式对象
            #endregion
            IRow rows;
            rows = sheet.CreateRow(0);
            rows.Height = 520;
            //小标题
            int i = 0;
            foreach (var item in list)
            {
                SetCellValue1(rows, cell, i, style3, item);
                i++;
            }
            //设置自适应宽度
            for (int columnNum = 0; columnNum < list.Count; columnNum++)
            {
                int columnWidth = sheet.GetColumnWidth(columnNum) / 256;
                for (int rowNum = 0; rowNum <= sheet.LastRowNum; rowNum++)
                {
                    IRow currentRow = sheet.GetRow(rowNum);
                    if (currentRow != null && currentRow.GetCell(columnNum) != null)
                    {
                        ICell currentCell = currentRow.GetCell(columnNum);
                        int length = Encoding.UTF8.GetBytes(currentCell.ToString()).Length;
                        if (columnWidth < length + 1)
                        {
                            columnWidth = length + 1;
                        }
                    }
                }
                if (columnWidth > 255)
                    columnWidth = 255;
                sheet.SetColumnWidth(columnNum, columnWidth * 256);
            }
            sheet.SetColumnWidth(0, 10 * 256);
            sheet.SetColumnWidth(1, 15 * 256);
            sheet.SetColumnWidth(2, 10 * 256);
            sheet.SetColumnWidth(3, 10 * 256);
            var filepath = Path.Combine(savePath, filename + ".xls");
            //创建目录
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            //删除现有文件
            if (!File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            using (FileStream file = new FileStream(filepath, FileMode.Create))
            {
                workbook.Write(file);
                file.Close();
            }
            return filepath;
        }
        private static void SetCellValue(IRow rows, ICell cell, int rowNum, ICellStyle style, dynamic value)
        {
            value =  LocalTool.ParseDouble(value);
            cell = rows.CreateCell(rowNum);
            cell.CellStyle = style;
            if (value != null)
                cell.SetCellValue(value);
        }
        private static void SetCellValue1(IRow rows, ICell cell, int rowNum, ICellStyle style, dynamic value)
        {
            cell = rows.CreateCell(rowNum);
            cell.CellStyle = style;
            if (value != null)
                cell.SetCellValue(value);
        }
        /// <summary>
        /// 生成 Excel 
        /// </summary>
        /// <param name="data">数据(该数组至少要有一项)</param>
        /// <param name="savePath">存储路径</param>
        /// <param name="filename">文件名（不带后缀）</param>
        /// <returns></returns>
        public static string CreateExcel(List<Dictionary<string, string>> data, Dictionary<string, string> list, string savePath, string filename)
        {
            var tableName = data[0];

            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("sheet");
            ICell cell = null;
            #region 样式部分
            //自定义颜色
            //HSSFPalette palette = workbook.GetCustomPalette(); //调色板实例
            //palette.SetColorAtIndex(序号, R, G, B);
            //palette.SetColorAtIndex(56, 231, 227, 231);
            //palette.SetColorAtIndex(57, 255, 193, 180);

            //样式一  普通样式
            IDataFormat dataformat = workbook.CreateDataFormat();
            ICellStyle style1 = workbook.CreateCellStyle();//创建样式对象
            IFont font1 = workbook.CreateFont(); //创建一个字体样式对象
            font1.FontName = "宋体"; //和excel里面的字体对应
            //font1.Color = new HSSFColor.Black().Indexed;//颜色
            //font.IsItalic = true; //斜体
            font1.FontHeightInPoints = 11;//字体大小
            //font.Boldweight = short.MaxValue;//字体加粗
            style1.Alignment = HorizontalAlignment.Center;//水平居中
            style1.VerticalAlignment = VerticalAlignment.Center;//垂直居中
            //style1.FillForegroundColor = palette.FindColor(231, 227, 231).Indexed;
            //style1.FillPattern = FillPattern.SolidForeground;
            //style1.DataFormat = dataformat.GetFormat("0.00");
            style1.SetFont(font1); //将字体样式赋给样式对象
            #endregion
            IRow rows = sheet.CreateRow(0);
            int i = 0;
            foreach (var item in list)
            {
                SetCellValue(rows, cell, i, style1, item.Key);
                i++;
            }
            i = 1;
            foreach (var item in data)
            {
                rows = sheet.CreateRow(i);
                int j = 0;
                foreach (var key in tableName)
                {
                    SetCellValue(rows, cell, j, style1, item.GetValueOrDefault(key.Key));
                    j++;
                }
                i++;
            }
            //设置自适应宽度
            for (int columnNum = 0; columnNum < tableName.Count; columnNum++)
            {
                int columnWidth = sheet.GetColumnWidth(columnNum) / 256;
                for (int rowNum = 0; rowNum <= sheet.LastRowNum; rowNum++)
                {
                    IRow currentRow = sheet.GetRow(rowNum);
                    if (currentRow != null && currentRow.GetCell(columnNum) != null)
                    {
                        ICell currentCell = currentRow.GetCell(columnNum);
                        int length = Encoding.UTF8.GetBytes(currentCell.ToString()).Length;
                        if (columnWidth < length + 1)
                        {
                            columnWidth = length + 1;
                        }
                    }
                }
                if (columnWidth > 255)
                    columnWidth = 255;
                sheet.SetColumnWidth(columnNum, columnWidth * 256);
                sheet.SetColumnWidth(1, 20 * 256);
            }
            var filepath = Path.Combine(savePath, filename + ".xls");
            //创建目录
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            //删除现有文件
            if (!File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            using (FileStream file = new FileStream(filepath, FileMode.Create))
            {
                workbook.Write(file);
                file.Close();
            }
            return filepath;
        }

        /// <summary>
        /// 生成 Excel 
        /// </summary>
        /// <param name="data">数据(该数组至少要有一项)</param>
        /// <param name="savePath">存储路径</param>
        /// <param name="filename">文件名（不带后缀）</param>
        /// <returns></returns>
        public static string CreateExcelToint(List<Dictionary<string, string>> data, Dictionary<string, string> list, string savePath, string filename)
        {
            var tableName = data[0];

            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("sheet");
            ICell cell = null;
            #region 样式部分
            IDataFormat dataformat = workbook.CreateDataFormat();
            //自定义颜色
            //HSSFPalette palette = workbook.GetCustomPalette(); //调色板实例
            //palette.SetColorAtIndex(序号, R, G, B);
            //palette.SetColorAtIndex(56, 231, 227, 231);
            //palette.SetColorAtIndex(57, 255, 193, 180);

            //样式一  普通样式
            ICellStyle style1 = workbook.CreateCellStyle();//创建样式对象
            IFont font1 = workbook.CreateFont(); //创建一个字体样式对象
            font1.FontName = "宋体"; //和excel里面的字体对应
            //font1.Color = new HSSFColor.Black().Indexed;//颜色
            //font.IsItalic = true; //斜体
            font1.FontHeightInPoints = 11;//字体大小
            //font.Boldweight = short.MaxValue;//字体加粗
            style1.Alignment = HorizontalAlignment.Center;//水平居中
            style1.VerticalAlignment = VerticalAlignment.Center;//垂直居中
            //style1.FillForegroundColor = palette.FindColor(231, 227, 231).Indexed;
            //style1.FillPattern = FillPattern.SolidForeground;
            style1.SetFont(font1); //将字体样式赋给样式对象
            //样式二  普通样式
            ICellStyle style2 = workbook.CreateCellStyle();//创建样式对象
            IFont font2 = workbook.CreateFont(); //创建一个字体样式对象
            font2.FontName = "宋体"; //和excel里面的字体对应
            //font1.Color = new HSSFColor.Black().Indexed;//颜色
            //font.IsItalic = true; //斜体
            font2.FontHeightInPoints = 11;//字体大小
            style2.DataFormat = dataformat.GetFormat("0.00");
            //font.Boldweight = short.MaxValue;//字体加粗
            style2.Alignment = HorizontalAlignment.Center;//水平居中
            style2.VerticalAlignment = VerticalAlignment.Center;//垂直居中
            //style1.FillForegroundColor = palette.FindColor(231, 227, 231).Indexed;
            //style1.FillPattern = FillPattern.SolidForeground;
            style2.SetFont(font1); //将字体样式赋给样式对象
            #endregion
            IRow rows = sheet.CreateRow(0);
            int i = 0;
            foreach (var item in list)
            {
                SetCellValue(rows, cell, i, style1, item.Key);
                i++;
            }
            i = 1;
            foreach (var item in data)
            {
                rows = sheet.CreateRow(i);
                int j = 0;
                foreach (var key in tableName)
                {
                    SetCellValue(rows, cell, j, style2, item.GetValueOrDefault(key.Key));
                    j++;
                }
                i++;
            }
            //设置自适应宽度
            for (int columnNum = 0; columnNum < tableName.Count; columnNum++)
            {
                int columnWidth = sheet.GetColumnWidth(columnNum) / 256;
                for (int rowNum = 0; rowNum <= sheet.LastRowNum; rowNum++)
                {
                    IRow currentRow = sheet.GetRow(rowNum);
                    if (currentRow != null && currentRow.GetCell(columnNum) != null)
                    {
                        ICell currentCell = currentRow.GetCell(columnNum);
                        int length = Encoding.UTF8.GetBytes(currentCell.ToString()).Length;
                        if (columnWidth < length + 1)
                        {
                            columnWidth = length + 1;
                        }
                    }
                }
                if (columnWidth > 255)
                    columnWidth = 255;
                sheet.SetColumnWidth(columnNum, columnWidth * 256);
            }
            var filepath = Path.Combine(savePath, filename + ".xls");
            //创建目录
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            //删除现有文件
            if (!File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            using (FileStream file = new FileStream(filepath, FileMode.Create))
            {
                workbook.Write(file);
                file.Close();
            }
            return filepath;
        }

    }
}
