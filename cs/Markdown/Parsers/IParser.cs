using Markdown.Tokens;

namespace Markdown.Parsers
{
    internal interface IParser
    {
        IList<Token> ParseToTokens(string textToParse);
    }
}
