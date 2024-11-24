using Markdown.Tags;
using Markdown.Tokens;
using Markdown.Tags.HTMLTags;

namespace Markdown.Converters.MdToHTMLConverters
{
    internal class ConverterMdToHTML : IConverter
    {
        private readonly Dictionary<string, ITag> _mdTagNamesToHTMLTags
            = new Dictionary<string, ITag>()
        {
            {"_", new HTMLTag("em") },
            {"__", new HTMLTag("strong") },
            {"# ", new HTMLTag("h1") },
        };

        public IList<Token> Convert(IList<Token> tokensToConvert)
        {
            throw new NotImplementedException();
        }
    }
}
