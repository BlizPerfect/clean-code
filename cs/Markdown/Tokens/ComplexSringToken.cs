using Markdown.Tags;
using System.Text;

namespace Markdown.Tokens
{
    internal class ComplexSringToken : Token
    {
        public ITag Tag { get => _tag!; }
        public Token[] InnerTokens { get => _innerTokens!.ToArray(); }

        public ComplexSringToken(ITag tag, IList<Token> innerTokens)
            : base(tag: tag, innerTokens: innerTokens)
        {
            if (tag is null)
            {
                throw new ArgumentException("Передаваемый токен не может быть null");
            }

            if (innerTokens is null)
            {
                throw new ArgumentException("Передаваемые внутренние токены не могут быть null");
            }

            if (innerTokens.Count == 0)
            {
                throw new ArgumentException(
                    "Передаваемые внутренние токены не могут быть пустыми");
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(Tag.OpeningTagName);
            foreach (var token in InnerTokens)
            {
                if (token is SimpleStringToken || token is MarkdownStringToken)
                {
                    sb.Append(token.ToString());
                    continue;
                }
                sb.Append(token.ToString());
            }
            sb.Append(Tag.ClosingTagName);

            return sb.ToString();
        }
    }
}
