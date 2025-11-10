
using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown;

public class MarkdownParser
{
    
    private readonly EscapeHandler escapeHandler = new();
    private readonly SymbolRules rules = new();
    private readonly TokenBuilder tokenBuilder = new();
    
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
        // var crossFlag = false;
        // var index = 0;
        stack.Push(root);

        while (!reader.CheckEndText())
        {
            var symbol = reader.GetSymbol();
            
            // Экранирование
            if (escapeHandler.TryHandle(symbol, reader, sb))
                continue;
            
            // Заголовок
            if (rules.IsHeader(symbol, reader))
            {
                tokenBuilder.OpenHeader(stack, sb);
                headerFlag = true;
                continue;
            }
            
            // жирный
            else if (rules.IsBold(symbol, reader))
            {
                if (rules.InItalic(stack))
                {
                    sb.Append("\\_\\_");
                    reader.MovePositions(1);
                    continue;
                }
                if ((rules.NextSpace(reader, 1) &&  !rules.InBold(stack)) ||
                     (rules.NextSpace(reader, - 2) && rules.InBold(stack)))
                {
                    sb.Append("\\_\\_");
                    reader.MovePositions(1);
                    continue;
                }
                if (rules.EmptyLine(reader, sb))
                    continue;
                
                
                tokenBuilder.SwitchBold(stack, sb);
                reader.MovePositions(1);
                
                continue;
            }
            
            // Курсив
            else if (rules.IsItalic(symbol, reader))
            {
                if ((rules.NextSpace(reader) && !rules.InItalic(stack)) || 
                    (rules.NextSpace(reader, - 2) && rules.InItalic(stack)))
                {
                    sb.Append("\\_");
                    continue;
                }

                // if (rules.InBold(stack))
                // {
                //     crossFlag = true;
                //     index = sb.
                // }
                    
                
                tokenBuilder.SwitchItalic(stack, sb);
                continue;
            }
            
            // Ссылка
            else if (symbol == '[')
            {
                tokenBuilder.Link(stack, sb, reader);
                continue;
            }

            // Перенос
            if (symbol == '\n')
            {
                tokenBuilder.NewLine(stack, sb, ref headerFlag);
                continue;
            }
            
            sb.Append(symbol);
            
        }
        tokenBuilder.FlushText(stack.Peek(), sb);
        return root;
    }
}
