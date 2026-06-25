namespace Chess.Core.Pieces;

public sealed class Rook : SlidingPiece
{
    private static readonly Direction[] Directions = [Direction.Up, Direction.Down, Direction.Left, Direction.Right];

    public bool CanCastle { get; private set; }
    
    public Rook(Color color, Position position) : base(color, position)
    {
        var startingRow = Color == Color.Light ? 0 : 7;
        CanCastle = startingRow == position.Row && position.Column is Column.A or Column.H;
    }

    internal override void Move(ChessBoard chessBoard, Position to)
    {
        CanCastle = false;
        
        base.Move(chessBoard, to);
    }

    public override MoveResult GetAvailableMoves(ChessBoard chessBoard) =>
        GetMovesAlongDirections(chessBoard, Directions);

    internal override IEnumerable<Position> GetAttackedPositions(ChessBoard chessBoard) =>
        GetAttackedPositionsAlongDirections(chessBoard, Directions);

    internal override void FindPinnedPiece(ChessBoard chessBoard, King enemyKing) =>
        FindPinnedPieceAlongDirections(chessBoard, Directions, enemyKing);
}