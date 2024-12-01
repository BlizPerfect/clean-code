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

            for (int i = 1; i < iterations; i++)
            {
                var growthFactor = averageExecutionTimes[i] / averageExecutionTimes[i - 1];
                Assert.That(growthFactor, Is.LessThanOrEqualTo(threshold));
            }
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
