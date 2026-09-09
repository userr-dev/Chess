namespace Chess.Core.Pieces;

public sealed class Queen : SlidingPiece
{
    private static readonly Direction[] Directions =
    [
        Direction.Up, Direction.Down, Direction.Left, Direction.Right, Direction.LeftUp, Direction.LeftDown,
        Direction.RightDown, Direction.RightUp
    ];

    protected override Direction[] OwnDirections => Directions;

    public override char? AnnotationSymbol => 'Q';

    public Queen(Color color, Position position) : base(color, position)
    {
    }

    internal override IEnumerable<Position> GetAttackedPositions(ChessBoard chessBoard) =>
        GetAttackedPositionsAlongDirections(chessBoard, Directions);
}