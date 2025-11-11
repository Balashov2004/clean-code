using System.Collections.Generic;
using System.Text;

namespace Markdown;

public class SymbolRules
{
    public bool IsHeader(char symbol, CharReader reader)
        => symbol == '#' && (reader.CheckNextPositions(-2) == '\n' || reader.Position == 1);

    public bool IsBold(char symbol, CharReader reader)
        => symbol == '_' && reader.CheckNextPositions(0) == '_';

    public bool InItalic(Stack<Token> stack)
        => stack.Peek().Type == TokenType.Italic;

    public bool InBold(Stack<Token> stack)
        => stack.Peek().Type == TokenType.Bold;
    
    public bool IsNextCharWhitespace(CharReader reader, int i = 0)
        => char.IsWhiteSpace(reader.CheckNextPositions(i));

    public bool EmptyLine(CharReader reader, StringBuilder sb)
    {
        if (reader.CheckNextPositions(1) == '_')
        {
            sb.Append("\\_\\_");
            reader.MovePositions();
            while (reader.CheckNextPositions(0) == '_')
            {
                reader.MovePositions();
                sb.Append("\\_");
            }
            
            return true;
        }

        return false;
    }

    public bool IsItalic(char symbol, CharReader reader)
    {
        if (symbol != '_')
            return false;

        char prev = reader.Position > 1 ? reader.CheckNextPositions(-2) : '\0';
        char next = !reader.IsEndOfText() ? reader.CheckNextPositions() : '\0';
        
        if (char.IsDigit(prev) || char.IsDigit(next))
            return false;

        return true;
    }

    public bool IsPrevCharWhiteLetter(CharReader reader)
        => char.IsLetter(reader.CheckNextPositions(-2));

    public void InWorld(Stack<Token> stack, StringBuilder sb, CharReader reader)
    {
        var word = new StringBuilder();
        word.Append(reader.CheckNextPositions(0));
        while (!reader.IsEndOfText())
        {
            var next = reader.CheckNextPositions();
            if (next == ' ' || reader.IsEndOfText(1))
            {
                reader.MovePositions();
                sb.Append("\\_" + word);
                break;
            }
            else if (next == '_')
            {
                reader.MovePositions(2);
                var tokenBuilder = new TokenBuilder();
                var italic = new Token(TokenType.Italic);
                tokenBuilder.FlushText(stack.Peek(), sb);
                stack.Peek().Children.Add(italic);
                stack.Push(italic);
                tokenBuilder.FlushText(italic, word);
                stack.Pop();
                break;

            }
            word.Append(next);
            reader.MovePositions();
        }
    }
    

}