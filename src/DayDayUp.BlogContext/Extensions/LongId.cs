using Snowflake.Core;

namespace DayDayUp.BlogContext.Extensions
{
    public static class LongId
    {
        private static readonly IdWorker _IdWorker = new IdWorker(1, 1);

        /// <summary>
        ///     long 类型Id
        /// </summary>
        /// <returns></returns>
        public static long GenerateNewId()
        {
            var id = _IdWorker.NextId();
            return id;
        }
    }
}