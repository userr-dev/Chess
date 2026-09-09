namespace Chess.Core.Pieces;

public sealed class Knight : Piece
{
    private static readonly HashSet<Direction> Directions =
        [new(-1, 2), new(1, 2), new(-1, -2), new(1, -2), new(-2, 1), new(-2, -1), new(2, 1), new(2, -1)];

    public override char? AnnotationSymbol => 'N';

    public Knight(Color color, Position position) : base(color, position)
    {
    }
    
    internal override bool IsAttackedKing(ChessBoard chessBoard, King enemyKing)
    {
        var direction = Position.GetDirectionFromTo(Position, enemyKing.Position);
        return Directions.Contains(direction);
    }
    
    public override AvailableMoves GetAvailableMoves(ChessBoard chessBoard)
    {
        List<Position> moves = [];
        List<Position> attacks = [];

        var checkState = chessBoard.GetCheckState(Color);
        
        if (IsPinned || checkState.IsDoubleChecked) return AvailableMoves.Empty;
        
        return checkState.IsChecked
            ? GetCheckEvasionMoves(chessBoard, checkState, moves, attacks)
            : GetMovesAndAttacks(chessBoard, moves, attacks);
    }

    private AvailableMoves GetMovesAndAttacks(ChessBoard chessBoard, List<Position> moves, List<Position> attacks)
    {
        foreach (var position in GetAttackedPositions(chessBoard))
        {
            ClassifyMoves(chessBoard, position, moves, attacks);
        }

        return new AvailableMoves(moves, attacks);
    }

    private AvailableMoves GetCheckEvasionMoves(ChessBoard chessBoard, CheckState checkState, List<Position> moves, List<Position> attacks)
    {
        foreach (var target in checkState.Targets)
        {
            var offset = Position.GetDirectionFromTo(Position, target);

            if (!Directions.Contains(offset)) continue;

            ClassifyMoves(chessBoard, target, moves, attacks);
        }

        return new AvailableMoves(moves, attacks);
    }
    
    internal override IEnumerable<Position> GetAttackedPositions(ChessBoard chessBoard)
    {
        foreach (var direction in Directions)
        {
            var position = Position;
            if (Position.TryMove(ref position, direction))
            {
                yield return position;
            }
        }
    }
}