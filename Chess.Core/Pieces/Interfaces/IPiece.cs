using Chess.Core.Board;

namespace Chess.Core.Pieces.Interfaces;

public interface IPiece
{
    int Id { get; }
    Color Color { get; }
    Position Position { get; set; }
}