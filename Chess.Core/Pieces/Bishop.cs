using Chess.Core.Board;
using Chess.Core.Pieces.Interfaces;

namespace Chess.Core.Pieces;

public class Bishop : IPiece
{
    public Color Color { get; }
    public Position Position { get; set; }

    public Bishop(Color color, Position position)
    {
        Color = color;
        Position = position;
    }
    
    public void GetAvailableMoves(ChessBoard chessBoard, out List<Square> possibleMoves,
        out List<Square> possibleAttacks)
    {
        possibleMoves = [];
        possibleAttacks = [];
        this.FindMovesAndAttacksInLine(chessBoard, Position.TryMoveLeftUp, possibleMoves, possibleAttacks);
        this.FindMovesAndAttacksInLine(chessBoard, Position.TryMoveLeftDown, possibleMoves, possibleAttacks);
        this.FindMovesAndAttacksInLine(chessBoard, Position.TryMoveRightUp, possibleMoves, possibleAttacks);
        this.FindMovesAndAttacksInLine(chessBoard, Position.TryMoveRightDown, possibleMoves, possibleAttacks);
    }
}