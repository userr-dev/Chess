using Chess.Core.Board;
using Chess.Core.Pieces.Interfaces;

namespace Chess.Core.Pieces;

public sealed class Pawn : Piece, IPawn
{
    private static readonly MoveDirection[][] AttackDirections =
    [
        [Position.TryMoveLeftUp, Position.TryMoveRightUp],
        [Position.TryMoveLeftDown, Position.TryMoveRightDown],
    ];
    
    private static readonly MoveDirection[] ForwardDirections = [Position.TryMoveUp, Position.TryMoveDown];
    
    public bool IsFirstMove { get; private set; } = true;

    public event Action<PromotionEventArgs>? Promotion;
    
    public Pawn(Color color, Position position) : base(color, position)
    {
    }

    public override void Move(ChessBoard chessBoard, Position to)
    {
        IsFirstMove = false;
        
        base.Move(chessBoard, to);

        if (!to.IsPromotionRow(Color)) return;
        
        var args = new PromotionEventArgs(Color, to);
        Promotion?.Invoke(args);
        chessBoard.PromotePawn(this, args.PromotedPiece ?? new Queen(Color, to));
    }

    public override MoveResult GetAvailableMoves(ChessBoard chessBoard)
    {
        return new MoveResult(
            FindMoves(chessBoard), 
            FindAttacks(chessBoard));
    }

    public override IEnumerable<Position> GetAttackedPositions(ChessBoard chessBoard)
    {
        foreach (var moveDirection in AttackDirections[(int)Color])
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

        if (!directionMove(ref position) || chessBoard[position].HasPiece) return moves;
        
        moves.Add(position);
        if (IsFirstMove && directionMove(ref position) && !chessBoard[position].HasPiece)
            moves.Add(position);

        return moves;
    }

    private List<Position> FindAttacks(ChessBoard chessBoard)
    {
        var attacks = new List<Position>(2);

        foreach (var position in GetAttackedPositions(chessBoard))
        {
            if (chessBoard[position].HasPiece
                && !chessBoard[position].HasPieceOfColor(Color))
            {
                attacks.Add(position);
            }
        }

        return attacks;
    }
    
    public sealed class PromotionEventArgs(Color color, Position position) : EventArgs
    {
        public Color Color { get; } = color;
        public Position Position { get; } = position;
    
        public Piece? PromotedPiece { get; set; }
    }
}