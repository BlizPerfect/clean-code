using FluentAssertions;
using Markdown.Parsers.MdParsers;
using Markdown.Renderers;
using System.Diagnostics;
using System.Text;

namespace Markdown.Tests
{
    internal class MdTests
    {
        private readonly Dictionary<string, string> _templates = new Dictionary<string, string>()
        {
            {"__Bold token__", "<strong>Bold token</strong>"},
            {"# Header token", "<h1>Header token</h1>"},
            {"# Header token 1\n# Header token 2", "<h1>Header token 1</h1><h1>Header token 2</h1>"},
            {"_Italic token_", "<em>Italic token</em>"},
            {"[Url name](Url link)", "<a href=\"Url link\">Url name</a>"},
            {"[_Url_ __name__ text](Url link)", "<a href=\"Url link\"><em>Url</em> <strong>name</strong> text</a>"},
            {"[NotAUrl", "[NotAUrl"},
            {"[NotAUrl]", "[NotAUrl]"},
            {"[NotAUrl](NotAUrl", "[NotAUrl](NotAUrl"},
            {"Text Token", "Text Token"},
            {"# _Set_ __of__ tokens", "<h1><em>Set</em> <strong>of</strong> tokens</h1>"},
            {"__Outer bold _Inner italic part_ outer bold__", "<strong>Outer bold <em>Inner italic part</em> outer bold</strong>"},
            {"_Outer italic __Inner Bold part__ outer Italic_", "<em>Outer italic __Inner Bold part__ outer Italic</em>"},
            {"Digits_12_3", "Digits_12_3"},
            {"Digits__12__3", "Digits__12__3"},
            {"__Sta__rt", "<strong>Sta</strong>rt"},
            {"_Sta_rt", "<em>Sta</em>rt"},
            {"S__tar__t", "S<strong>tar</strong>t"},
            {"S_tar_t", "S<em>tar</em>t"},
            {"St__art__", "St<strong>art</strong>"},
            {"St_art_", "St<em>art</em>"},
            {"Hel_lo, Wor_ld", "Hel_lo, Wor_ld"},
            {"Hel__lo, Wor__ld", "Hel__lo, Wor__ld"},
            {"_Hello world__", "_Hello world__"},
            {"__Hello world_", "__Hello world_"},
            {"_ Hello world_", "_ Hello world_"},
            {"_ Hello world _", "_ Hello world _"},
            {"_Hello world _", "_Hello world _"},
            {"__ Hello world__", "__ Hello world__"},
            {"__ Hello world __", "__ Hello world __"},
            {"__Hello world __", "__Hello world __"},
            {"_Hello__ _world__", "_Hello__ _world__"},
            {"__Hello_ __world_", "__Hello_ __world_"},
            {@"\_Hello world\_", "_Hello world_"},
            {@"_\_Hello world\__", "<em>_Hello world_</em>"},
            {@"\_\_Hello world\_\_", "__Hello world__"},
            {@"__\_\_Hello world\_\___", "<strong>__Hello world__</strong>"},
            {@"\[Url name](Url link)", "[Url name](Url link)"},
            {@"\", @"\"},
            {@"\a", "\\a"},
            {@"\__Italic token_", "_<em>Italic token</em>"},
            {@"Ste\gosaur\us", @"Ste\gosaur\us"},
            {@"\\_\\_Bold token\\_\\_", "<strong>Bold token</strong>"},
            {@"\\_Italic token\\_", "<em>Italic token</em>"},
            {"__", "__"},
            {"____", "____"},
        };

        [Test]
        public void Md_ThrowsException_ReceivingNullAsIParser()
        {
            Assert.Throws<ArgumentNullException>(() => new Md(null!, new RendererHTML()));
        }

        [Test]
        public void Md_ThrowsException_ReceivingNullAsIRenderer()
        {
            Assert.Throws<ArgumentNullException>(() => new Md(new ParserMd(), null!));
        }

        [TestCase(100, 10, 1.65)]
        public void Md_ShouldWorkInLinearTime(int iterations, int runsPerIteration, double threshold)
        {
            var md = new Md(new ParserMd(), new RendererHTML());

            var averageExecutionTimes = new double[iterations];
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < iterations; i++)
            {
                AddTemplates(stringBuilder);
                var repeatedTemplates = stringBuilder.ToString();

                var totalExecutionTime = 0.0;
                for (var j = 0; j < runsPerIteration; j++)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    var stopwatch = Stopwatch.StartNew();
                    md.Render(repeatedTemplates);
                    stopwatch.Stop();
                    totalExecutionTime += stopwatch.Elapsed.TotalMilliseconds;
                }
                averageExecutionTimes[i] = totalExecutionTime / runsPerIteration;
            }

            for (var i = 1; i < iterations; i++)
            {
                var growthFactor = averageExecutionTimes[i] / averageExecutionTimes[i - 1];
                Assert.That(growthFactor, Is.LessThanOrEqualTo(threshold));
            }
        }

        private void AddTemplates(StringBuilder stringBuilder)
        {
            foreach (var template in _templates)
            {
                stringBuilder.AppendLine(template.Key);
            }
        }

        [Test]
        public void Md_RendersCorrectly()
        {
            foreach (var template in _templates)
            {
                var md = new Md(new ParserMd(), new RendererHTML());
                var actual = md.Render(template.Key);
                actual.Should().Be(template.Value);
            }
        }
    }
}
