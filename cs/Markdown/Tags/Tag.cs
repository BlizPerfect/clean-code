namespace Markdown.Tags
{
    internal abstract class Tag
    {
        protected Tag(string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName))
            {
                throw new ArgumentException(
                    "Имя тега не может быть null или пустой строкой");
            }
            TagName = tagName;
        }
        public string TagName { get; }
    }
}
