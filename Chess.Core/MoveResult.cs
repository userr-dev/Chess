using Chess.Core.Board;

namespace Chess.Core;

public record MoveResult(IReadOnlyList<Position> Moves, IReadOnlyList<Position> Attacks);