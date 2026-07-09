using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Enums;

namespace MSC.IdeaForge.Application.Signals.Services;

/// <summary>
/// RSS veya Atom kaynaklarından XML parse ederek ham sinyalleri içe aktaran servis.
/// </summary>
public class RSSImporter
{
    private readonly HttpClient _httpClient;

    public RSSImporter(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Belirtilen RSS/Atom URL adresinden haberleri veya yayınları indirip ImportedSignal listesi olarak döner.
    /// </summary>
    public async Task<List<ImportedSignal>> ImportFromRSSAsync(string rssUrl)
    {
        var xmlString = await _httpClient.GetStringAsync(rssUrl);
        var doc = XDocument.Parse(xmlString);
        var list = new List<ImportedSignal>();

        // RSS 2.0 kontrolü
        var items = doc.Descendants("item");
        if (items.Any())
        {
            foreach (var item in items)
            {
                var title = item.Element("title")?.Value ?? "Başlıksız RSS Sinyali";
                var link = item.Element("link")?.Value;
                var description = item.Element("description")?.Value ?? string.Empty;
                var pubDateStr = item.Element("pubDate")?.Value;

                DateTime pubDate = DateTime.UtcNow;
                if (!string.IsNullOrEmpty(pubDateStr) && DateTime.TryParse(pubDateStr, out var parsedDate))
                {
                    pubDate = parsedDate.ToUniversalTime();
                }

                // İçeriği HTML taglerinden temizlemek için basit regex temizleme veya doğrudan alma
                string cleanDesc = StripHtmlTags(description);
                string summary = cleanDesc.Length > 200 ? cleanDesc.Substring(0, 200) + "..." : cleanDesc;

                list.Add(ImportedSignal.Create(
                    ImportSourceType.RSS,
                    link,
                    cleanDesc,
                    title,
                    summary,
                    pubDate
                ));
            }
        }
        else
        {
            // Atom Feed kontrolü
            XNamespace ns = "http://www.w3.org/2005/Atom";
            var entries = doc.Descendants(ns + "entry");
            foreach (var entry in entries)
            {
                var title = entry.Element(ns + "title")?.Value ?? "Başlıksız Atom Sinyali";
                var link = entry.Element(ns + "link")?.Attribute("href")?.Value;
                var content = entry.Element(ns + "content")?.Value 
                              ?? entry.Element(ns + "summary")?.Value 
                              ?? string.Empty;
                var publishedStr = entry.Element(ns + "published")?.Value 
                                   ?? entry.Element(ns + "updated")?.Value;

                DateTime pubDate = DateTime.UtcNow;
                if (!string.IsNullOrEmpty(publishedStr) && DateTime.TryParse(publishedStr, out var parsedDate))
                {
                    pubDate = parsedDate.ToUniversalTime();
                }

                string cleanContent = StripHtmlTags(content);
                string summary = cleanContent.Length > 200 ? cleanContent.Substring(0, 200) + "..." : cleanContent;

                list.Add(ImportedSignal.Create(
                    ImportSourceType.RSS,
                    link,
                    cleanContent,
                    title,
                    summary,
                    pubDate
                ));
            }
        }

        return list;
    }

    /// <summary>
    /// Ham içeriklerdeki HTML taglerini temizler.
    /// </summary>
    private static string StripHtmlTags(string html)
    {
        if (string.IsNullOrWhiteSpace(html)) return string.Empty;
        var array = new char[html.Length];
        int arrayIndex = 0;
        bool inside = false;

        for (int i = 0; i < html.Length; i++)
        {
            char let = html[i];
            if (let == '<')
            {
                inside = true;
                continue;
            }
            if (let == '>')
            {
                inside = false;
                continue;
            }
            if (!inside)
            {
                array[arrayIndex] = let;
                arrayIndex++;
            }
        }
        return new string(array, 0, arrayIndex).Trim();
    }
}
