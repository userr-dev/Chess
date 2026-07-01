namespace Chess.Core.Pieces;

public sealed class Bishop : SlidingPiece
{
    private static readonly Direction[] Directions =
        [Direction.LeftUp, Direction.LeftDown, Direction.RightDown, Direction.RightUp];

    protected override Direction[] OwnDirections => Directions;

    public Bishop(Color color, Position position) : base(color, position)
    {
    }

    internal override IEnumerable<Position> GetAttackedPositions(ChessBoard chessBoard) =>
        GetAttackedPositionsAlongDirections(chessBoard, Directions);
}