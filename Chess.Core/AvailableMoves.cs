namespace Chess.Core;

public sealed record AvailableMoves
{
    public IReadOnlyList<Position> Moves { get; }
    public IReadOnlyList<Position> Attacks { get; }
    public static AvailableMoves Empty { get; } = new([], []);
    public bool HasMoves => Moves.Count + Attacks.Count > 0;

    internal AvailableMoves(IReadOnlyList<Position> moves, IReadOnlyList<Position> attacks)
    {
        Moves = moves;
        Attacks = attacks;
    }
    
    public bool Contains(Position position)
    {
        return Moves.Contains(position) || Attacks.Contains(position);
    }

    public void Deconstruct(out IReadOnlyList<Position> moves, out IReadOnlyList<Position> attacks)
    {
        moves = Moves;
        attacks = Attacks;
    }
}