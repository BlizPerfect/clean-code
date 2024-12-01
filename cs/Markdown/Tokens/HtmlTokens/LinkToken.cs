using Markdown.Renderers;

namespace Markdown.Tokens.HtmlTokens
{
    internal class LinkToken : BasicMarkdownToken, IRenderable
    {
        public readonly string Link;

        public LinkToken(IRenderable innerItem, string link) : base(innerItem)
        {
            if (string.IsNullOrWhiteSpace(link))
            {
                throw new ArgumentNullException(
                    "Ссылка не может быть null или пустой строкой");
            }
            Link = link;
        }

        public void Render(IRenderer renderer)
        {
            ArgumentNullException.ThrowIfNull(renderer);

            renderer.RenderLink(this);
        }
    }
}

