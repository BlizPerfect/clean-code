namespace Markdown.Tags.MdTags
{
    internal class PairedMdTag(string tagName) : Tag(tagName), ITag
    {
        public string OpeningTagName => TagName;

        public string ClosingTagName => TagName;
    }
}
