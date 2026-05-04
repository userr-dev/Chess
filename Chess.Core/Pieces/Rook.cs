using Chess.Core.Board;
using Chess.Core.Pieces.Interfaces;

namespace Chess.Core.Pieces;

public class Rook : IPiece
{
    public Color Color { get; }
    public Position Position { get; set; }
    
    public bool IsMoveable { get; set; }

    public Rook(Color color, Position position)
    {
        Color = color;
        Position = position;
    }

    public void GetAvailableMoves(ChessBoard chessBoard, out List<Square> possibleMoves,
        out List<Square> possibleAttacks)
    {
        possibleMoves = [];
        possibleAttacks = [];
        
        this.FindMovesAndAttacksInLine(chessBoard, Position.TryMoveUp, possibleMoves, possibleAttacks);
        this.FindMovesAndAttacksInLine(chessBoard, Position.TryMoveDown, possibleMoves, possibleAttacks);
        this.FindMovesAndAttacksInLine(chessBoard, Position.TryMoveLeft, possibleMoves, possibleAttacks);
        this.FindMovesAndAttacksInLine(chessBoard, Position.TryMoveRight, possibleMoves, possibleAttacks);
    }
}