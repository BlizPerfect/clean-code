using Markdown.Tokens.HtmlTokens;

namespace Markdown.Parsers.MdParsers
{
    internal class ParserMd : IParser
    {
        private const string BoldMarkerStart = "__";
        private const string ItalicMarkerStart = "_";
        private const string LinkNameStart = "[";
        private const string LinkNameEnd = "]";
        private const string LinkUrlStart = "(";
        private const string LinkUrlEnd = ")";
        private const string HeaderMarkerStart = "# ";

        private static readonly HashSet<char> _markers = new HashSet<char>()
        {
            '_',
            '#',
            '[',
        };

        public IList<IRenderable> Parse(string text)
        {
            text = text.Replace(@"\\", "");
            return ParseTextPart(text);
        }

        private static List<IRenderable> ParseTextPart(string text)
        {
            var tokens = new List<IRenderable>();
            var index = 0;
            while (index < text.Length)
            {
                if (IsEscapeStart(text, index))
                {
                    index = ParseEscape(text, index, tokens);
                }
                else if (IsHeaderStart(text, index))
                {
                    index = ParseHeader(text, index, tokens);
                }
                else if (IsUrlStart(text, index))
                {
                    index = ParseLink(text, index, tokens);
                }
                else if (IsBoldStart(text, index))
                {
                    index = ParseBold(text, index, tokens);
                }
                else if (IsItalicStart(text, index))
                {
                    index = ParseItalic(text, index, tokens);
                }
                else
                {
                    index = ParseText(text, index, tokens);
                }
            }
            return tokens;
        }

        private static bool IsEscapeStart(string text, int index)
            => text[index] == '\\'
            && index + 1 < text.Length
            && _markers.Contains(text[index + 1]);

        private static bool IsHeaderStart(string text, int index)
            => text[index] == HeaderMarkerStart[0]
            && index + 1 < text.Length
            && text[index + 1] == HeaderMarkerStart[1];

        private static bool IsUrlStart(string text, int index)
            => text[index] == LinkNameStart[0];

        private static bool IsBoldStart(string text, int index)
            => IsItalicStart(text, index)
            && index + 1 < text.Length && IsItalicStart(text, index + 1);

        private static bool IsItalicStart(string text, int index)
            => text[index] == ItalicMarkerStart[0];

        private static int ParseEscape(string text, int index, List<IRenderable> tokens)
        {
            tokens.Add(new TextToken(text[index + 1].ToString()));
            return index + 2;
        }

        private static int ParseHeader(string text, int index, List<IRenderable> tokens)
        {
            var startIndex = index + HeaderMarkerStart.Length;
            var endIndex = FindEndOfLine(text, startIndex);
            var innerTokens = ParseSubTokens(text.Substring(startIndex, endIndex - startIndex));
            endIndex += 1;
            tokens.Add(new HeaderToken(WrapInnerTokens(innerTokens)));
            return endIndex;
        }

        private static int FindEndOfLine(string text, int startIndex)
        {
            var endIndex = text.IndexOf('\n', startIndex);
            if (endIndex == -1)
            {
                return text.Length;
            }
            return endIndex;
        }

        private static int ParseLink(string text, int index, List<IRenderable> tokens)
        {
            var nameStart = index + 1;
            var nameEnd = text.IndexOf(LinkNameEnd, nameStart);
            if (nameEnd == -1)
            {
                tokens.Add(new TextToken(LinkNameStart));
                return index + 1;
            }

            var urlStart = text.IndexOf(LinkUrlStart, nameEnd) + 1;
            var urlEnd = text.IndexOf(LinkUrlEnd, urlStart);
            if (urlStart == 0 || urlEnd == -1)
            {
                tokens.Add(new TextToken(LinkNameStart));
                return index + 1;
            }

            var linkName = text.Substring(nameStart, nameEnd - nameStart);
            var linkUrl = text.Substring(urlStart, urlEnd - urlStart);

            var innerTokens = ParseSubTokens(linkName);
            tokens.Add(new LinkToken(WrapInnerTokens(innerTokens), linkUrl));
            return urlEnd + LinkUrlEnd.Length;
        }

