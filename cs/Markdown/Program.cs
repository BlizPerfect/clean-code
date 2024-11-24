using Markdown.Converters.MdToHTMLConverters;
using Markdown.Parsers.MdParsers;
using Markdown.Tags;
using Markdown.Tags.HTMLTags;
using Markdown.Tags.MdTags;
using Markdown.Tokens;
using Markdown.Writers;

namespace Markdown
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var simpleText = "Hello, Andrey";
            var simpleTokenMD = new SimpleStringToken(simpleText);
            var simpleTokenHTML = new SimpleStringToken("Hello, Andrey");
            ShowEaxmple(simpleText, simpleTokenMD, simpleTokenHTML);


            var markdownText = "_Hello, Andrey_";
            var markdownTokenMD = new MarkdownStringToken(new PairedMdTag("_"), "Hello, Andrey");
            var markdownTokenHTML = new MarkdownStringToken(new HTMLTag("em"), "Hello, Andrey");
            ShowEaxmple(markdownText, markdownTokenMD, markdownTokenHTML);


            var complexText = "__с _разными_ символами__";
            var complexInnerTokensMD = new List<Token>()
            {
                new SimpleStringToken("c "),
                new MarkdownStringToken(new PairedMdTag("_"),"разными"),
                new SimpleStringToken(" символами")
            };
            var complexInnerTokensHTML = new List<Token>()
            {
                new SimpleStringToken("c "),
                new MarkdownStringToken(new HTMLTag("em"),"разными"),
                new SimpleStringToken(" символами")
            };
            var complexTokenMD = new ComplexSringToken(new PairedMdTag("__"), complexInnerTokensMD);
            var complexTokenHTML = new ComplexSringToken(new HTMLTag("strong"), complexInnerTokensHTML);
            ShowEaxmple(complexText, complexTokenMD, complexTokenHTML);


            var complexHeaderText = "# Заголовок __с _разными_ символами__";
            var complexHeaderInnerTokensMD = new List<Token>()
            {
                new SimpleStringToken("Заголовок "),
                new ComplexSringToken(new PairedMdTag("__"), complexInnerTokensMD)
            };
            var complexHeaderInnerTokensHTML = new List<Token>()
            {
                new SimpleStringToken("Заголовок "),
                new ComplexSringToken(new HTMLTag("strong"), complexInnerTokensHTML)
            };
            var complexHeaderTokenMD = new ComplexSringToken(new SingleMdTag("# "), complexHeaderInnerTokensMD);
            var complexHeaderTokenHTML = new ComplexSringToken(new HTMLTag("h1"), complexHeaderInnerTokensHTML);
            ShowEaxmple(complexHeaderText, complexHeaderTokenMD, complexHeaderTokenHTML);

            Console.ReadLine();

            var exampleText = "# Заголовок __с _разными_ символами__";
            var parser = new ParserMD();
            var converter = new ConverterMdToHTML();
            var writer = new WriterHTML();
            var md = new Md(parser, converter, writer);
            var htmlCode = md.Render(exampleText);
        }

        private static void ShowEaxmple(
            string rawText,
            Token tokenMD,
            Token tokenHTML)
        {
            Console.WriteLine("Пример:");
            Console.WriteLine($"Переданный текст: {rawText}");
            Console.WriteLine($"Востановленный из токена MD текст: {tokenMD}");
            Console.WriteLine($"Востановленный из токена HTML текст: {tokenHTML}");
            Console.WriteLine();
        }
    }
}
