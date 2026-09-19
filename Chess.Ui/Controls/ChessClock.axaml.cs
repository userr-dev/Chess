using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;

namespace Chess.Ui.Controls;

[TemplatePart(PauseButtonPartName, typeof(Button))]
public class ChessClock : TemplatedControl
{
    private readonly DispatcherTimer _timer;
    
    private const string PlayIcon = "\u25b6";
    private const string PauseIcon = "\u23f8";
    
    private const string PauseButtonPartName = "PART_PauseButton";
    private Button? _pauseButton;
    
    public static readonly StyledProperty<TimeSpan> LightTimeProperty = AvaloniaProperty.Register<ChessClock, TimeSpan>(
        nameof(LightTime));

    public TimeSpan LightTime
    {
        get => GetValue(LightTimeProperty);
        set => SetValue(LightTimeProperty, value);
    }

    public static readonly StyledProperty<TimeSpan> DarkTimeProperty = AvaloniaProperty.Register<ChessClock, TimeSpan>(
        nameof(DarkTime));

    public TimeSpan DarkTime
    {
        get => GetValue(DarkTimeProperty);
        set => SetValue(DarkTimeProperty, value);
    }

    public static readonly StyledProperty<bool> IsRunningProperty = AvaloniaProperty.Register<ChessClock, bool>(
        nameof(IsRunning));

    public bool IsRunning
    {
        get => GetValue(IsRunningProperty);
        set => SetValue(IsRunningProperty, value);
    }
    
    public static readonly StyledProperty<bool> IsPausedProperty = AvaloniaProperty.Register<ChessClock, bool>(
        nameof(IsPaused));

    public bool IsPaused
    {
        get => GetValue(IsPausedProperty);
        set => SetValue(IsPausedProperty, value);
    }

    public static readonly StyledProperty<ICommand?> PauseCommandProperty = AvaloniaProperty.Register<ChessClock, ICommand?>(
        nameof(PauseCommand));

    public ICommand? PauseCommand
    {
        get => GetValue(PauseCommandProperty);
        set => SetValue(PauseCommandProperty, value);
    }
    
    public static readonly StyledProperty<ICommand?> TickCommandProperty = AvaloniaProperty.Register<ChessClock, ICommand?>(
        nameof(TickCommand));

    public ICommand? TickCommand
    {
        get => GetValue(TickCommandProperty);
        set => SetValue(TickCommandProperty, value);
    }
    
    public ChessClock()
    {
        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick += TimerOnTick;
    }

    private void TimerOnTick(object? sender, EventArgs e)
    {
        if (TickCommand?.CanExecute(null) == true)
            TickCommand.Execute(null);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == IsRunningProperty || change.Property == IsPausedProperty)
        {
            _pauseButton?.Content = IsPaused ? PlayIcon : PauseIcon;
            UpdateTimerState();
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        
        _pauseButton = e.NameScope.Find<Button>(PauseButtonPartName);
        _pauseButton?.Content = PlayIcon;
    }

    private void UpdateTimerState()
    {
        if (IsRunning && !IsPaused)
            _timer.Start();
        else
            _timer.Stop();
    }
}