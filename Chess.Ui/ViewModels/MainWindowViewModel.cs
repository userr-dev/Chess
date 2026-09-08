using System;
using System.Collections.ObjectModel;
using Avalonia.Threading;
using Chess.Core;
using Chess.Core.MoveResults;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Chess.Ui.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly DispatcherTimer _timer;
    private readonly Game _game;

    private TimeControl? _selectedTimeControl;
    
    public BoardViewModel Board { get; }
    
    [ObservableProperty] 
    public partial bool IsGameMenuOpen { get; private set; } = true;

    [ObservableProperty] 
    public partial bool IsMoveHistoryMenuOpen { get; private set; } = false;
    
    [ObservableProperty]
    public partial TimeSpan? LightTimeRemaining { get; private set; }
    
    [ObservableProperty]
    public partial TimeSpan? DarkTimeRemaining { get; private set; }

    public ObservableCollection<Result> MoveHistory { get; } = [];
    
    public MainWindowViewModel()
    {
        _game = Game.Create();
        _game.GameEnded += GameOnGameEnded;
        _game.MoveHistory.Added += MoveHistoryOnAdded;
        _game.MoveHistory.Cleared += MoveHistoryOnCleared;
        
        LightTimeRemaining = _game.Clock?.GetRemaining(Color.Light);
        DarkTimeRemaining = _game.Clock?.GetRemaining(Color.Dark);
        
        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick += TimerOnTick;
        
        Board = new BoardViewModel(_game);
    }

    private void MoveHistoryOnCleared()
    {
        MoveHistory.Clear();
    }

    private void MoveHistoryOnAdded(Result result)
    {
        MoveHistory.Add(result);
    }

    private void GameOnGameEnded()
    {
        IsGameMenuOpen = true;
        IsMoveHistoryMenuOpen = false;
        _timer.Stop();
    }

    private void ClockOnIncrementAdded(Color color)
    {
        if (color == Color.Light)
        {
            LightTimeRemaining = _game.Clock?.GetRemaining(Color.Light);
        }
        else
        {
            DarkTimeRemaining = _game.Clock?.GetRemaining(Color.Dark);
        }
    }

    private void TimerOnTick(object? sender, EventArgs e)
    {
        _game.Clock?.Tick(_game.CurrentPlayer, TimeSpan.FromSeconds(1));
        if (_game.CurrentPlayer == Color.Light)
        {
            LightTimeRemaining = _game.Clock?.GetRemaining(Color.Light);
        }
        else
        {
            DarkTimeRemaining = _game.Clock?.GetRemaining(Color.Dark);
        }
    }

    [RelayCommand]
    private void ToggleStartMenu()
    {
        IsGameMenuOpen = !IsGameMenuOpen;
    }

    [RelayCommand]
    private void ToggleMoveHistoryMenu()
    {
        IsMoveHistoryMenuOpen = !IsMoveHistoryMenuOpen;
    }
    
    [RelayCommand]
    private void SelectTimeControl(TimeControl? timeControl)
    {
        _selectedTimeControl = timeControl;
    }

    [RelayCommand]
    private void StartGame()
    {
        IsGameMenuOpen = false;
        IsMoveHistoryMenuOpen = true;

        _game.SetTimeControl(_selectedTimeControl);
        _game.Clock?.IncrementAdded += ClockOnIncrementAdded;
        
        LightTimeRemaining = _game.Clock?.GetRemaining(Color.Light);
        DarkTimeRemaining = _game.Clock?.GetRemaining(Color.Dark);

        if (_game.IsGameStarted || _game.IsGameEnd) _game.Reset();
        
        _game.Start();
        _timer.Start();
    }
}