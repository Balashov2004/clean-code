
using System.Collections.Generic;
using System.Text;

namespace Markdown;

public class MarkdownParser
{
    public Token Parse(string text)
    {
        var reader = new CharReader(text);
        return ParseTokens(reader);
    }

    public Token ParseTokens(CharReader reader)
    {
        var root = new Token(TokenType.Text);
        var sb = new StringBuilder();
        var stack = new Stack<Token>();
        var headerFlag = false;
        stack.Push(root);

        while (!reader.CheckEndText())
        {
            var symbol = reader.GetSymbol();
            
            if (symbol == '\\' && !reader.CheckEndText())
            {
                char next = reader.CheckNextPositions(0);
                
                
                if (next == '_' || next == '#' || next == '\\')
                {
                    reader.MovePositions();
                    sb.Append('\\' + next.ToString());
                    continue;
                }
                
                sb.Append('\\');
                continue;
            }
            
            //заголовок
            if (symbol == '#' && (reader.Position == 1 || reader.CheckNextPositions(-1) == '\n'))
            {
                FlushText(stack.Peek(), sb);
                var header = new Token(TokenType.Header);
                stack.Peek().Children.Add(header);
                stack.Push(header);
                headerFlag = true;
                
                continue;
            }
            // жирный
            else if (symbol == '_' && reader.CheckNextPositions(0) == '_')
            {
                FlushText(stack.Peek(), sb);
                reader.MovePositions(1);
                
                if (stack.Peek().Type == TokenType.Bold)
                {
                    stack.Pop();
                }
                else
                {
                    var bold = new Token(TokenType.Bold);
                    stack.Peek().Children.Add(bold);
                    stack.Push(bold);
                }
                continue;
            }
            //курсив
            else if (symbol == '_')
            {
                FlushText(stack.Peek(), sb);
                if (stack.Peek().Type == TokenType.Italic)
                {
                    stack.Pop();
                }
                else
                {
                    var italic = new Token(TokenType.Italic);
                    stack.Peek().Children.Add(italic);
                    stack.Push(italic);
                }
                continue;
            }

            if (symbol == '\n')
            {
                FlushText(stack.Peek(), sb);
                
                if (headerFlag && stack.Peek().Type == TokenType.Header)
                {
                    stack.Pop();
                    headerFlag = false;
                }
                stack.Peek().Children.Add(new Token(TokenType.Text, "\n"));
                continue;
            }
            
            sb.Append(symbol);
            
        }
        FlushText(stack.Peek(), sb);
        
        return root;
    }

    private void FlushText(Token parent, StringBuilder sb)
    {
        if (sb.Length > 0)
        {
            parent.Children.Add(new Token(TokenType.Text, sb.ToString()));
            sb.Clear();
        }
    }
}
