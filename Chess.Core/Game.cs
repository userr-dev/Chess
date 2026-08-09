namespace Chess.Core;

public class Game
{
    public Color CurrentPlayer { get; private set; } = Color.Light;
    public GameResult GameResult { get; private set; } = GameResult.None;

    public bool IsGameEnd => GameResult is not GameResult.None;
    
    public ChessBoard ChessBoard { get; } = ChessBoard.CreateStandard();

    public bool TryMove(Piece piece, AvailableMoves availableMoves, Position to)
    {
        return TryExecuteMove(piece, availableMoves, to, () => piece.Move(ChessBoard, to));
    }

    public bool TryPromoteMove(Pawn pawn, AvailableMoves availableMoves, Position to, PromotionType promotionType)
    {
        if (!to.IsPromotionRow(pawn.Color)) return false;

        return TryExecuteMove(pawn, availableMoves, to, () =>
        {
            pawn.Move(ChessBoard, to);
            var promotionPiece = promotionType.GetPromotionPiece(pawn.Color, to);
            ChessBoard.PromotePawn(pawn, promotionPiece);
        });
    }
    
    private bool TryExecuteMove(Piece piece, AvailableMoves availableMoves, Position to, Action move)
    {
        if (IsGameEnd) return false;
        if (piece.Color != CurrentPlayer) return false;
        if (!availableMoves.Contains(to)) return false;

        move();
        UpdateGameResult();
        CurrentPlayer = CurrentPlayer.Opposite();
        return true;
    }
    
    private void UpdateGameResult()
    {
        var opponent = CurrentPlayer.Opposite();
        if (ChessBoard.IsCheckmate(opponent))
        {
            GameResult = CurrentPlayer == Color.Light ? GameResult.LightWin : GameResult.DarkWin;
        }
        else if (ChessBoard.IsStalemate(opponent))
        {
            GameResult = GameResult.Draw;
        }
    }
}