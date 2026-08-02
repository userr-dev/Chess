namespace Chess.Core;

public class Game
{
    public Color CurrentPlayer { get; private set; } = Color.Light;
    public GameResult GameResult { get; private set; } = GameResult.None;

    public bool IsGameEnd => GameResult is not GameResult.None;
    
    public ChessBoard ChessBoard { get; } = ChessBoard.CreateStandard();

    public bool TryMove(Piece piece, AvailableMoves availableMoves, Position to)
    {
        if (IsGameEnd) return false;
        if (piece.Color != CurrentPlayer) return false;
        if (!availableMoves.Contains(to)) return false;
        
        piece.Move(ChessBoard, to);
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