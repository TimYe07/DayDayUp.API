using System;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DayDayUp.BlogContext.ValueObject;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Nlp.V20190408;
using TencentCloud.Nlp.V20190408.Models;
using TencentCloud.Tmt.V20180321;
using TencentCloud.Tmt.V20180321.Models;

namespace DayDayUp.BlogContext.Services
{
    public class TextConversion : ITextConversionService
    {
        public TextConversion
        (
            IHttpClientFactory clientFactory,
            IOptions<Secrets> options,
            ILogger<TextConversion> logger)
        {
            _client = clientFactory.CreateClient("markdown");
            _secrets = options.Value;
            _logger = logger;
        }

        private readonly HttpClient _client;
        private readonly Secrets _secrets;
        private readonly ILogger<TextConversion> _logger;

        public async Task<TextDocument> ToMarkdownAsync(string content)
        {
            var data = new
            {
                text = content,
                type = "md",
                extraDocData = true
            };
            var httpContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/text/render", httpContent);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("markdown render was error...");
                return new TextDocument()
                {
                    Doc = new Document()
                    {
                        Html = content,
                        Source = content
                    },
                    Languages = new string[] { },
                    Toc = new TocItem[] { }
                };
            }

            var articleStr = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TextDocument>(articleStr);
        }

        public async Task<string> GenerateSlugAsync(string title)
        {
            if (Regex.IsMatch(title, @"^\w+$"))
            {
                return title;
            }

            try
            {
                var cred = new Credential
                {
                    SecretId = _secrets.Tencent.SecretId,
                    SecretKey = _secrets.Tencent.SecretKey
                };

                var clientProfile = new ClientProfile();
                var httpProfile = new HttpProfile();
                httpProfile.Endpoint = ("tmt.tencentcloudapi.com");
                clientProfile.HttpProfile = httpProfile;
                TmtClient client = new TmtClient(cred, "ap-guangzhou", clientProfile);
                TextTranslateRequest req = new TextTranslateRequest();
                var strParams = JsonConvert.SerializeObject(new
                {
                    SourceText = title,
                    Source = "zh",
                    Target = "en",
                    ProjectId = 0
                });
                req = TextTranslateRequest.FromJsonString<TextTranslateRequest>(strParams);
                var resp = await client.TextTranslate(req);

                var targetText = Regex.Replace(resp.TargetText.ToLower(), @"([\p{P}*])", "");

                return targetText.Replace(" ", "-");
            }
            catch (TencentCloudSDKException e)
            {
                _logger.LogError(e.Message);
                return string.Empty;
            }
        }

        public async Task<string> GenerateSummaryAsync(string content)
        {
            try
            {
                content = MarkdownUtility.RemoveMarkdownTags(content);
                if (content.Length <= 0)
                {
                    return string.Empty;
                }

                var splitLength = content.Length >= 2000 ? 2000 : content.Length;
                content = content.AsSpan().Slice(0, splitLength).ToString();

                var cred = new Credential
                {
                    SecretId = _secrets.Tencent.SecretId,
                    SecretKey = _secrets.Tencent.SecretKey
                };

                var clientProfile = new ClientProfile();
                var httpProfile = new HttpProfile();
                httpProfile.Endpoint = ("nlp.tencentcloudapi.com");
                clientProfile.HttpProfile = httpProfile;

                var client = new NlpClient(cred, "ap-guangzhou", clientProfile);
                AutoSummarizationRequest req = new AutoSummarizationRequest();
                var strParams = JsonConvert.SerializeObject(new
                {
                    Text = content
                });
                req = AutoSummarizationRequest.FromJsonString<AutoSummarizationRequest>(strParams);
                var resp = await client.AutoSummarization(req);
                return resp.Summary;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return string.Empty;
            }
        }
    }
}