using Markdown.Converters;
using Markdown.Parsers;
using Markdown.Writers;

namespace Markdown
{
    internal class Md(IParser parser, IConverter converter, IWriter writer)
    {
        private readonly IParser _parser = parser;
        private readonly IConverter _converter = converter;
        private readonly IWriter _writer = writer;

        public string Render(string text)
        {
            var mdTokens = _parser.ParseToTokens(text);

            var HTMLTokens = _converter.Convert(mdTokens);

            return _writer.Write(HTMLTokens);
        }
    }
}
