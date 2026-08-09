namespace Chess.Core.Pieces;

public sealed class Pawn : Piece
{
    private static readonly Dictionary<Color, Direction[]> AttackDirections = new()
    {
        { Color.Light, [Direction.LeftUp, Direction.RightUp] },
        { Color.Dark, [Direction.LeftDown, Direction.RightDown] }
    };
    
    private static readonly Dictionary<Color, Direction> ForwardDirections = new()
    {
        { Color.Light, Direction.Up },
        { Color.Dark, Direction.Down },
    };
    
    public bool CanDoubleAdvance { get; private set; }
    
    public Pawn(Color color, Position position) : base(color, position)
    {
        CanDoubleAdvance = (color == Color.Light && position.Row == 1) 
                      || (color == Color.Dark && position.Row == 6);
    }

    internal override void Move(ChessBoard chessBoard, Position to)
    {
        CanDoubleAdvance = false;

        if (chessBoard[to].HasEnemyPiece(Color))
        {
            chessBoard.CapturePiece(chessBoard[to].Piece!);
        }
        chessBoard.MovePiece(this, to);
        ChangePosition(to);

        if (!to.IsPromotionRow(Color))
        {
            chessBoard.UpdateBoardState(Color);
            return;
        }
        
        chessBoard.PromotePawn(this, new Queen(Color, to)); // Default Promotion
    }

    internal override bool IsAttackedKing(ChessBoard chessBoard, King enemyKing)
    {
        var direction = Position.GetDirectionFromTo(Position, enemyKing.Position);
        return AttackDirections[Color].Contains(direction);
    }

    internal override IEnumerable<Position> GetAttackedPositions(ChessBoard chessBoard) =>
        GetAttackedPositions(AttackDirections[Color]);

    private IEnumerable<Position> GetAttackedPositions(IEnumerable<Direction> directions)
    {
        foreach (var direction in directions)
        {
            var position = Position;
            if (Position.TryMove(ref position, direction))
            {
                yield return position;
            }
        }
    }

    public override AvailableMoves GetAvailableMoves(ChessBoard chessBoard)
    {
        var checkState = chessBoard.GetCheckState(Color);

        if (checkState.IsDoubleChecked) return AvailableMoves.Empty;
        
        return checkState.IsChecked 
            ? GetCheckEvasionMoves(chessBoard, checkState) 
            : GetMovesAndAttacks(chessBoard);
    }
    
    private AvailableMoves GetCheckEvasionMoves(ChessBoard chessBoard, CheckState checkState)
    {
        var attackerPosition = checkState.Attacker!.Position;
        
        var attacks = GetCheckEvasionAttacks(attackerPosition);
        var moves = GetCheckBlockingMoves(chessBoard, checkState, attackerPosition);
        
        return new AvailableMoves(moves, attacks);
    }

    private List<Position> GetCheckEvasionAttacks(Position attackerPosition)
    {
        List<Position> attacks = [];
        
        var attackDirection = Position.GetDirectionFromTo(Position, attackerPosition);
        if (!AttackDirections[Color].Contains(attackDirection)) return attacks;
        if (IsPinned && !PinnedDirections!.Contains(attackDirection)) return attacks;
        
        attacks.Add(attackerPosition);
        return attacks;
    }

    private List<Position> GetCheckBlockingMoves(ChessBoard chessBoard, CheckState checkState, Position attackerPosition)
    {
        if (checkState.BlockingPositions.Count == 0) return [];
        chessBoard.TryGetKing(Color, out var king);

        return GetMoves(chessBoard, position => Position.IsInDirection(attackerPosition, king!.Position, position));
    }

    private AvailableMoves GetMovesAndAttacks(ChessBoard chessBoard)
    {
        var moves = GetMoves(chessBoard);
        var attacks = GetAttacks(chessBoard);

        return new AvailableMoves(moves, attacks);
    }
    
    private List<Position> GetMoves(ChessBoard chessBoard)
    {
        return GetMoves(chessBoard, position => !chessBoard[position].HasPiece);
    }

    private List<Position> GetMoves(ChessBoard chessBoard, Predicate<Position> filter)
    {
        var moves = new List<Position>(2);
        var direction = ForwardDirections[Color];

        if (IsPinned && !PinnedDirections!.Contains(direction)) return moves;
        
        var position = Position;
        for (int i = 0; i < (CanDoubleAdvance ? 2 : 1); i++)
        {
            if (!Position.TryMove(ref position, direction) || chessBoard[position].HasPiece) break;
            if (filter(position)) moves.Add(position);
        }

        return moves;
    }
    
    private List<Position> GetAttacks(ChessBoard chessBoard)
    {
        var attacks = new List<Position>(2);

        var attacksDirections = GetAttacksDirections();
        foreach (var position in GetAttackedPositions(attacksDirections))
        {
            if (chessBoard[position].HasEnemyPiece(Color))
            {
                attacks.Add(position);
            }
        }

        return attacks;
    }

    private IEnumerable<Direction> GetAttacksDirections()
    {
        var attackDirections = AttackDirections[Color];
        return IsPinned ? attackDirections.Intersect(PinnedDirections!) : attackDirections;
    }
}