        private static int ParseItalic(string text, int index, List<IRenderable> tokens)
        {
            var startIndex = index + 1;

            if (startIndex >= text.Length
                || IsSurroundedByDigits(text, index, 1)
                || char.IsWhiteSpace(text[startIndex]))
            {
                tokens.Add(new TextToken(ItalicMarkerStart));
                return startIndex;
            }

            var endIndex = FindClosingMarker(text,
                startIndex,
                ItalicMarkerStart);
            if (endIndex == -1)
            {
                tokens.Add(new TextToken(ItalicMarkerStart));
                return startIndex;
            }

            var innerTokens = ParseSubTokens(text.Substring(startIndex, endIndex - startIndex));
            CorrectBoldTokensInsideItalicToken(innerTokens);
            tokens.Add(new ItalicToken(WrapInnerTokens(innerTokens)));
            return endIndex + ItalicMarkerStart.Length;
        }

        private static void CorrectBoldTokensInsideItalicToken(IList<IRenderable> innerTokens)
        {
            for (var i = 0; i < innerTokens.Count; i++)
            {
                if (innerTokens[i] is BoldToken boldToken)
                {
                    var innerTextToken = (TextToken)boldToken.InnerItem;
                    innerTokens[i] = new TextToken(
                        $"{BoldMarkerStart}{innerTextToken.Text}{BoldMarkerStart}");
                }
            }
        }

        private static int ParseBold(string text, int index, List<IRenderable> tokens)
        {
            var startIndex = index + BoldMarkerStart.Length;

            if (startIndex >= text.Length
                || IsSurroundedByDigits(text, index, BoldMarkerStart.Length)
                || char.IsWhiteSpace(text[startIndex]))
            {
                tokens.Add(new TextToken(BoldMarkerStart));
                return startIndex;
            }

            var endIndex = FindClosingMarker(text, startIndex, BoldMarkerStart);
            if (endIndex == -1)
            {
                tokens.Add(new TextToken(BoldMarkerStart));
                return startIndex;
            }

            var innerTokens = ParseSubTokens(text.Substring(startIndex, endIndex - startIndex));
            tokens.Add(new BoldToken(WrapInnerTokens(innerTokens)));
            return endIndex + BoldMarkerStart.Length;
        }

        private static bool IsSurroundedByDigits(string text, int index, int markerLength)
        {
            var isLeftDigit = index - 1 >= 0 && char.IsDigit(text[index - 1]);
            var isRightDigit = index + markerLength < text.Length
                && char.IsDigit(text[index + markerLength]);
            return isLeftDigit || isRightDigit;
        }

        private static int ParseText(string text, int index, List<IRenderable> tokens)
        {
            var startIndex = index;

            while (index < text.Length && !_markers.Contains(text[index]))
            {
                if (IsEscapeStart(text, index))
                {
                    if (index + 1 < text.Length && char.IsLetterOrDigit(text[index + 1]))
                    {
                        index += 1;
                        continue;
                    }
                    break;
                }
                index += 1;
            }

            tokens.Add(new TextToken(text.Substring(startIndex, index - startIndex)));
            return index;
        }

        private static IList<IRenderable> ParseSubTokens(string text)
            => ParseTextPart(text);

        private static int CountConsecutiveCharacters(string text, int index, char character)
        {
            var result = 0;
            while (index + result < text.Length && text[index + result] == character)
            {
                result += 1;
            }
            return result;
        }

        private static int FindClosingMarker(string text, int startIndex, string marker)
        {
            for (var i = startIndex; i < text.Length; i++)
            {
                if (char.IsWhiteSpace(text[i]))
                {
                    var nextIndex = FindClosingMarker(text, i + 1, marker);
                    if (IsInsideOfWord(text, nextIndex, marker.Length))
                    {
                        return -1;
                    };

                }

                if (IsEscapeStart(text, i))
                {
                    i += 1;
                    continue;
                }

                if (i + marker.Length > text.Length)
                {
                    return -1;
                }

                var consecutiveCharactersCount = CountConsecutiveCharacters(text, i, marker[0]);
                var sub = text.Substring(i, marker.Length);
                if (sub == marker && consecutiveCharactersCount == marker.Length)
                {
                    var preMarkerIndex = i - 1;
                    if (preMarkerIndex >= startIndex && !char.IsWhiteSpace(text[preMarkerIndex]))
                    {
                        return i;
                    }
                }
                i += consecutiveCharactersCount;
            }

            return -1;
        }

        private static bool IsInsideOfWord(string text, int index, int markerLength)
            => index - markerLength >= 0
            && char.IsLetter(text[index - markerLength])
            && index + markerLength < text.Length
            && char.IsLetter(text[index + markerLength]);

        private static IRenderable WrapInnerTokens(IList<IRenderable> innerTokens)
        {
            if (innerTokens.Count == 1)
            {
                return innerTokens[0];
            }
            return new SetToken(innerTokens.ToArray());
        }
    }
}