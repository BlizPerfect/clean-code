using Markdown.Tags;
using Markdown.Tags.MdTags;
using Markdown.Tokens;

namespace Markdown.Parsers.MdParsers
{
    internal class ParserMD : IParser
    {
        private readonly Dictionary<string, ITag> _mdTags = new Dictionary<string, ITag>()
        {
            {"_", new PairedMdTag("_") },
            {"__", new PairedMdTag("__") },
            {"# ", new SingleMdTag("# ") }
        };

        public IList<Token> ParseToTokens(string textToParse)
        {
            throw new NotImplementedException();
        }
    }
}
