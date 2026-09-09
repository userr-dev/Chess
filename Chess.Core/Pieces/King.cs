namespace Chess.Core.Pieces;

public sealed class King : Piece
{
    private static readonly Direction[] Directions =
    [
        Direction.Up, Direction.Down, Direction.Left, Direction.Right, Direction.LeftUp, Direction.LeftDown,
        Direction.RightDown, Direction.RightUp
    ];

    public bool CanCastle { get; private set; }

    public override char? AnnotationSymbol => 'K';

    public King(Color color, Position position) : base(color, position)
    {
        var startingRow = Color == Color.Light ? 0 : 7;
        CanCastle = startingRow == position.Row && position.Column is Column.E;
    }

    internal override Result Move(ChessBoard chessBoard, Position to)
    {
        CanCastle = false;
        var from = Position;

        var result = MoveCore(chessBoard, to);
        
        if (!IsCastlingMove(from, to))
        {
            chessBoard.UpdateBoardState(Color);
            return result;
        }
        
        return chessBoard.Castle(this, from, to);
    }

    public override AvailableMoves GetAvailableMoves(ChessBoard chessBoard)
    {
        List<Position> moves = [];
        List<Position> attacks = [];

        var checkState = chessBoard.GetCheckState(Color);
        var isKingChecked = checkState.IsChecked;
        
        var enemyAttacks = chessBoard.GetAttackedPositions(Color.Opposite());

        foreach (var position in GetAttackedPositions(chessBoard))
        {
            var canMove = !enemyAttacks.Contains(position);
            
            if (!canMove) continue;
            
            ClassifyMoves(chessBoard, position, moves, attacks);
        }

        if (!isKingChecked && CanCastle && !enemyAttacks.Contains(Position))
        {
            var castlingMoves = GetCastlingMoves(chessBoard, enemyAttacks);
            moves.AddRange(castlingMoves);
        }
        
        return new AvailableMoves(moves, attacks);
    }

    internal override bool IsAttackedKing(ChessBoard chessBoard, King enemyKing) => false;

    internal override IEnumerable<Position> GetAttackedPositions(ChessBoard chessBoard)
    {
        foreach (var direction in Directions)
        {
            var position = Position;
            if (!Position.TryMove(ref position, direction)) continue;
            yield return position;
        }
    }

    private IEnumerable<Position> GetCastlingMoves(ChessBoard chessBoard, HashSet<Position> enemyAttackedPositions)
    {
        var rooks = chessBoard.GetCastlingRooks(Color);
        
        foreach (var rook in rooks)
        {
            var isKingSide = rook.Position.Column > Position.Column;
            var direction = isKingSide ? Direction.Right : Direction.Left;
            if (!IsCastlingPathClear(chessBoard, rook.Position, enemyAttackedPositions, direction)) continue;
            
            var columnOffset = isKingSide ? 2 : -2;
            yield return Position.Create(Position.Column.Shift(columnOffset), Position.Row);
        }
    }

    private bool IsCastlingPathClear(ChessBoard chessBoard, Position rookPosition,
        HashSet<Position> enemyAttackedPositions, Direction direction)
    {
        var position = Position;
        while (Position.TryMove(ref position, direction) && position != rookPosition)
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