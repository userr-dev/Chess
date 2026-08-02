namespace Chess.Core.Pieces;

public abstract class SlidingPiece : Piece
{
    protected abstract Direction[] OwnDirections { get; }
    protected SlidingPiece(Color color, Position position) : base(color, position)
    {
    }

    private Direction[] GetDirections()
    {
        return IsPinned ? PinnedDirections! : OwnDirections;
    }
    
    protected IEnumerable<Position> GetAttackedPositionsAlongDirections(ChessBoard chessBoard,
        IEnumerable<Direction> directions)
    {
        chessBoard.TryGetKing(Color.Opposite(), out var enemyKing);
        
        foreach (var direction in directions)
        {
            var position = Position;
            while (Position.TryMove(ref position, direction))
            {
                yield return position;
                if (chessBoard[position].HasPiece && chessBoard[position].Piece == enemyKing) continue;
                if (chessBoard[position].HasPiece) break;
            }
        }
    }

    internal override bool IsAttackedKing(ChessBoard chessBoard, King enemyKing)
    {
        if (!TryGetDirectionToKing(enemyKing, out var direction)) return false;

        var position = Position;
        while (Position.TryMove(ref position, direction))
        {
            if (chessBoard[position].Piece == enemyKing) return true;
            if (chessBoard[position].HasPiece) return false;
        }

        return false;
    }

    public override AvailableMoves GetAvailableMoves(ChessBoard chessBoard)
    {
        var checkState = chessBoard.GetCheckState(Color);
        if (checkState.IsDoubleChecked) return AvailableMoves.Empty;
        
        return checkState.IsChecked
            ? GetCheckEvasionMoves(chessBoard, checkState)
            : GetMovesAndAttacks(chessBoard);
    }

    private AvailableMoves GetMovesAndAttacks(ChessBoard chessBoard)
    {
        List<Position> moves = [];
        List<Position> attacks = [];
        foreach (var position in GetAttackedPositionsAlongDirections(chessBoard, GetDirections()))
        {
            ClassifyMoves(chessBoard, position, moves, attacks);
        }

        return new AvailableMoves(moves, attacks);
    }

    private AvailableMoves GetCheckEvasionMoves(ChessBoard chessBoard, CheckState checkState)
    {
        List<Position> moves = [];
        List<Position> attacks = [];
        foreach (var target in checkState.Targets)
        {
            var direction = Position.GetDirectionFromTo(Position, target).NormalizedOrSelf();

            if (!GetDirections().Contains(direction)) continue;
        
            var position = Position;
            while (Position.TryMove(ref position, direction))
            {
                if (position != target && chessBoard[position].HasPiece) break;
                if (position != target) continue;
                
                ClassifyMoves(chessBoard, target, moves, attacks);
                break;
            }
        }

        return new AvailableMoves(moves, attacks);
    }
    
    internal void DetectAndMarkPin(ChessBoard chessBoard , King enemyKing)
    {
        if (!TryGetDirectionToKing(enemyKing, out var direction)) return;
        
        Piece? candidate = null;
        var position = Position;
        while (Position.TryMove(ref position, direction))
        {
            var square = chessBoard[position];

            if (square.Piece == enemyKing)
            {
                candidate?.Pin(direction.GetAxis());
                break;
            }

            if (square.HasFriendlyPiece(Color)) break;
            if (square.HasPiece && candidate is not null) break;
            candidate ??= square.Piece;
        }
    }

    internal bool TryGetDirectionToKing(King enemyKing, out Direction direction)
    {
        direction = Position.GetDirectionFromTo(Position, enemyKing.Position).NormalizedOrSelf();
        return OwnDirections.Contains(direction);
    }
    
    protected override void OnPin(Direction[] pinnedAxis)
    {
        PinnedDirections = OwnDirections.Intersect(pinnedAxis).ToArray();
    }
}