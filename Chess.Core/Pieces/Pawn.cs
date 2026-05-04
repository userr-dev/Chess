using Chess.Core.Board;
using Chess.Core.Pieces.Interfaces;

namespace Chess.Core.Pieces;

public class Pawn : IPiece
{
    public Color Color { get; }
    public Position Position { get; set; }

    public bool IsFirstMove { get; set; } = true; // private set

    public Pawn(Color color, Position position)
    {
        Color = color;
        Position = position;
    }
    
    public void GetAvailableMoves(ChessBoard chessBoard, out List<Square> possibleMoves, out List<Square> possibleAttacks)
    {
        possibleMoves = [..FindMoves(chessBoard)];
        possibleAttacks = [..FindAttacks(chessBoard)];
    }
    
    private IEnumerable<Square> FindMoves(ChessBoard chessBoard)
    {
        PositionMoveDelegate directionMove = Color == Color.Light ? Position.TryMoveUp : Position.TryMoveDown;
        var position = Position;
        
        if (directionMove(ref position) && !chessBoard[position].HasPiece)
        {
            yield return chessBoard[position];
            if (IsFirstMove && directionMove(ref position) && !chessBoard[position].HasPiece)
                yield return chessBoard[position];
        }
    }

    private IEnumerable<Square> FindAttacks(ChessBoard chessBoard)
    {
        var direction = Color == Color.Light ? 1 : -1;
        int[] columnOffsets = [-1, 1];
        
        foreach (var columnOffset in columnOffsets)
        {
            var position = Position;
            if (Position.TryMove(ref position, columnOffset, direction) 
                && chessBoard[position].HasPiece 
                && !chessBoard[position].HasPieceOfColor(Color))
            {
                yield return chessBoard[position];
            }
        }
    }
}