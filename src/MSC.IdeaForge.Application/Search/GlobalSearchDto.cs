using System;
using System.Collections.Generic;

namespace MSC.IdeaForge.Application.Search;

/// <summary>
/// Sistem geneli arama sonuçlarını kategorilere göre taşıyan DTO.
/// </summary>
public class GlobalSearchDto
{
    public List<SearchResultItem> Problems { get; set; } = new();
    public List<SearchResultItem> KnowledgeEntries { get; set; } = new();
    public List<SearchResultItem> FieldNotes { get; set; } = new();

    // Herhangi bir sonuç var mı?
    public bool HasResults => Problems.Count > 0 || KnowledgeEntries.Count > 0 || FieldNotes.Count > 0;
}

/// <summary>
/// Tek bir arama sonucu öğesi.
/// </summary>
public class SearchResultItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;

    // Sonuca tıklandığında gidilecek göreli adres
    public string Url { get; set; } = string.Empty;
}
