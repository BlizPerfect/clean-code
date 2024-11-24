using Markdown.Tags;

namespace Markdown.Tokens
{
    internal abstract class Token(
        ITag? tag = null,
        string? content = null,
        IList<Token>? innerTokens = null)
    {
        protected readonly ITag? _tag = tag;
        protected readonly string? _content = content;
        protected readonly IList<Token>? _innerTokens = innerTokens;
    }
}

