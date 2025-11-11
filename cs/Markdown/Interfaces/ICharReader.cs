namespace Markdown.Interfaces;

public interface ICharReader
{
    int Position { get; }
    bool IsEndOfText(int steps = 0);
    char GetSymbol();
    char CheckNextPositions(int steps = 1);
    void MovePositions(int steps = 1);
}