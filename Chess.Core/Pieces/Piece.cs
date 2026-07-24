namespace Chess.Core.Pieces;

public abstract class Piece
{
    public Color Color { get; }
    public Position Position { get; private set; }
    protected Direction[]? PinnedDirections { get; set; }
    public bool IsPinned => PinnedDirections is not null;
    protected Piece(Color color, Position position)
    {
        Color = color;
        Position = position;
    }
    
    public abstract MoveResult GetAvailableMoves(ChessBoard chessBoard);

    internal abstract bool IsAttackedKing(ChessBoard chessBoard, King enemyKing);
    
    internal abstract IEnumerable<Position> GetAttackedPositions(ChessBoard chessBoard);

    internal virtual void Move(ChessBoard chessBoard, Position to)
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
    
    internal void Pin(Direction[] pinnedAxis) => OnPin(pinnedAxis);
    
    internal void Unpin()
    {
        PinnedDirections = null;
    }

    protected virtual void OnPin(Direction[] pinnedAxis)
    {
        PinnedDirections = pinnedAxis;
    }

    protected void ClassifyMoves(ChessBoard chessBoard, Position position, List<Position> moves, List<Position> attacks)
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
    
    public override string ToString()
    {
        return $"{GetType().Name} {Position}";
    }
}