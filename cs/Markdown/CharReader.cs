using Markdown.Interfaces;

namespace Markdown;

public class CharReader : ICharReader
{
    private readonly string text;
    private int position;
    private readonly int length;

    public CharReader(string text)
    {
        this.text = text;
        length = text.Length;
    }
    
    public int Position => position;

    public bool IsEndOfText(int steps = 0)
    {
        if (position + steps >= length || position + steps < 0)
            return true;
        return false;
    }

    public char GetSymbol()
    {
        if (IsEndOfText())
            return '\0';
        position++;
        return text[position - 1];
    }

    public char CheckNextPositions(int steps = 1)
    {
        if (IsEndOfText(steps))
            return '\0';
        return text[position + steps];
    }

    public void MovePositions(int steps = 1)
    {
        position += steps;
    }
}