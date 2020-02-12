using System;
using HashidsNet;
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

        public static string EncodeLongId(this long id, string salt)
        {
            if (string.IsNullOrEmpty(salt))
            {
                throw new ArgumentNullException(nameof(salt));
            }

            var hashids = new Hashids(salt);
            return hashids.EncodeLong(id);
        }

        public static long DecodeLongId(this string encodeLongId, string salt)
        {
            if (string.IsNullOrEmpty(salt))
            {
                throw new ArgumentNullException(nameof(salt));
            }

            var hashids = new Hashids(salt);
            return hashids.DecodeLong(encodeLongId)[0];
        }
    }
}