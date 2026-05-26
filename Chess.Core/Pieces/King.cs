using Chess.Core.Board;
using Chess.Core.Pieces.Interfaces;

namespace Chess.Core.Pieces;

public sealed class King : Piece, ICastlingPiece
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
        var position = Position;
        
        base.Move(chessBoard, to);

        var columnOffset = to.Column.DistanceTo(position.Column);
        if (columnOffset != 2) return;
         
        var rook = chessBoard.GetPieces(Color).OfType<Rook>().First(r =>
            r.CanCastle && (to.Column < position.Column
                ? r.Position.Column < position.Column
                : r.Position.Column > position.Column));
        
        chessBoard.Castling(this, rook);
    }

    public override MoveResult GetAvailableMoves(ChessBoard chessBoard)
    {
        List<Position> moves = [];
        List<Position> attacks = [];
        
        var enemyAttacks = chessBoard.GetPieces(Color.Opposite())
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

        if (CanCastle && !enemyAttacks.Contains(Position))
        {
            var castlingMoves = GetCastlingMove(chessBoard, enemyAttacks);
            moves.AddRange(castlingMoves);
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

    private IEnumerable<Position> GetCastlingMove(ChessBoard chessBoard, HashSet<Position> enemyAttackedPositions)
    {
        var rooks = chessBoard.GetPieces(Color).OfType<Rook>().Where(r => r.CanCastle);
        
        foreach (var rook in rooks)
        {
            MoveDirection direction = rook.Position.Column > Position.Column ? Position.TryMoveRight : Position.TryMoveLeft;
            if (!CheckPossibleCastling(chessBoard, rook.Position, enemyAttackedPositions, direction)) continue;
            
            var columnOffset = rook.Position.Column > Position.Column ? 2 : -2;
            yield return Position.Create(Position.Column.Shift(columnOffset), Position.Row);
        }
    }

    private bool CheckPossibleCastling(ChessBoard chessBoard, Position rookPosition,
        HashSet<Position> enemyAttackedPositions, MoveDirection direction)
    {
        var position = Position;
        while (direction(ref position) && position != rookPosition)
        {
            if (chessBoard[position].HasPiece || enemyAttackedPositions.Contains(position))
            {
                return false;
            }
        }

        return true;
    }
}