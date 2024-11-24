namespace Markdown.Tags.MdTags
{
    internal class SingleMdTag(string tagName) : Tag(tagName), ITag
    {
        public string OpeningTagName => TagName;

        public string ClosingTagName => string.Empty;
    }
}
