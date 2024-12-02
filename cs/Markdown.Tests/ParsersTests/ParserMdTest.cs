using FluentAssertions;
using Markdown.Parsers.MdParsers;
using Markdown.Tokens.HtmlTokens;

namespace Markdown.Tests.ParsersTests
{
    [TestFixture]
    public class ParserMdTests
    {
        private ParserMd _parser;
        private readonly List<IRenderable> _expected = new List<IRenderable>();

        [SetUp]
        public void SetUp()
        {
            _parser = new ParserMd();
            _expected.Clear();
        }

        [TestCase("__Bold token__")]
        public void Parse_SimpleBoldText_Correctly(string text)
        {
            _expected.Add(
                new BoldToken(
                    new TextToken("Bold token")));
            CheckCorrectness(text);
        }

        [TestCase("# Header token")]
        public void Parse_SimpleHeaderText_Correctly(string text)
        {
            _expected.Add(
                new HeaderToken(
                    new TextToken("Header token")));
            CheckCorrectness(text);
        }

        [TestCase("# Header token 1\n# Header token 2")]
        public void Parse_DoubleHeaderText_Correctly(string text)
        {
            _expected.Add(
                new HeaderToken(
                    new TextToken("Header token 1")));
            _expected.Add(
                new HeaderToken(
                    new TextToken("Header token 2")));
            CheckCorrectness(text);
        }

        [TestCase("_Italic token_")]
        public void Parse_SimpleItalicText_Correctly(string text)
        {
            _expected.Add(
                new ItalicToken(
                    new TextToken("Italic token")));
            CheckCorrectness(text);
        }

        [TestCase("[Url name](Url link)")]
        public void Parse_SimpleLinkText_Correctly(string text)
        {
            _expected.Add(
                new LinkToken(
                    new TextToken("Url name"), "Url link"));
            CheckCorrectness(text);
        }

        [TestCase("Text Token")]
        public void Parse_SimpleText_Correctly(string text)
        {
            _expected.Add(
                new TextToken(text));
            CheckCorrectness(text);
        }

        [TestCase("# _Set_ __of__ tokens")]
        public void Parse_ComplexText_Correctly(string text)
        {
            _expected.Add(
                new HeaderToken(
                    new SetToken(
                        new ItalicToken(
                            new TextToken("Set")),
                        new TextToken(" "),
                        new BoldToken(
                            new TextToken("of")),
                        new TextToken(" tokens"))));
            CheckCorrectness(text);
        }

        [TestCase("__Outer bold _Inner italic part_ outer bold__")]
        public void Parse_ItalicInsideBold_Correctly(string text)
        {
            _expected.Add(
                new BoldToken(
                    new SetToken(
                        new TextToken("Outer bold "),
                        new ItalicToken(
                            new TextToken("Inner italic part")),
                        new TextToken(" outer bold"))));
            CheckCorrectness(text);
        }

        [TestCase("__Sta__rt")]
        public void Parse_BoldInsideOneWordAtStart_Correctly(string text)
        {
            _expected.Add(
                new BoldToken(
                    new TextToken("Sta")));
            _expected.Add(new TextToken("rt"));
            CheckCorrectness(text);
        }

        [TestCase("_Sta_rt")]
        public void Parse_ItalicInsideOneWordAtStart_Correctly(string text)
        {
            _expected.Add(
               new ItalicToken(
                   new TextToken("Sta")));
            _expected.Add(new TextToken("rt"));
            CheckCorrectness(text);
        }

        [TestCase("S__tar__t")]
        public void Parse_BoldInsideOneWordAtCenter_Correctly(string text)
        {
            _expected.Add(
                new TextToken("S"));
            _expected.Add(
                new BoldToken(
                    new TextToken("tar")));
            _expected.Add(new TextToken("t"));
            CheckCorrectness(text);
        }

        [TestCase("S_tar_t")]
        public void Parse_ItalicInsideOneWordAtCenter_Correctly(string text)
        {
            _expected.Add(
                new TextToken("S"));
            _expected.Add(
                new ItalicToken(
                    new TextToken("tar")));
            _expected.Add(new TextToken("t"));
            CheckCorrectness(text);
        }

        [TestCase("St__art__")]
        public void Parse_BoldInsideOneWordAtEnd_Correctly(string text)
        {
            _expected.Add(
                new TextToken("St"));
            _expected.Add(
                new BoldToken(
                    new TextToken("art")));
            CheckCorrectness(text);
        }

        [TestCase("St_art_")]
        public void Parse_ItalicInsideOneWordAtEnd_Correctly(string text)
        {
            _expected.Add(
                new TextToken("St"));
            _expected.Add(
                new ItalicToken(
                    new TextToken("art")));
            CheckCorrectness(text);
        }

        [TestCase(@"\")]
        [TestCase(@"\a")]
        public void Parse_EscapedText_Correctly(string text)
        {
            _expected.Add(
                new TextToken(text));
            CheckCorrectness(text);
        }

        [TestCase(@"\__Italic token_")]
        public void Parse_EscapeThenToken_Correctly(string text)
        {
            _expected.Add(
                new TextToken("_"));
            _expected.Add(
                new ItalicToken(
                    new TextToken("Italic token")));
            CheckCorrectness(text);
        }

        [TestCase(@"Ste\gosaur\us")]
        public void Parse_EscapedNothing_Correctly(string text)
        {
            _expected.Add(
                new TextToken(text));
            CheckCorrectness(text);
        }

        [TestCase(@"\\_\\_Bold token\\_\\_")]
        public void Parse_BoldTokenEscapedEscapeSymbol_Correctly(string text)
        {
            _expected.Add(
                new BoldToken(
                    new TextToken("Bold token")));
            CheckCorrectness(text);
        }

        [TestCase(@"\\_Italic token\\_")]
        public void Parse_ItalicTokenEscapedEscapeSymbol_Correctly(string text)
        {
            _expected.Add(
                new ItalicToken(
                    new TextToken("Italic token")));
            CheckCorrectness(text);
        }

        private void CheckCorrectness(string text)
        {
            var actual = _parser.Parse(text);
            actual.Should().BeEquivalentTo(_expected, options => options.RespectingRuntimeTypes());
        }
    }
}
