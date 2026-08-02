namespace Chess.Core;

public record AvailableMoves(IReadOnlyList<Position> Moves, IReadOnlyList<Position> Attacks)
{
    public static AvailableMoves Empty { get; } = new([], []);
    public bool HasMoves => Moves.Count + Attacks.Count > 0;

    public bool Contains(Position position)
    {
        return Moves.Contains(position) || Attacks.Contains(position);
    }
}