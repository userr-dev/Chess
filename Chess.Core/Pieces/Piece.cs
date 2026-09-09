namespace Chess.Core.Pieces;

public abstract class Piece
{
    public Color Color { get; }
    public Position Position { get; private set; }
    protected Direction[]? PinnedDirections { get; set; }
    public bool IsPinned => PinnedDirections is not null;
    
    public abstract char? AnnotationSymbol { get; }
    
    protected Piece(Color color, Position position)
    {
        Color = color;
        Position = position;
    }
    
    public abstract AvailableMoves GetAvailableMoves(ChessBoard chessBoard);

    internal abstract bool IsAttackedKing(ChessBoard chessBoard, King enemyKing);
    
    internal abstract IEnumerable<Position> GetAttackedPositions(ChessBoard chessBoard);

    protected Result MoveCore(ChessBoard chessBoard, Position to)
    {
        Result result = Moved.Create(this, Position, to);
        
        if (chessBoard[to].HasEnemyPiece(Color))
        {
            var enemyPiece = chessBoard[to].Piece!;
            chessBoard.CapturePiece(enemyPiece);
            result = Captured.FromMoved((Moved)result, enemyPiece);
        }
        chessBoard.MovePiece(this, to);
        ChangePosition(to);

        return result;
    }
    
    internal virtual Result Move(ChessBoard chessBoard, Position to)
    {
        var result = MoveCore(chessBoard, to);
        chessBoard.UpdateBoardState(Color);

        return result;
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