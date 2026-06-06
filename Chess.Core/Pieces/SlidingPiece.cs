using Chess.Core.Board;

namespace Chess.Core.Pieces;

public abstract class SlidingPiece : Piece
{
    protected SlidingPiece(Color color, Position position) : base(color, position)
    {
    }

    protected IEnumerable<Position> GetAttackedPositionsAlongDirections(ChessBoard chessBoard,
        IEnumerable<MoveDirection> directions)
    {
        var enemyKing = chessBoard.GetPieces(Color.Opposite()).OfType<King>().First();
        
        foreach (var direction in directions)
        {
            var position = Position;
            while (direction(ref position))
            {
                yield return position;
                if (chessBoard[position].HasPiece && chessBoard[position].Piece == enemyKing) continue;
                if (chessBoard[position].HasPiece) break;
            }
        }
    }

    protected MoveResult GetMovesAlongDirections(ChessBoard chessBoard, MoveDirection[] directions)
    {
        List<Position> moves = [];
        List<Position> attacks = [];

        var checkState = chessBoard.GetCheckState(Color);

        if (checkState.IsDoubleChecked) return new MoveResult(moves, attacks);
        
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

        return ApplyCheckFilter(checkState, moves, attacks);
    }

    protected void FindPinnedPieceAlongDirections(ChessBoard chessBoard, MoveDirection[] directions, King enemyKing)
    {
        foreach (var direction in directions)
        {
            Piece? candidate = null;
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