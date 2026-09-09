namespace Chess.Core.MoveResults;

public sealed class Moved : Result
{
    private Moved(Piece movedPiece, Position from, Position to) : base(movedPiece, from, to)
    {
    }

    public override string ToNotation()
    {
        return $"{MovedPiece.AnnotationSymbol}{From}{To}{CheckSuffix}";
    }

    internal static Moved Create(Piece movedPiece, Position from, Position to) => new(movedPiece, from, to);
}