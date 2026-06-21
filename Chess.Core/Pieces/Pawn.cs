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

    public event EventHandler<PromotionEventArgs>? Promoted;
    
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
        
        RaisePromotion(chessBoard, to);
    }

    public override MoveResult GetAvailableMoves(ChessBoard chessBoard)
    {
        var checkState = chessBoard.GetCheckState(Color);

        if (checkState.IsDoubleChecked) return new MoveResult([], []);
        
        var moves = FindMoves(chessBoard);
        var attacks = FindAttacks(chessBoard);

        return ApplyCheckFilter(checkState, moves, attacks);
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

    private List<Position> FindMoves(ChessBoard chessBoard)
    {
        var moves = new List<Position>(2);
        var direction = ForwardDirections[Color];
        var position = Position;

        if (IsPinned && !AllowedDirections!.Contains(direction)) return moves;
        
        if (!Position.TryMove(ref position, direction) || chessBoard[position].HasPiece) return moves;
        
        moves.Add(position);
        if (CanDoubleAdvance && Position.TryMove(ref position, direction) && !chessBoard[position].HasPiece)
            moves.Add(position);

        return moves;
    }

    private List<Position> FindAttacks(ChessBoard chessBoard)
    {
        var attacks = new List<Position>(2);

        var attackDirections = IsPinned
            ? AttackDirections[Color].Intersect(AllowedDirections!)
            : AttackDirections[Color];
        
        foreach (var position in GetAttackedPositions(attackDirections))
        {
            if (chessBoard[position].HasEnemyPiece(Color))
            {
                attacks.Add(position);
            }
        }

        return attacks;
    }

    private void RaisePromotion(ChessBoard chessBoard, Position to)
    {
        var args = new PromotionEventArgs(Color, to);
        Promoted?.Invoke(this, args);
        chessBoard.PromotePawn(this, args.PromotedPiece ?? new Queen(Color, to));
    }
    
    public sealed class PromotionEventArgs(Color color, Position position) : EventArgs
    {
        public Color Color { get; } = color;
        public Position Position { get; } = position;
    
        public Piece? PromotedPiece 
        { 
            get;
            set
            {
                if (value is King or Pawn)
                    throw new ArgumentException("Cannot promote to King or Pawn.");
                if (value?.Color != Color)
                    throw new ArgumentException("Cannot promote to enemy color");
                field = value;
            }
        }
    }
}