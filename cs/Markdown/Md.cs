
using System;
using System.IO;

namespace Markdown;

public class Md
{
    private string input;
    private string output;
    private MarkdownParser parser;
    private Render render;
    
    
    public Md(string input, string output)
    {
        this.input = input;
        this.output = output;
        parser = new MarkdownParser();
        render = new Render();
        
    }

    public void Render()
    {
        Token root = parser.Parse(WorkWithFile(input, mode: "read"));
        string html = render.Start(root);
        
        WorkWithFile(output, html, html);
    }

    private string WorkWithFile(string path, string content = "null", string mode = "read")
    {
        if (mode == "read")
            return File.ReadAllText(path);
        else if (mode == "write")
            File.WriteAllText(path, content);
        return null;
    }
    
}