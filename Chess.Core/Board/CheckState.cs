namespace Chess.Core.Board;

public class CheckState
{
    private readonly List<Piece> _attackers = [];
    private readonly List<Position> _blockingPositions = [];
    
    public bool IsChecked => Attackers.Count > 0;
    public bool IsDoubleChecked => Attackers.Count >= 2;

    public IReadOnlyList<Piece> Attackers => _attackers;
    public IReadOnlyList<Position> BlockingPositions => _blockingPositions;
    
    internal void Update(ChessBoard chessBoard, Color attackingColor)
    {
        _attackers.Clear();
        _blockingPositions.Clear();
        
        if (!chessBoard.TryGetKing(attackingColor.Opposite(), out var enemyKing)) return;
        
        _attackers.AddRange(GetAttackers(chessBoard, attackingColor, enemyKing));
        if (Attackers.Count is 0) return;

        _blockingPositions.AddRange(GetBlockingPositions(chessBoard, Attackers, enemyKing));
    }

    private static IEnumerable<Piece> GetAttackers(ChessBoard chessBoard, Color attackingColor, King enemyKing)
    {
        return chessBoard.GetPieces(attackingColor)
            .Where(p => p.GetAttackedPositions(chessBoard).Contains(enemyKing.Position));
    }
    
    private static IEnumerable<Position> GetBlockingPositions(ChessBoard chessBoard, IReadOnlyList<Piece> attackers, King enemyKing)
    {
        if (attackers.Count >= 2) return [];

        var attacker = attackers[0];
        if (attacker is not SlidingPiece) return [];

        return attacker.GetAvailableMoves(chessBoard).Moves
            .Where(p => Position.IsInDirection(attacker.Position, enemyKing.Position, p));
    }
}