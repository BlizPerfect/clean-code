﻿using Markdown.Tokens.HtmlTokens;

namespace Markdown.Renderers
{
    internal interface IRenderer
    {
        void RenderText(TextToken textToken);
        void RenderItalic(ItalicToken italicToken);
        void RenderBold(BoldToken boldToken);
        void RenderHeader(HeaderToken headerToken);
        void RenderLink(LinkToken linkToken);
        void RenderSet(SetToken setToken);
    }
}
