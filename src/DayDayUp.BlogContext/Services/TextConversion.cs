using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DayDayUp.BlogContext.ValueObject;
using JiebaNet.Segmenter;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Nlp.V20190408;
using TencentCloud.Nlp.V20190408.Models;
using TencentCloud.Tmt.V20180321;
using TencentCloud.Tmt.V20180321.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
            _keywordProcessor = Init();
            _logger = logger;
        }

        private readonly HttpClient _client;
        private readonly Secrets _secrets;
        private readonly KeywordProcessor _keywordProcessor;
        private readonly ILogger<TextConversion> _logger;

        public async Task<TextDocument> ToMarkdownAsync(string content)
        {
            var data = new
            {
                text = content,
                type = "md",
                extraDocData = true
            };
            var httpContent = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
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

        public string ExtractKeywords(string text, int worldCount)
        {
            var keywords = _keywordProcessor.ExtractKeywords(text);
            return !keywords.Any()
                ? string.Empty
                : string.Join(",", keywords.Count() > worldCount ? keywords.Take(worldCount) : keywords);
        }

        private KeywordProcessor Init()
        {
            var kp = new KeywordProcessor();
            var tagList = new List<TagOptions>();
            using (var sr =
                File.OpenText($"{Path.GetDirectoryName(typeof(TextConversion).Assembly.Location)}/Data/tags.json"))
            {
                var json = sr.ReadToEnd();
                tagList = JsonSerializer.Deserialize<List<TagOptions>>(json);
            }

            var worldList = new List<string>();
            foreach (var tag in tagList)
            {
                worldList.Add(tag.Name);
                if (tag.Alias.Any()) worldList.AddRange(tag.Alias);
            }

            kp.AddKeywords(worldList);

            return kp;
        }
    }

    public class TagOptions
    {
        public string Name { get; set; }
        public string[] Alias { get; set; }
    }
}