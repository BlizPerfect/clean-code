using Markdown.Tags;

namespace Markdown.Tokens
{
    internal class MarkdownStringToken : Token
    {
        public ITag Tag { get => _tag!; }

        public string Content { get => _content!; }

        public MarkdownStringToken(ITag tag, string content) : base(tag: tag, content: content)
        {
            if (tag is null)
            {
                throw new ArgumentException("Передаваемый токен не может быть null");
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException(
                    "Содержимое токена не может быть null или пустой строкой");
            }
        }

        public override string ToString()
            => $"{Tag.OpeningTagName}{Content}{Tag.ClosingTagName}";
    }
}
