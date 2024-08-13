using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Polly.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InventoryManagement.Shared.Infrastructure.Localization
{
    public class JsonStringLocalizer : IStringLocalizer
    {
        private readonly IDistributedCache _cache;

        public JsonStringLocalizer(IDistributedCache cache)
        {
            _cache = cache;
        }

        public LocalizedString this[string name]
        {
            get
            {
                string value = GetString(name);
                return new LocalizedString(name, value ?? name, value == null);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var actualValue = this[name];
                return !actualValue.ResourceNotFound
                    ? new LocalizedString(name, string.Format(actualValue.Value, arguments), false)
                    : actualValue;
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }

        private string GetString(string key)
        {
            string relativeFilePath = $"Resources/{Thread.CurrentThread.CurrentCulture.Name}.json";
            var fileDirPath = Assembly.GetExecutingAssembly().Location
            .Replace("InventoryManagement.Shared.Infrastructure.dll", "");
            string fullFilePath = System.IO.Path.Combine(fileDirPath, relativeFilePath);
            if (File.Exists(fullFilePath))
            {
                string cacheKey = $"locale_{Thread.CurrentThread.CurrentCulture.Name}_{key}";
                string cacheValue = _cache.GetString(cacheKey)!;
                if (!string.IsNullOrEmpty(cacheValue)) return cacheValue;
                string result = GetValueFromJSON(key, fullFilePath);
                if (!string.IsNullOrEmpty(result)) _cache.SetString(cacheKey, result);
                return result;
            }

            return default(string)!;
        }

        private string GetValueFromJSON(string propertyName, string filePath)
        {
            if (propertyName == null) return default!;
            if (filePath == null) return default!;

            var options = new JsonReaderOptions
            {
                AllowTrailingCommas = true,
                CommentHandling = JsonCommentHandling.Skip
            };
            ReadOnlySpan<byte> jsonReadOnlySpan = File.ReadAllBytes(filePath);
            var reader = new Utf8JsonReader(jsonReadOnlySpan, options);
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.PropertyName && reader.GetString() == propertyName)
                {
                    reader.Read();
                    return reader.GetString()!;
                }
            }
            return default!;
        }
    }
}