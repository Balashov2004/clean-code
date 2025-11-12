namespace Markdown.Interfaces;

public interface IRenderer
{
    string Render(Token root);
}