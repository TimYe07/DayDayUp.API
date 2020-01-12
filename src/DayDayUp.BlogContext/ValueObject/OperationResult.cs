using System;
using System.Collections.Generic;

namespace DayDayUp.BlogContext.ValueObject
{
    public class OperationResult
    {
        /// <summary>
        ///     是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        ///     时间
        /// </summary>
        public DateTime TimeStamp { get; set; } = DateTime.Now;

        /// <summary>
        ///     消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     详细的错误说明
        /// </summary>
        public List<Error> Errors { get; set; } = new List<Error>();

        public static OperationResult Succeed(string message = "")
        {
            return new OperationResult {Success = true, Message = message};
        }

        public static OperationResult Fail(string message = "")
        {
            return new OperationResult {Success = false, Message = message};
        }
    }

    public class Error
    {
        public string Name { get; set; }
        public string Message { get; set; }
    }
}