namespace Chess.Core.MoveResults;

public sealed class CapturedAndPromoted : PromotedResult
{
    public Piece CapturedPiece { get; }
    
    private CapturedAndPromoted(Pawn movedPawn, Position from, Position to, Piece capturedPiece, Piece promotionPiece) : base(movedPawn, from, to, promotionPiece)
    {
        CapturedPiece = capturedPiece;
    }

    private CapturedAndPromoted(Captured pawnCaptured, Piece promotionPiece) 
        : this((Pawn)pawnCaptured.MovedPiece, pawnCaptured.From, pawnCaptured.To, pawnCaptured.CapturedPiece, promotionPiece)
    {
        
    }


    internal static CapturedAndPromoted FromCaptured(Captured pawnCaptured, Piece promotionPiece) =>
        new(pawnCaptured, promotionPiece);
    
    public override string ToNotation()
    {
        return $"{From}x{To}={PromotionPiece.AnnotationSymbol}{CheckSuffix}";
    }
}