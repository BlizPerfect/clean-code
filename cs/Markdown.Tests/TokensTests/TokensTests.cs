using Markdown.Tokens.HtmlTokens;

namespace Markdown.Tests.TokensTests
{
    [TestFixture]
    public class TokensTests
    {
        private readonly string _dummyText = "Hello, World!";
        private readonly TextToken _dummyTextToken = new TextToken("Hello, World!");
        private readonly string _dummyLink = "https://www.google.com";

        [Test]
        public void BoldToken_ThrowsException_ReceivingNullAsIRenderable()
        {
            Assert.Throws<ArgumentNullException>(() => new BoldToken(null!));
        }

        [Test]
        public void BoldToken_ThrowsException_ReceivingNullAsIRenderer_OnRender()
        {
            Assert.Throws<ArgumentNullException>(()
                => new BoldToken(_dummyTextToken).Render(null!));
        }

        [Test]
        public void HeaderToken_ThrowsException_ReceivingNullAsIRenderable()
        {
            Assert.Throws<ArgumentNullException>(() => new HeaderToken(null!));
        }

        [Test]
        public void HeaderToken_ThrowsException_ReceivingNullAsIRenderer_OnRender()
        {
            Assert.Throws<ArgumentNullException>(()
                => new HeaderToken(_dummyTextToken).Render(null!));
        }

        [Test]
        public void ItalicToken_ThrowsException_ReceivingNullAsIRenderable()
        {
            Assert.Throws<ArgumentNullException>(() => new ItalicToken(null!));
        }

        [Test]
        public void ItalicToken_ThrowsException_ReceivingNullAsIRenderer_OnRender()
        {
            Assert.Throws<ArgumentNullException>(()
                => new ItalicToken(_dummyTextToken).Render(null!));
        }

        [Test]
        public void LinkToken_ThrowsException_ReceivingNullAsIRenderable()
        {
            Assert.Throws<ArgumentNullException>(() => new LinkToken(null!, _dummyLink));
        }

        [TestCase(null!)]
        [TestCase("")]
        [TestCase(" ")]
        public void LinkToken_ThrowsException_ReceivingullOrWhiteSpaceAsLink(string link)
        {
            var dummy = new TextToken("Dummy text");
            Assert.Throws<ArgumentNullException>(() => new LinkToken(dummy, link));
        }

        [Test]
        public void LinkToken_ThrowsException_ReceivingNullAsIRenderer_OnRender()
        {
            Assert.Throws<ArgumentNullException>(()
                => new LinkToken(_dummyTextToken, _dummyLink).Render(null!));
        }

        [Test]
        public void SetToken_ThrowsException_ReceivingNullAsIRenderable()
        {
            Assert.Throws<ArgumentNullException>(() => new SetToken(null!));
        }

        [Test]
        public void SetToken_ThrowsException_ReceivingNullAsIRenderer_OnRender()
        {
            Assert.Throws<ArgumentNullException>(()
                => new SetToken(_dummyTextToken).Render(null!));
        }

        [Test]
        public void TextToken_ThrowsException_ReceivingNullAsText()
        {
            Assert.Throws<ArgumentNullException>(() => new TextToken(null!));
        }

        [Test]
        public void TextToken_ThrowsException_ReceivingNullAsIRenderer_OnRender()
        {
            Assert.Throws<ArgumentNullException>(()
                => new TextToken(_dummyText).Render(null!));
        }
    }
}
