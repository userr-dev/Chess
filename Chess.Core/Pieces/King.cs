using Chess.Core.Board;

namespace Chess.Core.Pieces;

public sealed class King : Piece
{
    private static readonly MoveDirection[] Directions =
    [
        Position.TryMoveLeftUp, Position.TryMoveLeftDown, Position.TryMoveRightUp, Position.TryMoveRightDown,
        Position.TryMoveUp, Position.TryMoveDown, Position.TryMoveLeft, Position.TryMoveRight
    ];

    public bool CanCastle { get; private set; }
    
    public King(Color color, Position position) : base(color, position)
    {
        var startingRow = Color == Color.Light ? 0 : 7;
        CanCastle = startingRow == position.Row && position.Column is Column.E;
    }

    public override void Move(ChessBoard chessBoard, Position to)
    {
        CanCastle = false;
        var from = Position;
        
        chessBoard.MovePiece(this, to);
        ChangePosition(to);
        
        if (!IsCastlingMove(from, to))
        {
            chessBoard.UpdateBoardState(Color);
            return;
        }
        
        chessBoard.Castle(this, from, to);
    }

    public override MoveResult GetAvailableMoves(ChessBoard chessBoard)
    {
        List<Position> moves = [];
        List<Position> attacks = [];

        var checkState = chessBoard.GetCheckState(Color);
        var isKingChecked = checkState.IsChecked || checkState.IsDoubleChecked;
        
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
            else if (chessBoard[position].HasEnemyPiece(Color) && canMoved)
            {
                attacks.Add(position);
            }
        }

        if (!isKingChecked && CanCastle && !enemyAttacks.Contains(Position))
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

    private static bool IsCastlingMove(Position from, Position to)
    {
        var columnOffset = to.Column.DistanceTo(from.Column);
        return columnOffset == 2;
    }
}