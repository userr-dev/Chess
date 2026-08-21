namespace Chess.Core;

public readonly struct TimeControl
{
    public TimeSpan Remaining { get; }
    public TimeSpan Increment { get; }

    public TimeControl(int minutesRemaining, int increment = 0)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(minutesRemaining);
        ArgumentOutOfRangeException.ThrowIfNegative(increment);
        
        Remaining = TimeSpan.FromMinutes(minutesRemaining); 
        Increment = TimeSpan.FromSeconds(increment);
    }
    
    public override string ToString()
    {
        return Increment == TimeSpan.Zero
            ? $"{Remaining.TotalMinutes:0} min"
            : $"{Remaining.TotalMinutes:0}+{Increment.TotalSeconds:0}";
    }

    // Bullet
    public static readonly TimeControl Bullet1 = new(1);
    public static readonly TimeControl Bullet1Plus1 = new(1, 1);
    public static readonly TimeControl Bullet2Plus1 = new(2, 1);
        
    // Blitz
    public static readonly TimeControl Blitz3 = new(3);
    public static readonly TimeControl Blitz3Plus2 = new(3, 2);
    public static readonly TimeControl Blitz5 = new(5);
        
    // Rapid
    public static readonly TimeControl Rapid10 = new(10);
    public static readonly TimeControl Rapid10Plus5 = new(10, 5);
    public static readonly TimeControl Rapid15Plus10 = new(15, 10);
    
    public static readonly TimeControl[] TimeControls =
    [
        Bullet1, Bullet1Plus1, Bullet2Plus1, 
        Blitz3, Blitz3Plus2, Blitz5,
        Rapid10, Rapid10Plus5, Rapid15Plus10
    ];
}