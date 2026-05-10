using Chess.Core.Board;

namespace Chess.Core.Pieces.Interfaces;

public interface IPiece
{
    Color Color { get; }
    Position Position { get; set; }

    MoveResult GetAvailableMoves(ChessBoard chessBoard);
    IEnumerable<Position> GetAttackedPositions(ChessBoard chessBoard);
}