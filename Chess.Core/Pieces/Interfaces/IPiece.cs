using Chess.Core.Board;

namespace Chess.Core.Pieces.Interfaces;

public interface IPiece
{
    Color Color { get; }
    Position Position { get; }
    
    MoveDirection[]?  AllowedDirections { get; set; }
    bool IsPinned { get; }
    
    MoveResult GetAvailableMoves(ChessBoard chessBoard);
    IEnumerable<Position> GetAttackedPositions(ChessBoard chessBoard);

    void Move(ChessBoard chessBoard, Position to);
}