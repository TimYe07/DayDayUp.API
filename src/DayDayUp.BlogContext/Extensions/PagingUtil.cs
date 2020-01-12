namespace DayDayUp.BlogContext.Extensions
{
    public static class PagingUtil
    {
        public static int QueryPageValidator(int page)
        {
            page = page < 0 ? 1 : page;
            return page;
        }

        public static int QuerySizeValidator(int size)
        {
            size = size < 0 ? 1 : size;
            return size;
        }

        public static int QueryPageLimit(int page, int limit = 100)
        {
            page = QueryPageValidator(page);
            page = page > limit ? limit : page;
            return page;
        }

        public static int QuerySizeLimit(int size, int limit = 50)
        {
            size = QuerySizeValidator(size);
            size = size > limit ? limit : size;
            return size;
        }

        public static int CalculateTotalPage(int totalCount, int size, int? limit)
        {
            var totalPage = (totalCount + size - 1) / size;
            if (limit.HasValue)
                return totalCount > limit.Value ? limit.Value : totalCount;

            return totalPage;
        }
    }
}