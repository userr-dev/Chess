using System.Diagnostics.CodeAnalysis;

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

    public bool IsEnPassant { get; private set; }
    
    public override char? AnnotationSymbol => null;

    public event EventHandler<PromotionEventArgs>? Promoted; 
    
    public Pawn(Color color, Position position) : base(color, position)
    {
        CanDoubleAdvance = (color == Color.Light && position.Row == 1) 
                      || (color == Color.Dark && position.Row == 6);
    }

    internal override Result Move(ChessBoard chessBoard, Position to)
    {
        CanDoubleAdvance = false;

        var from = Position;
        
        if (Math.Abs(to.Row - from.Row) == 2)
        {
            IsEnPassant = true;
        }
        var result = MoveCore(chessBoard, to);

        if (IsEnPassantCapturedMove(chessBoard, to, out var enPassantPawn))
        {
            var enPassantResult = chessBoard.CaptureEnPassant(this, from, to, enPassantPawn);
            return enPassantResult;
        }
        
        if (!to.IsPromotionRow(Color))
        {
            chessBoard.UpdateBoardState(Color);
            return result;
        }
        
        var promotionPiece = RaisePromotion(chessBoard, to);
        return result.ToPromoted(promotionPiece);
    }

    private bool IsEnPassantCapturedMove(ChessBoard chessBoard, Position to, [MaybeNullWhen(false)] out Pawn enPassantPawn)
    {
        var rowIncrement = Color is Color.Light ? 1 : -1;
        var enPassantPosition = Position.Create(to.Column, to.Row - rowIncrement);
        enPassantPawn = null;

        if (chessBoard[enPassantPosition].Piece is not Pawn { IsEnPassant: true }) return false;
        
        enPassantPawn = (Pawn)chessBoard[enPassantPosition].Piece!;
        return true;
    }

    internal void NotEnPassant()
    {
        IsEnPassant = false;
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
        var enPassantMove = GetEnPassantCheckEvasionAttack(chessBoard, checkState);
        if (enPassantMove.HasValue) attacks.Add(enPassantMove.Value);
        
        var moves = GetCheckBlockingMoves(chessBoard, checkState, attackerPosition);
        
        return new AvailableMoves(moves, attacks);
    }

    private Position? GetEnPassantCheckEvasionAttack(ChessBoard chessBoard, CheckState checkState)
    {
        if (checkState.Attacker is not Pawn { IsEnPassant: true }) return null;

        var enPassantMove = GetEnPassantMove(chessBoard, GetAttacksDirections());
        return enPassantMove;
    }
    
    private List<Position> GetCheckEvasionAttacks(Position attackerPosition)
    {
        List<Position> attacks = new(2);
        
        var attackDirection = Position.GetDirectionFromTo(Position, attackerPosition);
        if (!GetAttacksDirections().Contains(attackDirection)) return attacks;
        
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

    private Position? GetEnPassantMove(ChessBoard chessBoard, Direction[] attackDirections)
    {
        Position? enPassantAttackPosition = null;
        
        foreach (var attackDirection in attackDirections)
        {
            var position = Position;
            if (!Position.TryMove(ref position, attackDirection)) continue;
            
            var rowIncrement = Color is Color.Light ? 1 : -1;
            var enPassantPosition = Position.Create(position.Column, position.Row - rowIncrement);
            enPassantAttackPosition ??=
                chessBoard[enPassantPosition].Piece is Pawn { IsEnPassant: true } ? position : null;
        }

        return enPassantAttackPosition;
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

        var enPassantMove = GetEnPassantMove(chessBoard, attacksDirections);
        if (enPassantMove.HasValue)
        {
            attacks.Add(enPassantMove.Value);
        }
        
        return attacks;
    }

    private Direction[] GetAttacksDirections()
    {
        var attackDirections = AttackDirections[Color];
        return IsPinned ? [.. attackDirections.Intersect(PinnedDirections!)] : attackDirections;
    }
    
    private Piece RaisePromotion(ChessBoard chessBoard, Position to)
    {
        var args = new PromotionEventArgs(Color, to);
        Promoted?.Invoke(this, args);

        var promotionPiece = args.PromotedPiece ?? new Queen(Color, to);
        chessBoard.PromotePawn(this, promotionPiece);

        return promotionPiece;
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