namespace Chess.Core.MoveResults;

public abstract class Result(Piece movedPiece, Position from, Position to)
{
    public Piece MovedPiece { get; } = movedPiece;
    public Position From { get; } = from;
    public Position To { get; } = to;

    public char? CheckSuffix { get; private set; }
    
    internal void AppendCheckSuffix(char? suffix)
    {
        CheckSuffix = suffix;
    }
    
    public abstract string ToNotation();
    
    public override string ToString()
    {
        return ToNotation();
    }
}