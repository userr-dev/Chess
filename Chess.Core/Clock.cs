namespace Chess.Core;

public sealed class Clock
{
    private readonly Dictionary<Color, TimeSpan> _remaining;
    private readonly TimeSpan _increment;
    
    private Clock(TimeControl timeControl)
    {
        _remaining = new Dictionary<Color, TimeSpan>
        {
            { Color.Light , timeControl.Remaining},
            { Color.Dark , timeControl.Remaining}
        };
        
        _increment = timeControl.Increment;
    }

    public event Action<Color>? TimeExpired;
    public event Action<Color>? IncrementAdded;
    
    public TimeSpan GetRemaining(Color color) => _remaining[color];
    
    internal void OnMoveCompleted(Color color)
    {
        _remaining[color] += _increment;
        IncrementAdded?.Invoke(color);
    }
    
    
    public void Tick(Color color, TimeSpan elapsed)
    {
        _remaining[color] -= elapsed;
        if (_remaining[color] > TimeSpan.Zero) return;
        
        _remaining[color] = TimeSpan.Zero;
        TimeExpired?.Invoke(color);
    }
    
    public static Clock FromTimeControl(TimeControl timeControl) => new(timeControl);
}