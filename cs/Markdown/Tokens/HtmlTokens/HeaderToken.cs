using Markdown.Renderers;

namespace Markdown.Tokens.HtmlTokens
{
    internal class HeaderToken(IRenderable innerItem) : BasicMarkdownToken(innerItem), IRenderable
    {
        public void Render(IRenderer renderer)
        {
            ArgumentNullException.ThrowIfNull(renderer);

            renderer.RenderHeader(this);
        }
    }
}