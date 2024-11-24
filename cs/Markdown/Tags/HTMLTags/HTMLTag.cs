namespace Markdown.Tags.HTMLTags
{
    internal class HTMLTag(string tagName) : Tag(tagName), ITag
    {
        public string OpeningTagName => $"<{TagName}>";

        public string ClosingTagName => $"</{TagName}>";
    }
}
