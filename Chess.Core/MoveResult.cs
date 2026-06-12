namespace Chess.Core;

public record MoveResult(IReadOnlyList<Position> Moves, IReadOnlyList<Position> Attacks)
{
    public bool HasMoves => Moves.Count + Attacks.Count > 0;
}