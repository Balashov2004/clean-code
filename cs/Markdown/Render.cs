
using System.Text;

namespace Markdown;

public class Render
{
    public string Start(Token root)
    {
        var html = new StringBuilder();
        html.Append(RenderToken(root));
        return html.ToString();
    }

    private string RenderToken(Token token)
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
                    foreach (var child in token.Children)
                        sb.Append(RenderToken(child));
                }
                break;

            case TokenType.Header:
                sb.Append("<h1>");
                foreach (var child in token.Children)
                    sb.Append(RenderToken(child));
                sb.Append("</h1>\n");
                break;

            case TokenType.Bold:
                sb.Append("<strong>");
                foreach (var child in token.Children)
                    sb.Append(RenderToken(child));
                sb.Append("</strong>");
                break;

            case TokenType.Italic:
                sb.Append("<em>");
                foreach (var child in token.Children)
                    sb.Append(RenderToken(child));
                sb.Append("</em>");
                break;

            default:
                foreach (var child in token.Children)
                    sb.Append(RenderToken(child));
                break;
        }
        
        return sb.ToString();
    }

    
    
}