
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

    public string Render()
    {
        Token root = parser.Parse(WorkWithFile(input, mode: "r"));
        string html = render.RenderToHtml(root);
        
        // WorkWithFile(output, html, "w");
        return html;
    }

    private string WorkWithFile(string path, string content = "null", string mode = "read")
    {
        if (mode == "r")
            return File.ReadAllText(path);
        else if (mode == "w")
            File.WriteAllText(path, content);

        return null;
    }
    
}