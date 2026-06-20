using KnightMoveOffset = (int Column, int Row);

namespace Chess.Core.Pieces;

public sealed class Knight : Piece
{
    private static readonly HashSet<KnightMoveOffset> MoveOffsets =
        [(-1, 2), (1, 2), (-1, -2), (1, -2), (-2, 1), (-2, -1), (2, 1), (2, -1)];
    
    public Knight(Color color, Position position) : base(color, position)
    {
    }
    
    public override MoveResult GetAvailableMoves(ChessBoard chessBoard)
    {
        List<Position> moves = [];
        List<Position> attacks = [];

        var checkState = chessBoard.GetCheckState(Color);
        
        if (IsPinned || checkState.IsDoubleChecked) return new MoveResult(moves, attacks);
        if (checkState.IsChecked) return GetCheckEvasionMoves(chessBoard, checkState, moves, attacks);
        
        foreach (var position in GetAttackedPositions(chessBoard))
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

    private MoveResult GetCheckEvasionMoves(ChessBoard chessBoard, CheckState checkState, List<Position> moves, List<Position> attacks)
    {
        Position[] targets = [..checkState.BlockingPositions, checkState.Attackers[0].Position];

        foreach (var target in targets)
        {
            var offset = new KnightMoveOffset(
                (int)target.Column - (int)Position.Column,
                target.Row - Position.Row);

            if (!MoveOffsets.Contains(offset)) continue;

            if (!chessBoard[target].HasPiece)
                moves.Add(target);
            else if (chessBoard[target].HasEnemyPiece(Color))
                attacks.Add(target);
        }

        return new MoveResult(moves, attacks);
    }
    
    internal override IEnumerable<Position> GetAttackedPositions(ChessBoard chessBoard)
    {
        foreach (var offset in MoveOffsets)
        {
            var position = Position;
            if (Position.TryMove(ref position, offset.Column, offset.Row))
            {
                yield return position;
            }
        }
    }
}