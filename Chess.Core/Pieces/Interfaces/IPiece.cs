using Chess.Core.Board;

namespace Chess.Core.Pieces.Interfaces;

public interface IPiece
{
    Color Color { get; }
    Position Position { get; set; }

    void GetAvailableMoves(ChessBoard chessBoard, out List<Square> possibleMoves, out List<Square> possibleAttacks);
}