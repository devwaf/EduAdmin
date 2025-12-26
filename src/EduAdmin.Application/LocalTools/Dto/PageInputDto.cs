using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EduAdmin.LocalTools.Dto
{
    /// <summary>
    /// 分页入参
    /// </summary>
    public class PageInputDto
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 第几页
        /// </summary>
        public int PageNum { get; set; }
        /// <summary>
        /// 页面总数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 分割数组
        /// </summary>
        /// <param name="list"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<T> CutPage<T>(List<T> list, int pageNum, int pageSize)
        {
            //设一个默认值
            int size = 5;
            List<T> res;
            //第0页返回所有内容
            if (list == null || list.Count == 0 || pageNum == 0)
            {
                res = list;
            }
            else
            {
                if (pageSize == 0)
                {
                    pageSize = size;
                }
                res = list.Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();
            }
            return res;
        }
        /// <summary>
        /// 分割数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public PageOutput<T> CutPage<T>(List<T> list)
        {
            return new PageOutput<T>(list, PageNum, PageSize);
        }
    }
    /// <summary>
    /// 分页输出
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageOutput<T>
    {
        /// <summary>
        /// 数组
        /// </summary>
        public List<T> List { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int Count { get; set; }
        public PageOutput()
        {
            List = new List<T>();
        }
        public PageOutput(List<T> list, int count)
        {
            //List = list ?? throw new ArgumentNullException(nameof(list));
            List = list ?? new List<T>();
            Count = count;
        }
        public PageOutput(List<T> list, int pageNum, int pageSize)
        {
            Count = list.Count;
            List = PageInputDto.CutPage(list, pageNum, pageSize);
        }
    }
    public class ProjectPageInputDto : PageInputDto
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid ProjectId { get; set; }
    }
    public class CompanyPageInputDto : PageInputDto
    {
        /// <summary>
        /// 公司Id
        /// </summary>
        public Guid CompanyId { get; set; }
    }

}
