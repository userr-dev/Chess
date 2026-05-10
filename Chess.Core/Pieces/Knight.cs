using Chess.Core.Board;
using KnightMoveOffset = (int Column, int Row);

namespace Chess.Core.Pieces;

public sealed class Knight : Piece
{
    private static readonly KnightMoveOffset[] MoveOffsets =
        [(-1, 2), (1, 2), (-1, -2), (1, -2), (-2, 1), (-2, -1), (2, 1), (2, -1)];
    
    public Knight(Color color, Position position) : base(color, position)
    {
    }
    
    public override MoveResult GetAvailableMoves(ChessBoard chessBoard)
    {
        List<Position> moves = [];
        List<Position> attacks = [];
        
        foreach (var position in GetAttackedPositions(chessBoard))
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

    public override IEnumerable<Position> GetAttackedPositions(ChessBoard chessBoard)
    {
        foreach (var offset in MoveOffsets)
        {
            var position = Position;
            if (Position.TryMove(ref position, offset.Column, offset.Row))
            {
                yield return position;
            }
        }
    }
}