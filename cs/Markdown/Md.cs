
using System;

namespace Markdown;

public class Md
{
    public Md(string text)
    {
        var parser = new MarkdownParser();
        Token root = parser.Parse(text);
        PrintToken(root, 0);
        
        var render = new Render();
        string html = render.Start(root);
        File.WriteAllText("resurses/output.md", html);
        // Console.WriteLine(html);
    }
    
    static void PrintToken(Token token, int indent)
    {
        Console.WriteLine(new string(' ', indent * 2) + token.Type + 
                          (token.Content != null ? $" \"{token.Content}\"" : ""));
        foreach (var child in token.Children)
            PrintToken(child, indent + 1);
    }
    
}