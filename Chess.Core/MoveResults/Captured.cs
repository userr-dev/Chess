namespace Chess.Core.MoveResults;

public sealed class Captured : Result
{
    public Piece CapturedPiece { get; }

    private Captured(Piece movedPiece, Position from, Position to, Piece capturedPiece) 
        : base(movedPiece, from, to)
    {
        CapturedPiece = capturedPiece;
    }

    private Captured(Moved moved, Piece capturedPiece) 
        : this(moved.MovedPiece, moved.From, moved.To, capturedPiece)
    {
    }

    internal static Captured FromMoved(Moved moved, Piece capturedPiece)
    {
        return new Captured(moved, capturedPiece);
    }
    
    public override string ToNotation()
    {
        return $"{MovedPiece.AnnotationSymbol}{From}x{To}{CheckSuffix}";
    }
}