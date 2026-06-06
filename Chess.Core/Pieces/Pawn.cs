using Chess.Core.Board;

namespace Chess.Core.Pieces;

public sealed class Pawn : Piece
{
    private static readonly MoveDirection[][] AttackDirections =
    [
        [Position.TryMoveLeftUp, Position.TryMoveRightUp],
        [Position.TryMoveLeftDown, Position.TryMoveRightDown],
    ];
    
    private static readonly MoveDirection[] ForwardDirections = [Position.TryMoveUp, Position.TryMoveDown];
    
    public bool CanDoubleAdvance { get; private set; }

    public event EventHandler<PromotionEventArgs>? Promoted;
    
    public Pawn(Color color, Position position) : base(color, position)
    {
        CanDoubleAdvance = (color == Color.Light && position.Row == 1) 
                      || (color == Color.Dark && position.Row == 6);
    }

    public override void Move(ChessBoard chessBoard, Position to)
    {
        CanDoubleAdvance = false;
        
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

    public override IEnumerable<Position> GetAttackedPositions(ChessBoard chessBoard) =>
        GetAttackedPositions(AttackDirections[(int)Color]);

    private IEnumerable<Position> GetAttackedPositions(IEnumerable<MoveDirection> directions)
    {
        foreach (var moveDirection in directions)
        {
            var position = Position;
            if (moveDirection(ref position))
            {
                yield return position;
            }
        }
    }

    private List<Position> FindMoves(ChessBoard chessBoard)
    {
        var moves = new List<Position>(2);
        var directionMove = ForwardDirections[(int)Color];
        var position = Position;

        if (IsPinned && !AllowedDirections!.Contains(directionMove)) return moves;
        
        if (!directionMove(ref position) || chessBoard[position].HasPiece) return moves;
        
        moves.Add(position);
        if (CanDoubleAdvance && directionMove(ref position) && !chessBoard[position].HasPiece)
            moves.Add(position);

        return moves;
    }

    private List<Position> FindAttacks(ChessBoard chessBoard)
    {
        var attacks = new List<Position>(2);

        var attackDirections = IsPinned
            ? AttackDirections[(int)Color].Intersect(AllowedDirections!)
            : AttackDirections[(int)Color];
        
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