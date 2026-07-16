using System.Collections.Generic;

namespace MSC.IdeaForge.Domain.ValueObjects;

public class PatentAnalysisResult
{
    public string PatentRisk { get; set; } = string.Empty; // Düşük/Orta/Yüksek
    public List<string> ExistingPatents { get; set; } = new();
    public List<string> PatentableAspects { get; set; } = new();
    public List<string> Recommendations { get; set; } = new();
    public string FreedomToOperate { get; set; } = string.Empty;
}
