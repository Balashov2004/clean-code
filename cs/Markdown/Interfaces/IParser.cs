namespace Markdown.Interfaces;

public interface IParser
{
    Token Parse(string text);
}