using System.Linq;
using System.Text;

namespace Markdown;

public class MarkdownParser
{
    public Token Parse(string text)
    {
        var reader = new CharReader(text);
        return ParseTokens(reader, headerFlag: false, inSingle: false, inDouble: false);
    }

    public Token ParseTokens(CharReader reader, bool headerFlag, bool inSingle, bool inDouble)
    {
        var root = new Token(TokenType.Text);
        var sb = new StringBuilder();

        while (!reader.CheckEndText())
        {
            var symbol = reader.GetSymbol();

            if (symbol == '#' && (reader.Position == 1 || reader.CheckNextPositions(-1) == '\n'))
            {
                if (sb.Length > 0)
                {
                    root.Children.Add(new Token(TokenType.Text, sb.ToString()));
                    sb.Clear();
                }

                reader.MovePositions(1);
                var headerToken = new Token(TokenType.Header);
                var headerParseNext = ParseTokens(reader, headerFlag: true, inSingle, inDouble);
                headerToken.Children.AddRange(headerParseNext.Children);
                root.Children.Add(headerToken);

                if (!reader.CheckEndText() && reader.GetSymbol() == '\n')
                {
                    root.Children.Add(new Token(TokenType.Text, "\n"));
                }
                continue;
            }

            if (headerFlag && symbol == '\n')
            {
                break;
            }

            if (symbol == '_' && reader.CheckNextPositions() == '_')
            {
                if (inSingle)
                {
                    sb.Append("__");
                    reader.MovePositions(2);
                    continue;
                }

                if (inDouble)
                {
                    if (HasNonSpaceBefore(root, sb))
                    {
                        reader.MovePositions(2);
                        return root;
                    }
                    else
                    {
                        sb.Append("__");
                        reader.MovePositions(2);
                        continue;
                    }
                }

                if (sb.Length > 0)
                {
                    root.Children.Add(new Token(TokenType.Text, sb.ToString()));
                    sb.Clear();
                }

                reader.MovePositions(2);
                var boldToken = new Token(TokenType.Bold);
                var innerBold = ParseTokens(reader, headerFlag: false, inSingle, inDouble: true);

                if (IsEmptyContent(innerBold))
                {
                    string innerText = CollectText(innerBold);
                    root.Children.Add(new Token(TokenType.Text, "__" + innerText + "__"));
                }
                else
                {
                    boldToken.Children.AddRange(innerBold.Children);
                    root.Children.Add(boldToken);
                }

                continue;
            }

            if (symbol == '_')
            {
                char prev = GetPrevChar(root, sb);
                char next = reader.CheckNextPositions();
                if (char.IsDigit(prev) && char.IsDigit(next))
                {
                    sb.Append('_');
                    continue;
                }

                if (inSingle)
                {
                    if (HasNonSpaceBefore(root, sb))
                    {
                        return root;
                    }
                    else
                    {
                        sb.Append('_');
                        continue;
                    }
                }

                if (sb.Length > 0)
                {
                    root.Children.Add(new Token(TokenType.Text, sb.ToString()));
                    sb.Clear();
                }

                var italicToken = new Token(TokenType.Italic);
                var innerItalic = ParseTokens(reader, headerFlag: false, inSingle: true, inDouble);

                if (IsEmptyContent(innerItalic))
                {
                    string innerText = CollectText(innerItalic);
                    root.Children.Add(new Token(TokenType.Text, "_" + innerText + "_"));
                }
                else
                {
                    italicToken.Children.AddRange(innerItalic.Children);
                    root.Children.Add(italicToken);
                }

                continue;
            }

            sb.Append(symbol);
        }

        if (sb.Length > 0)
        {
            root.Children.Add(new Token(TokenType.Text, sb.ToString()));
        }

        return root;
    }

    private bool IsEmptyContent(Token t)
    {
        if (t == null) return true;
        if (t.Children == null || t.Children.Count == 0) return true;
        if (t.Children.All(ch => ch.Type == TokenType.Text && string.IsNullOrEmpty(ch.Content))) return true;
        return false;
    }

    private string CollectText(Token t)
    {
        if (t == null) return string.Empty;
        var sb = new StringBuilder();
        foreach (var ch in t.Children)
        {
            if (ch.Type == TokenType.Text)
                sb.Append(ch.Content);
            else
                sb.Append(CollectText(ch));
        }
        return sb.ToString();
    }

    private char GetPrevChar(Token root, StringBuilder sb)
    {
        if (sb.Length > 0) return sb[sb.Length - 1];
        if (root.Children.Count == 0) return '\0';
        var last = root.Children.Last();
        if (last.Type == TokenType.Text && !string.IsNullOrEmpty(last.Content))
            return last.Content[last.Content.Length - 1];
        return FindLastCharInToken(last);
    }

    private char FindLastCharInToken(Token token)
    {
        if (token == null) return '\0';
        if (token.Type == TokenType.Text && !string.IsNullOrEmpty(token.Content))
            return token.Content[token.Content.Length - 1];
        if (token.Children == null || token.Children.Count == 0) return '\0';
        return FindLastCharInToken(token.Children.Last());
    }

    private bool HasNonSpaceBefore(Token root, StringBuilder sb)
    {
        char prev = GetPrevChar(root, sb);
        return prev != '\0' && !char.IsWhiteSpace(prev);
    }
}
