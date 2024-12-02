using FluentAssertions;
using Markdown.Parsers.MdParsers;
using Markdown.Renderers;
using System.Diagnostics;
using System.Text;

namespace Markdown.Tests
{
    internal class MdTests
    {
        private readonly string[] _templates = new string[]
        {
            "__Bold token__",
            "# Header token",
            "# Header token 1\n# Header token 2",
            "_Italic token_",
            "[Url name](Url link)",
            "[NotAUrl",
            "[NotAUrl]",
            "[NotAUrl](NotAUrl",
            "Text Token",
            "# _Set_ __of__ tokens",
            "__Outer bold _Inner italic part_ outer bold__",
            "_Outer italic __Inner Bold part__ outer Italic_",
            "Digits_12_3",
            "Digits__12__3",
            "__Sta__rt",
            "_Sta_rt",
            "S__tar__t",
            "S_tar_t",
            "St__art__",
            "St_art_",
            "Hel_lo, Wor_ld",
            "Hel__lo, Wor__ld",
            "_Hello world__",
            "__Hello world_",
            "_ Hello world_",
            "_ Hello world _",
            "_Hello world _",
            "__ Hello world__",
            "__ Hello world __",
            "__Hello world __",
            "_Hello__ _world__",
            "__Hello_ __world_",
            @"\_Hello world\_",
            @"_\_Hello world\__",
            @"\_\_Hello world\_\_",
            @"__\_\_Hello world\_\___",
            @"\[Url name](Url link)",
            @"\",
            @"\a",
            @"\__Italic token_",
            @"Ste\gosaur\us",
            @"\\_\\_Bold token\\_\\_",
            @"\\_Italic token\\_",
            "__",
            "____",
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

        [TestCase("__Bold token__", "<strong>Bold token</strong>")]
        [TestCase("# Header token", "<h1>Header token</h1>")]
        [TestCase("# Header token 1\n# Header token 2", "<h1>Header token 1</h1><h1>Header token 2</h1>")]
        [TestCase("_Italic token_", "<em>Italic token</em>")]
        [TestCase("[Url name](Url link)", "<a href=\"Url link\">Url name</a>")]
        [TestCase("[NotAUrl", "[NotAUrl")]
        [TestCase("[NotAUrl]", "[NotAUrl]")]
        [TestCase("[NotAUrl](NotAUrl", "[NotAUrl](NotAUrl")]
        [TestCase("Text Token", "Text Token")]
        [TestCase("# _Set_ __of__ tokens", "<h1><em>Set</em> <strong>of</strong> tokens</h1>")]
        [TestCase("__Outer bold _Inner italic part_ outer bold__", "<strong>Outer bold <em>Inner italic part</em> outer bold</strong>")]
        [TestCase("_Outer italic __Inner Bold part__ outer Italic_", "<em>Outer italic __Inner Bold part__ outer Italic</em>")]
        [TestCase("Digits_12_3", "Digits_12_3")]
        [TestCase("Digits__12__3", "Digits__12__3")]
        [TestCase("__Sta__rt", "<strong>Sta</strong>rt")]
        [TestCase("_Sta_rt", "<em>Sta</em>rt")]
        [TestCase("S__tar__t", "S<strong>tar</strong>t")]
        [TestCase("S_tar_t", "S<em>tar</em>t")]
        [TestCase("St__art__", "St<strong>art</strong>")]
        [TestCase("St_art_", "St<em>art</em>")]
        [TestCase("Hel_lo, Wor_ld", "Hel_lo, Wor_ld")]
        [TestCase("Hel__lo, Wor__ld", "Hel__lo, Wor__ld")]
        [TestCase("_Hello world__", "_Hello world__")]
        [TestCase("__Hello world_", "__Hello world_")]
        [TestCase("_ Hello world_", "_ Hello world_")]
        [TestCase("_ Hello world _", "_ Hello world _")]
        [TestCase("_Hello world _", "_Hello world _")]
        [TestCase("__ Hello world__", "__ Hello world__")]
        [TestCase("__ Hello world __", "__ Hello world __")]
        [TestCase("__Hello world __", "__Hello world __")]
        [TestCase("_Hello__ _world__", "_Hello__ _world__")]
        [TestCase("__Hello_ __world_", "__Hello_ __world_")]
        [TestCase(@"\_Hello world\_", "_Hello world_")]
        [TestCase(@"_\_Hello world\__", "<em>_Hello world_</em>")]
        [TestCase(@"\_\_Hello world\_\_", "__Hello world__")]
        [TestCase(@"__\_\_Hello world\_\___", "<strong>__Hello world__</strong>")]
        [TestCase(@"\[Url name](Url link)", "[Url name](Url link)")]
        [TestCase(@"\", @"\")]
        [TestCase(@"\a", "\\a")]
        [TestCase(@"\__Italic token_", "_<em>Italic token</em>")]
        [TestCase(@"Ste\gosaur\us", @"Ste\gosaur\us")]
        [TestCase(@"\\_\\_Bold token\\_\\_", "<strong>Bold token</strong>")]
        [TestCase(@"\\_Italic token\\_", "<em>Italic token</em>")]
        [TestCase("__", "__")]
        [TestCase("____", "____")]
        public void Md_RendersCorrectly(string text, string expected)
        {
            var md = new Md(new ParserMd(), new RendererHTML());
            var actual = md.Render(text);
            actual.Should().Be(expected);
        }


        private void AddTemplates(StringBuilder stringBuilder)
        {
            foreach (var template in _templates)
            {
                stringBuilder.AppendLine(template);
            }
        }
    }
}
