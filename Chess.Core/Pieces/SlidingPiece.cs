namespace Chess.Core.Pieces;

public abstract class SlidingPiece : Piece
{
    protected SlidingPiece(Color color, Position position) : base(color, position)
    {
    }

    protected IEnumerable<Position> GetAttackedPositionsAlongDirections(ChessBoard chessBoard,
        IEnumerable<MoveDirection> directions)
    {
        chessBoard.TryGetKing(Color.Opposite(), out var enemyKing);
        
        foreach (var direction in directions)
        {
            var position = Position;
            while (direction(ref position))
            {
                yield return position;
                if (chessBoard[position].HasPiece && chessBoard[position].Piece == enemyKing) continue;
                if (chessBoard[position].HasPiece) break;
            }
        }
    }

    protected MoveResult GetMovesAlongDirections(ChessBoard chessBoard, MoveDirection[] directions)
    {
        List<Position> moves = [];
        List<Position> attacks = [];

        var checkState = chessBoard.GetCheckState(Color);

        if (checkState.IsDoubleChecked) return new MoveResult(moves, attacks);
        if (checkState.IsChecked) return GetCheckEvasionMoves(chessBoard, checkState, directions, moves, attacks);
        
        var allowedDirections = IsPinned ? directions.Intersect(AllowedDirections!) : directions;
        
        foreach (var position in GetAttackedPositionsAlongDirections(chessBoard, allowedDirections))
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

        return new MoveResult(moves, attacks);
    }

    private MoveResult GetCheckEvasionMoves(ChessBoard chessBoard, CheckState checkState,
        MoveDirection[] directions, List<Position> moves, List<Position> attacks)
    {
        Position[] targets = [.. checkState.BlockingPositions, checkState.Attackers[0].Position];
    
        foreach (var target in targets)
        {
            var (columnOffset, rowOffset) = (target.Column - Position.Column, target.Row - Position.Row);
        
            if (!MoveDirection.TryGetDirection(columnOffset, rowOffset, out var direction)) continue;
            if (!directions.Contains(direction)) continue;
            if (IsPinned && !AllowedDirections!.Contains(direction)) continue;
        
            var position = Position;
            while (direction(ref position))
            {
                if (position != target && chessBoard[position].HasPiece) break;
                if (position != target) continue;
                
                if (chessBoard[target].HasEnemyPiece(Color))
                    attacks.Add(target);
                else
                    moves.Add(target);
                break;
            }
        }

        return new MoveResult(moves, attacks);
    }
    
    protected void FindPinnedPieceAlongDirections(ChessBoard chessBoard, MoveDirection[] directions, King enemyKing)
    {
        foreach (var direction in directions)
        {
            Piece? candidate = null;
            var position = Position;
            while (direction(ref position))
            {
                var square = chessBoard[position];

                if (square.Piece == enemyKing)
                {
                    candidate?.AllowedDirections = MoveDirection.GetAxisDirections(direction);
                    break;
                }

                if (square.HasFriendlyPiece(Color)) break;
                if (square.HasPiece && candidate is not null) break;
                candidate ??= square.Piece;
            }
        }
    }

    internal abstract void FindPinnedPiece(ChessBoard chessBoard, King enemyKing);
}