
using System;

namespace Markdown;

public class Md
{
    public Md(string text)
    {
        var parser = new MarkdownParser();
        Token root = parser.Parse(text);
        PrintToken(root);
    }
    
    static void PrintToken(Token token, string indent = "")
    {
        Console.WriteLine($"{indent}{token.Type}: {(token.Content ?? "")}");
        
        foreach (var child in token.Children)
        {
            PrintToken(child, indent + "  ");
        }
    }
    
}