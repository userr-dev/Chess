using System.Diagnostics;

namespace Chess.Core.MoveResults;

internal static class ResultsExtensions
{
    extension(Result result)
    {
        internal PromotedResult ToPromoted(Piece promotionPiece)
        {
            if (result.MovedPiece is not Pawn) throw new ArgumentException("Not pawn move result");
            
            return result switch
            {
                Moved moved => MovedAndPromoted.FromMoved(moved, promotionPiece),
                Captured captured => CapturedAndPromoted.FromCaptured(captured, promotionPiece),
                _ => throw new UnreachableException($"Cannot promote a {result.GetType().Name} result.")
            };
        }
    }
}