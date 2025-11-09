using System.Text;

namespace Markdown;

public class EscapeHandler
{
    public bool TryHandle(char symbol, CharReader reader, StringBuilder sb)
    {
        if (symbol != '\\' || reader.CheckEndText())
            return false;

        char next = reader.CheckNextPositions(0);

        if (next == '_' || next == '#' || next == '\\' || next == '[')
        {
            reader.MovePositions();
            sb.Append('\\');
            sb.Append(next);
            return true;
        }

        sb.Append('\\');
        return true;
    }
}