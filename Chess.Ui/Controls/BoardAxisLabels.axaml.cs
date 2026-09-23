using System.Collections;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;

namespace Chess.Ui.Controls;

public class BoardAxisLabels : TemplatedControl
{
    public static readonly StyledProperty<IEnumerable> ItemsSourceProperty = AvaloniaProperty.Register<BoardAxisLabels, IEnumerable>(
        nameof(ItemsSource));

    public IEnumerable ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static readonly StyledProperty<Orientation> OrientationProperty = AvaloniaProperty.Register<BoardAxisLabels, Orientation>(
        nameof(Orientation));

    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }
}