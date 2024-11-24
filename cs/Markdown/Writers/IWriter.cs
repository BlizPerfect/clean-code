using Markdown.Tokens;

namespace Markdown.Writers
{
    internal interface IWriter
    {
        string Write(IList<Token> tokensToWrite);
    }
}
