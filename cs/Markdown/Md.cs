using Markdown.Parsers;
using Markdown.Renderers;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Markdown.Tests")]
namespace Markdown
{
    internal class Md(IParser parser, IRenderer renderer)
    {
        public readonly IParser Parser = parser ?? throw new ArgumentNullException(
                "Передаваемый IParser не может быть null.");

        public readonly IRenderer Renderer = renderer ?? throw new ArgumentNullException(
                "Передаваемый IRenderer не может быть null.");

        public string Render(string text)
        {
            var mdTokens = Parser.Parse(text);
            foreach (var token in mdTokens)
            {
                token.Render(Renderer);
            }
            return Renderer.ToString()!;
        }
    }
}
