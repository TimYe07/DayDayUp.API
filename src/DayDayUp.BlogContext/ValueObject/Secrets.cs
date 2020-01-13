namespace DayDayUp.BlogContext.ValueObject
{
    public class Secrets
    {
        public Secret Tencent { get; set; }
    }

    public class Secret
    {
        public string SecretId { get; set; }
        public string SecretKey { get; set; }
    }
}