using Chess.Core.Board;
using Chess.Core.Pieces.Interfaces;

namespace Chess.Core.Pieces;

public class King : Piece, ICastlingPiece
{
    private static readonly MoveDirection[] Directions =
    [
        Position.TryMoveLeftUp, Position.TryMoveLeftDown, Position.TryMoveRightUp, Position.TryMoveRightDown,
        Position.TryMoveUp, Position.TryMoveDown, Position.TryMoveLeft, Position.TryMoveRight
    ];

    public bool CanCastle { get; private set; } = true;
    
    public King(Color color, Position position) : base(color, position)
    {
    }

    public override void Move(ChessBoard chessBoard, Position to)
    {
        CanCastle = false;
        
        base.Move(chessBoard, to);
    }

    public override MoveResult GetAvailableMoves(ChessBoard chessBoard)
    {
        List<Position> moves = [];
        List<Position> attacks = [];
        
        var enemyAttacks = chessBoard.GetPieces(Color == Color.Light ? Color.Dark : Color.Light)
            .SelectMany(p => p.GetAttackedPositions(chessBoard))
            .ToHashSet();

        foreach (var position in GetAttackedPositions(chessBoard))
        {
            var canMoved = !enemyAttacks.Contains(position);
            
            if (!chessBoard[position].HasPiece && canMoved)
            {
                moves.Add(position);
            }
            else if (!chessBoard[position].HasPieceOfColor(Color) && canMoved)
            {
                attacks.Add(position);
            }
        }

        return new MoveResult(moves, attacks);
    }

    public override IEnumerable<Position> GetAttackedPositions(ChessBoard chessBoard)
    {
        foreach (var direction in Directions)
        {
            var position = Position;
            if (!direction(ref position)) continue;
            yield return position;
        }
    }
}