namespace Chess.Core.Board;

public class CheckState
{
    private readonly Color _color;
    private readonly Color _enemyColor;
    
    private readonly List<Piece> _attackers = [];
    private readonly List<Position> _blockingPositions = [];
    private Position[]? _targets;
    
    public bool IsChecked => _attackers.Count > 0;
    public bool IsDoubleChecked => _attackers.Count >= 2;

    public Piece? Attacker { get; private set; }
    public IReadOnlyList<Piece> Attackers => _attackers;
    public IReadOnlyList<Position> BlockingPositions => _blockingPositions;
    public IReadOnlyList<Position> Targets => _targets ?? [];
    
    internal CheckState(Color color)
    {
        _color = color;
        _enemyColor = _color.Opposite();
    }
    
    internal void Update(ChessBoard chessBoard)
    {
        Reset();
        
        if (!chessBoard.TryGetKing(_color, out var king)) return;
        
        _attackers.AddRange(GetAttackers(chessBoard, _enemyColor, king));
        if (_attackers.Count is not 1) return;

        Attacker = _attackers[0];
        _blockingPositions.AddRange(GetBlockingPositions(chessBoard, Attacker, king));

        _targets = [Attacker.Position, .._blockingPositions];
    }

    private void Reset()
    {
        _attackers.Clear();
        _blockingPositions.Clear();
        _targets = null;
        Attacker = null;
    }
    
    private static IEnumerable<Piece> GetAttackers(ChessBoard chessBoard, Color attackingColor, King king)
    {
        return chessBoard.GetPieces(attackingColor).Where(p => p.IsAttackedKing(chessBoard, king));
    }
    
    private static IEnumerable<Position> GetBlockingPositions(ChessBoard chessBoard, Piece attacker, King king)
    {
        return attacker is SlidingPiece slidingPiece
            ? GetAttackedPositionsToKing(chessBoard, king, slidingPiece)
            : [];
    }

    private static IEnumerable<Position> GetAttackedPositionsToKing(ChessBoard chessBoard, King king, SlidingPiece slidingPiece)
    {
        if (!slidingPiece.TryGetDirectionToKing(king, out var direction)) yield break;

        var position = slidingPiece.Position;
        while (Position.TryMove(ref position, direction))
        {
            if(chessBoard[position].Piece == king) break;
            yield return position;
        }
    }
}