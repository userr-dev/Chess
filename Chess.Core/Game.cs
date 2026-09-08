namespace Chess.Core;

public class Game
{
    public Color CurrentPlayer { get; private set; } = Color.Light;
    public GameResult GameResult { get; private set; } = GameResult.None;

    public bool IsGameStarted { get; private set; }
    public bool IsGameEnd => GameResult is not GameResult.None;
    
    public ChessBoard ChessBoard { get; } = ChessBoard.Create();

    public Clock? Clock { get; private set; }

    public MoveHistory MoveHistory { get; } = MoveHistory.Create();
    
    public event Action? GameEnded;
    
    private Game()
    {
        GameEnded += OnGameEnded;
    }

    private void OnGameEnded()
    {
        Stop();
    }

    private void ClockOnTimeExpired(Color color)
    {
        GameResult = color == Color.Light ? GameResult.DarkWin : GameResult.LightWin;
        GameEnded?.Invoke();
    }

    public void Start()
    {
        IsGameStarted = true;
    }

    private void Stop()
    {
        IsGameStarted = false;
    }
    
    public void SetTimeControl(TimeControl? timeControl)
    {
        if (!timeControl.HasValue)
        {
            Clock = null;
            return;
        }
        
        Clock = Clock.FromTimeControl(timeControl.Value);
        Clock.TimeExpired += ClockOnTimeExpired;
    }

    public void Reset()
    {
        ClearMoveHistory();
        ChessBoard.Reset();
        GameResult = GameResult.None;
        CurrentPlayer = Color.Light;
    }

    public void ClearMoveHistory()
    {
        MoveHistory.Clear();
    }
    
    public bool TryMove(Piece piece, AvailableMoves availableMoves, Position to)
    {
        if (!IsGameStarted) return false;
        if (IsGameEnd) return false;
        if (piece.Color != CurrentPlayer) return false;
        if (!availableMoves.Contains(to)) return false;

        var result = piece.Move(ChessBoard, to);
        UpdateGameResult();
        
        result.AppendCheckSuffix(GetCheckSuffix());
        MoveHistory.Add(result);
        
        Clock?.OnMoveCompleted(CurrentPlayer);
        CurrentPlayer = CurrentPlayer.Opposite();
        return true;
    }

    private char? GetCheckSuffix()
    {
        var opposite = CurrentPlayer.Opposite();
        var checkState = ChessBoard.GetCheckState(opposite);
        return IsGameEnd && checkState.IsChecked ? '#' : checkState.IsChecked ? '+' : null;
    }
    
    private void UpdateGameResult()
    {
        var opponent = CurrentPlayer.Opposite();
        if (ChessBoard.IsCheckmate(opponent))
        {
            GameResult = CurrentPlayer == Color.Light ? GameResult.LightWin : GameResult.DarkWin;
            GameEnded?.Invoke();
        }
        else if (ChessBoard.IsStalemate(opponent))
        {
            GameResult = GameResult.Draw;
            GameEnded?.Invoke();
        }
    }

    public static Game Create() => new();
}