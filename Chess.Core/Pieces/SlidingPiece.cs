using Chess.Core.Board;

namespace Chess.Core.Pieces;

public abstract class SlidingPiece : Piece
{
    protected SlidingPiece(Color color, Position position) : base(color, position)
    {
    }

    protected IEnumerable<Position> GetAttackedPositionsAlongDirections(ChessBoard chessBoard,
        MoveDirection[] directions)
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

        foreach (var position in GetAttackedPositionsAlongDirections(chessBoard, directions))
        {
            if (!chessBoard[position].HasPiece)
            {
                moves.Add(position);
            }
            else if (!chessBoard[position].HasPieceOfColor(Color))
            {
                attacks.Add(position);
            }
        }

        return new MoveResult(moves, attacks);
    }
}