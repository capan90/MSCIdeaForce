namespace MSC.IdeaForge.Domain.ValueObjects;

/// <summary>
/// Yapay zeka tarafından bulunan, bir problemle bağlantılı diğer problemi temsil eden değer nesnesi.
/// </summary>
public class RelatedProblem
{
    public string Title { get; set; } = string.Empty;
    public string RelationshipType { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
}
