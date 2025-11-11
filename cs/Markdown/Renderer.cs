
using System.Text;
using Markdown.Interfaces;

namespace Markdown;

public class Renderer :  IRenderer
{
    public string Render(Token root)
    {
        var html = new StringBuilder();
        html.Append(RenderToHtml(root));
        return html.ToString();
    }

    private string RenderToHtml(Token token)
    {
        var sb = new StringBuilder();

        switch (token.Type)
        {
            case TokenType.Text:
                if (!string.IsNullOrEmpty(token.Content))
                {
                    sb.Append(token.Content);
                }
                else if (token.Children.Count > 0)
                {
                    sb.Append(RenderChildren(token));
                }
                break;

            case TokenType.Header:
                sb.Append("<h1>");
                sb.Append(RenderChildren(token));
                sb.Append("</h1>\n");
                break;

            case TokenType.Bold:
                sb.Append("<strong>");
                sb.Append(RenderChildren(token));
                sb.Append("</strong>");
                break;

            case TokenType.Italic:
                sb.Append("<em>");
                sb.Append(RenderChildren(token));
                sb.Append("</em>");
                break;
            
            case TokenType.Link:
                sb.Append($"<a href=\"{token.Value}\">{token.Children[0].Content}</a>");
                break;

            default:
                sb.Append(RenderChildren(token));
                break;
        }
        return sb.ToString();
    }
    private string RenderChildren(Token token)
    {
        var sb = new StringBuilder();
        foreach (var child in token.Children)
            sb.Append(RenderToHtml(child));
        return sb.ToString();
    }

    
    
}