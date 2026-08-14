using System;
using Avalonia;
using Avalonia.Controls;

namespace Chess.Ui.Controls;

public partial class ChessClock : UserControl
{

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
    
    public ChessClock()
    {
        InitializeComponent();
    }
}