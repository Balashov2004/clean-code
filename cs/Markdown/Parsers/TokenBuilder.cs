using System.Collections.Generic;
using System.Text;

namespace Markdown;

public class TokenBuilder
{
    public void OpenHeader(Stack<Token> stack, StringBuilder sb)
    {
        FlushText(stack.Peek(), sb);
        var header = new Token(TokenType.Header);
        stack.Peek().Children.Add(header);
        stack.Push(header);
    }
    
    public void SwitchBold(Stack<Token> stack, StringBuilder sb)
    {
        FlushText(stack.Peek(), sb);

        if (stack.Peek().Type == TokenType.Bold)
            stack.Pop();
        else
        {
            var bold = new Token(TokenType.Bold);
            stack.Peek().Children.Add(bold);
            stack.Push(bold);
        }
    }
    
    public void SwitchItalic(Stack<Token> stack, StringBuilder sb)
    {
        FlushText(stack.Peek(), sb);

        if (stack.Peek().Type == TokenType.Italic)
            stack.Pop();
        else
        {
            var italic = new Token(TokenType.Italic);
            stack.Peek().Children.Add(italic);
            stack.Push(italic);
        }
    }
    
    public void NewLine(Stack<Token> stack, StringBuilder sb, ref bool headerFlag)
    {
        FlushText(stack.Peek(), sb);

        if (headerFlag && stack.Peek().Type == TokenType.Header)
        {
            stack.Pop();
            headerFlag = false;
        }

        stack.Peek().Children.Add(new Token(TokenType.Text, "\n"));
    }
    
    public void FlushText(Token parent, StringBuilder sb)
    {
        if (sb.Length > 0)
        {
            parent.Children.Add(new Token(TokenType.Text, sb.ToString()));
            sb.Clear();
        }
    }
}