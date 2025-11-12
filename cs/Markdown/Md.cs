
using System;
using System.IO;
using Markdown.Interfaces;

namespace Markdown;
public class Md : IMd
{
    private readonly string input;
    private readonly string output;
    private MarkdownParser parser;
    private Renderer render;
    
    
    public Md(string input, string output)
    {
        this.input = input;
        this.output = output;
        parser = new MarkdownParser();
        render = new Renderer();
        
    }

    public string Start()
    {
        var root = parser.Parse(WorkWithFile.Reader(input));
        var html = render.Render(root);
        
        WorkWithFile.Writer(output, html);
        return html;
    }
    
    
}