namespace Markdown;

public class CharReader
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

    public bool CheckEndText(int steps = 0)
    {
        if (position + steps >= length)
            return true;
        return false;
    }

    public char GetSymbol()
    {
        if (CheckEndText())
            return '\0';
        position++;
        return text[position - 1];
    }

    public char CheckNextPositions(int steps = 1)
    {
        if (CheckEndText(steps))
            return '\0';
        return text[position + steps];
    }

    public void MovePositions(int steps = 1)
    {
        position += steps;
    }
}