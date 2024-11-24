namespace Markdown.Tags
{
    internal interface ITag
    {
        string OpeningTagName { get; }
        string ClosingTagName { get; }
    }
}
