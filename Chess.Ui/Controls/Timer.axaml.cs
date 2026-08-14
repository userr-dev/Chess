using System;
using Avalonia;
using Avalonia.Controls.Primitives;

namespace Chess.Ui.Controls;

public class Timer : TemplatedControl
{
    public static readonly StyledProperty<TimeSpan> ValueProperty = AvaloniaProperty.Register<Timer, TimeSpan>(
        nameof(Value));

    public TimeSpan Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
}