namespace Chess.Core.MoveResults;

public abstract class PromotedResult(Pawn movedPiece, Position from, Position to, Piece promotionPiece)
    : Result(movedPiece, from, to)
{
    public Piece PromotionPiece { get; } = promotionPiece;
}