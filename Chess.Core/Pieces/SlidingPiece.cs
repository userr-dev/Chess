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

    public override MoveResult GetAvailableMoves(ChessBoard chessBoard)
    {
        List<Position> moves = [];
        List<Position> attacks = [];

        var checkState = chessBoard.GetCheckState(Color);

        if (checkState.IsDoubleChecked) return new MoveResult(moves, attacks);
        if (checkState.IsChecked) return GetCheckEvasionMoves(chessBoard, checkState, moves, attacks);
        
        foreach (var position in GetAttackedPositionsAlongDirections(chessBoard, GetDirections()))
        {
            if (!chessBoard[position].HasPiece)
            {
                moves.Add(position);
            }
            else if (chessBoard[position].HasEnemyPiece(Color))
            {
                attacks.Add(position);
            }
        }

        return new MoveResult(moves, attacks);
    }

    private MoveResult GetCheckEvasionMoves(ChessBoard chessBoard, CheckState checkState, List<Position> moves, List<Position> attacks)
    {
        Position[] targets = [checkState.Attackers[0].Position, ..checkState.BlockingPositions];
    
        foreach (var target in targets)
        {
            var (columnOffset, rowOffset) = (target.Column - Position.Column, target.Row - Position.Row);
            if (!(columnOffset == 0 || rowOffset == 0 || Math.Abs(columnOffset) == Math.Abs(rowOffset))) continue;
            var direction = new Direction(Math.Sign(columnOffset), Math.Sign(rowOffset));

            if (!GetDirections().Contains(direction)) continue;
        
            var position = Position;
            while (Position.TryMove(ref position, direction))
            {
                if (position != target && chessBoard[position].HasPiece) break;
                if (position != target) continue;
                
                if (chessBoard[target].HasEnemyPiece(Color))
                    attacks.Add(target);
                else
                    moves.Add(target);
                break;
            }
        }

        return new MoveResult(moves, attacks);
    }
    
    internal void FindPinnedPiece(ChessBoard chessBoard , King enemyKing)
    {
        var (columnOffset, rowOffset) = (enemyKing.Position.Column - Position.Column, enemyKing.Position.Row - Position.Row);
        if (!(columnOffset == 0 || rowOffset == 0 || Math.Abs(columnOffset) == Math.Abs(rowOffset))) return;
        var direction = new Direction(Math.Sign(columnOffset), Math.Sign(rowOffset));
        if (!OwnDirections.Contains(direction)) return;
        
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

    protected override void OnPin(Direction[] pinnedAxis)
    {
        PinnedDirections = OwnDirections.Intersect(pinnedAxis).ToArray();
    }
}