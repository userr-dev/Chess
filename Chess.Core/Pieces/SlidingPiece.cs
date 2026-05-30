using Chess.Core.Board;
using Chess.Core.Pieces.Interfaces;

namespace Chess.Core.Pieces;

public abstract class SlidingPiece : Piece
{
    protected SlidingPiece(Color color, Position position) : base(color, position)
    {
    }

    protected IEnumerable<Position> GetAttackedPositionsAlongDirections(ChessBoard chessBoard,
        IEnumerable<MoveDirection> directions)
    {
        foreach (var direction in directions)
        {
            var position = Position;
            while (direction(ref position))
            {
                yield return position;
                if (chessBoard[position].HasPiece) break;
            }
        }
    }

    protected MoveResult GetMovesAlongDirections(ChessBoard chessBoard, MoveDirection[] directions)
    {
        List<Position> moves = [];
        List<Position> attacks = [];

        var allowedDirections = IsPinned ? directions.Intersect(AllowedDirections!) : directions;
        
        foreach (var position in GetAttackedPositionsAlongDirections(chessBoard, allowedDirections))
        {
            if (!chessBoard[position].HasPiece)
            {
                moves.Add(position);
            }
            else if (chessBoard[position].HasEnemyPiece(Color))
            {
                attacks.Add(position);
            }
        }

        return new MoveResult(moves, attacks);
    }

    protected void FindPinnedPieceAlongDirections(ChessBoard chessBoard, MoveDirection[] directions, King enemyKing)
    {
        foreach (var direction in directions)
        {
            IPiece? candidate = null;
            var position = Position;
            while (direction(ref position))
            {
                var square = chessBoard[position];

                if (square.Piece == enemyKing)
                {
                    candidate?.AllowedDirections = MoveDirection.GetAxisDirections(direction);
                    break;
                }

                if (square.HasFriendlyPiece(Color)) break;
                if (square.HasPiece && candidate is not null) break;
                candidate ??= square.Piece;
            }
        }
    }

    public abstract void FindPinnedPiece(ChessBoard chessBoard, King enemyKing);
}