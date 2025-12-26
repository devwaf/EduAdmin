using System;
using System.Collections.Generic;
using System.Text;

namespace EduAdmin.LocalTools.Dto
{
    /// <summary>
    /// 抽象结果对象
    /// </summary>
    public abstract class IResultDto
    {
        /// <summary>
        /// 结果
        /// </summary>
        public abstract bool Result { get; set; }
        /// <summary>
        /// 显示信息
        /// </summary>
        public abstract string Message { get; set; }
    }
    /// <summary>
    /// 结果对象
    /// </summary>
    public class ResultDto : IResultDto
    {
        /// <summary>
        /// 结果
        /// </summary>
        public override bool Result { get; set; }
        /// <summary>
        /// 显示信息
        /// </summary>
        public override string Message { get; set; }

        public ResultDto()
        {
        }
        public ResultDto(bool result, string message)
        {
            Result = result;
            Message = message;
        }
    }
    /// <summary>
    /// 删除结果
    /// </summary>
    public class DeleteResult : ResultDto
    {
        public DeleteResult()
        {
            Result = true;
            Message = "删除成功";
        }
        public DeleteResult(string message)
        {
            Result = false;
            Message = message;
        }
    }
    /// <summary>
    /// 修改结果
    /// </summary>
    public class UpdateResult : ResultDto
    {
        public UpdateResult()
        {
            Result = true;
            Message = "修改成功";
        }
        public UpdateResult(string message)
        {
            Result = false;
            Message = message;
        }
    }
    public class AddResult<T> : ResultDto
    {
        /// <summary>
        /// 新增返回的主键
        /// </summary>
        public T Id { get; set; }
        public AddResult(T Id)
        {
            this.Id = Id;
            Result = true;
            Message = "添加成功";
        }
        public AddResult(string message)
        {
            Result = false;
            Message = message;
        }
    }
}
