using System;
using Avalonia.Threading;
using Chess.Core;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chess.Ui.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly DispatcherTimer _timer;
    private readonly Game _game;
    public BoardViewModel Board { get; }

    [ObservableProperty]
    public partial TimeSpan LightTimeRemaining { get; private set; }
    
    [ObservableProperty]
    public partial TimeSpan DarkTimeRemaining { get; private set; }
    
    public MainWindowViewModel()
    {
        _game = Game.CreateWithClock(TimeControl.Rapid10);
        LightTimeRemaining = _game.Clock?.GetRemaining(Color.Light) ?? TimeSpan.Zero;
        DarkTimeRemaining = _game.Clock?.GetRemaining(Color.Dark) ?? TimeSpan.Zero;
        
        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        
        _timer.Tick += TimerOnTick;
        _game.Clock?.IncrementAdded += ClockOnIncrementAdded;
        _game.Clock?.TimeExpired += ClockOnTimeExpired;
        
        Board = new BoardViewModel(_game);
        
        _timer.Start();
    }
    
    private void ClockOnTimeExpired(Color color)
    {
        _timer.Stop();
    }

    private void ClockOnIncrementAdded(Color color)
    {
        if (color == Color.Light)
        {
            LightTimeRemaining = _game.Clock?.GetRemaining(Color.Light) ?? TimeSpan.Zero;
        }
        else
        {
            DarkTimeRemaining = _game.Clock?.GetRemaining(Color.Dark) ?? TimeSpan.Zero;
        }
    }

    private void TimerOnTick(object? sender, EventArgs e)
    {
        _game.Clock?.Tick(_game.CurrentPlayer, TimeSpan.FromSeconds(1));
        if (_game.CurrentPlayer == Color.Light)
        {
            LightTimeRemaining = _game.Clock?.GetRemaining(Color.Light) ?? TimeSpan.Zero;
        }
        else
        {
            DarkTimeRemaining = _game.Clock?.GetRemaining(Color.Dark) ?? TimeSpan.Zero;
        }
    }
}