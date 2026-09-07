namespace Chess.Core.MoveResults;

public sealed class Castled : Result
{
    public Moved RookMoved { get; }
    public CastleSide CastleSide { get; }
    
    private Castled(King movedKing, Position from, Position to, Moved rookMoved, CastleSide castleSide) : base(movedKing, from, to)
    {
        RookMoved = rookMoved;
        CastleSide = castleSide;
    }

    public override string ToNotation()
    {
        var castleAnnotation = CastleSide is CastleSide.KingSide ? "O-O" : "O-O-O";
        return $"{castleAnnotation}{CheckSuffix}";
    }

    internal static Castled Create(King movedKing, Position from, Position to, Moved rookMoved, CastleSide castleSide)
    {
        if (rookMoved.MovedPiece is not Rook) throw new ArgumentException("Not rook moved");
        
        return new Castled(movedKing, from, to, rookMoved, castleSide);
    }
}