using System.Collections.Generic;

namespace Markdown;

public class SymbolRules
{
    public bool IsHeader(char symbol, CharReader reader)
        => symbol == '#' && (reader.CheckNextPositions(-1) == '\n' || reader.Position == 1);

    public bool IsBold(char symbol, CharReader reader)
        => symbol == '_' && reader.CheckNextPositions(0) == '_';

    public bool InItalic(Stack<Token> stack)
        => stack.Peek().Type == TokenType.Italic;

    public bool InBold(Stack<Token> stack)
        => stack.Peek().Type == TokenType.Bold;
    
    public bool NextSpace(CharReader reader, int i = 0)
        => char.IsWhiteSpace(reader.CheckNextPositions(i));

    public bool IsItalic(char symbol, CharReader reader)
    {
        if (symbol != '_')
            return false;

        char prev = reader.Position > 1 ? reader.CheckNextPositions(-2) : '\0';
        char next = !reader.CheckEndText() ? reader.CheckNextPositions() : '\0';
        
        if (char.IsDigit(prev) || char.IsDigit(next))
            return false;

        return true;
    }
    
    
    
}