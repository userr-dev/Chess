namespace Chess.Core.MoveResults;

public sealed class EnPassantCaptured : Result
{
    public Pawn EnPassantPawn { get; }
    
    private EnPassantCaptured(Pawn movedPiece, Position from, Position to, Pawn enPassantPawn) : base(movedPiece, from, to)
    {
        EnPassantPawn = enPassantPawn;
    }

    internal static EnPassantCaptured Create(Pawn movedPiece, Position from, Position to, Pawn enPassantPawn) =>
        new(movedPiece, from, to, enPassantPawn);
    
    public override string ToNotation()
    {
        return $"{MovedPiece.AnnotationSymbol}{From}x{To}{CheckSuffix}";
    }
}