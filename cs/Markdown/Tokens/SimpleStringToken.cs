namespace Markdown.Tokens
{
    internal class SimpleStringToken : Token
    {
        public string Content { get => _content!; }

        public SimpleStringToken(string content) : base(content: content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException(
                    "Содержимое токена не может быть null или пустой строкой");
            }
        }

        public override string ToString() => Content;
    }
}
