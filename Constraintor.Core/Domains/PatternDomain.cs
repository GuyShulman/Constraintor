using System.Text.RegularExpressions;

namespace Constraintor.Core.Domains;

public class PatternDomain : Domain
{
    public string Pattern { get; }
    private readonly Regex _regex;

    public PatternDomain(string pattern)
    {
        Pattern = pattern ?? throw new ArgumentNullException(nameof(pattern));
        _regex = new Regex(Pattern, RegexOptions.Compiled);
    }

    // TODO implement if needed
    public override IEnumerable<object>? GetValues() => null;

    public override bool Contains(object value) => value is string str && _regex.IsMatch(str);

    public override Domain Intersect(Domain other) => this;

    public override string ToString() => $"Pattern: {Pattern}";
}