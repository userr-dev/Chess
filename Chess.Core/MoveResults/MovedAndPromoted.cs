namespace Chess.Core.MoveResults;

public sealed class MovedAndPromoted : PromotedResult
{
    private MovedAndPromoted(Pawn movedPawn, Position from, Position to, Piece promotionPiece) : base(movedPawn, from, to, promotionPiece)
    {
    }

    private MovedAndPromoted(Moved pawnMoved, Piece promotionPiece) 
        : this((Pawn)pawnMoved.MovedPiece, pawnMoved.From, pawnMoved.To, promotionPiece)
    {
    }

    internal static MovedAndPromoted FromMoved(Moved pawnMoved, Piece promotionPiece)
    {
        return new MovedAndPromoted(pawnMoved, promotionPiece);
    }
    
    public override string ToNotation()
    {
        return $"{From}{To}={PromotionPiece.AnnotationSymbol}{CheckSuffix}";
    }
}