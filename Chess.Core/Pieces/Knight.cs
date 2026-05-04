using Chess.Core.Board;
using Chess.Core.Pieces.Interfaces;
using KnightMoveOffset = (int Column, int Row);

namespace Chess.Core.Pieces;

public class Knight : IPiece
{
    private static readonly KnightMoveOffset[] MovesOffsets = [(-1,2),(1,2),(-1,-2),(1,-2),(-2,1),(-2,-1),(2,1),(2,-1)];
    
    public Color Color { get; }
    public Position Position { get; set; }

    public Knight(Color color, Position position)
    {
        Color = color;
        Position = position;
    }
    
    public void GetAvailableMoves(ChessBoard chessBoard, out List<Square> possibleMoves,
        out List<Square> possibleAttacks)
    {
        possibleMoves = [];
        possibleAttacks = [];
        FindMovesAndAttacks(chessBoard, possibleMoves, possibleAttacks);
    }

    private void FindMovesAndAttacks(ChessBoard chessBoard, List<Square> moves, List<Square> attacks)
    {
        foreach (var offset in MovesOffsets)
        {
            var position = Position;
            if (Position.TryMove(ref position, offset.Column, offset.Row) && !chessBoard[position].HasPiece) 
                moves.Add(chessBoard[position]);
            
            if (position != Position && chessBoard[position].HasPiece && !chessBoard[position].HasPieceOfColor(Color))
                attacks.Add(chessBoard[position]);
        }
    }
}