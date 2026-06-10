using Chess.Core.Board;

namespace Chess.Core.Pieces;

public abstract class Piece
{
    public Color Color { get; }
    public Position Position { get; private set; }
    public MoveDirection[]? AllowedDirections { get; set; }
    public bool IsPinned => AllowedDirections is not null;

    protected Piece(Color color, Position position)
    {
        Color = color;
        Position = position;
    }

    protected static MoveResult ApplyCheckFilter(CheckState checkState, List<Position> moves, List<Position> attacks)
    {
        if (!checkState.IsChecked)
            return new MoveResult(moves, attacks);
        
        return new MoveResult(
            moves.Intersect(checkState.BlockingPositions).ToList(),
            attacks.Where(p => p == checkState.Attackers[0].Position).ToList());
    }
    
    public abstract MoveResult GetAvailableMoves(ChessBoard chessBoard);

    public abstract IEnumerable<Position> GetAttackedPositions(ChessBoard chessBoard);
    
    public virtual void Move(ChessBoard chessBoard, Position to)
    {
        if (chessBoard[to].HasEnemyPiece(Color))
        {
            chessBoard.CapturePiece(chessBoard[to].Piece!);
        }
        chessBoard.MovePiece(this, to);
        ChangePosition(to);
        chessBoard.UpdateBoardState(Color);
    }

    protected void ChangePosition(Position to)
    {
        Position = to;
    }
    
    public override string ToString()
    {
        return $"{GetType().Name} {Position}";
    }
}