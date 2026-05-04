using Chess.Core.Board;
using Chess.Core.Pieces.Interfaces;

namespace Chess.Core.Pieces;

public static class PieceExtension
{
    extension(IPiece piece)
    {
        internal void FindMovesAndAttacksInLine(ChessBoard chessBoard, PositionMoveDelegate direction,
            in List<Square> moves, in List<Square> attacks)
        {
            var position = piece.Position;
            while (direction(ref position) && !chessBoard[position].HasPiece)
            {
                moves.Add(chessBoard[position]);
            }
            
            if (position != piece.Position && chessBoard[position].HasPiece && !chessBoard[position].HasPieceOfColor(piece.Color)) 
                attacks.Add(chessBoard[position]);
        }
    }
}