using System.Collections.Generic;

namespace Markdown;

public class Token
{
    public TokenType Type { get; set; }
    public string Content { get; set; }
    public List<Token> Children { get; set; } = new List<Token>();
    
    public Token(TokenType type, string content = null)
    {
        Type = type;
        Content = content;
    }
    
}