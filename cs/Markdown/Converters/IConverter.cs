using Markdown.Tokens;

namespace Markdown.Converters
{
    internal interface IConverter
    {
        IList<Token> Convert(IList<Token> tokensToConvert);
    }
}
