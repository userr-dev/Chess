using Chess.Core.Board;

namespace Chess.Tests.Board;

public class MoveDirectionTests
{
    private static readonly MoveDirection[] VerticalDirections = [Position.TryMoveDown, Position.TryMoveUp]; 
    private static readonly MoveDirection[] HorizontalDirections = [Position.TryMoveLeft, Position.TryMoveRight];
    private static readonly MoveDirection[] MainDiagonalDirections = [Position.TryMoveLeftUp, Position.TryMoveRightDown];
    private static readonly MoveDirection[] AntiDiagonalDirections = [Position.TryMoveLeftDown, Position.TryMoveRightUp];
    
    private static MoveDirection GetDirection(string name) => name switch
    {
        nameof(Position.TryMoveUp) => Position.TryMoveUp,
        nameof(Position.TryMoveDown) => Position.TryMoveDown,
        nameof(Position.TryMoveLeft) => Position.TryMoveLeft,
        nameof(Position.TryMoveRight) => Position.TryMoveRight,
        nameof(Position.TryMoveLeftUp) => Position.TryMoveLeftUp,
        nameof(Position.TryMoveLeftDown) => Position.TryMoveLeftDown,
        nameof(Position.TryMoveRightUp) => Position.TryMoveRightUp,
        nameof(Position.TryMoveRightDown) => Position.TryMoveRightDown,
        _ => throw new ArgumentException($"Unknown direction: {name}")
    };

    [Theory]
    [InlineData(nameof(Position.TryMoveUp))]
    [InlineData(nameof(Position.TryMoveDown))]
    public void GetAxisDirections_VerticalDirection_ReturnsVerticalAxis(string directionName)
    {
        var direction = GetDirection(directionName);
        var result = MoveDirection.GetAxisDirections(direction);
        Assert.Equal(VerticalDirections, result);
    }
 
    [Theory]
    [InlineData(nameof(Position.TryMoveLeft))]
    [InlineData(nameof(Position.TryMoveRight))]
    public void GetAxisDirections_HorizontalDirection_ReturnsHorizontalAxis(string directionName)
    {
        var direction = GetDirection(directionName);
        var result = MoveDirection.GetAxisDirections(direction);
        Assert.Equal(HorizontalDirections, result);
    }
 
    [Theory]
    [InlineData(nameof(Position.TryMoveLeftUp))]
    [InlineData(nameof(Position.TryMoveRightDown))]
    public void GetAxisDirections_MainDiagonalDirection_ReturnsMainDiagonalAxis(string directionName)
    {
        var direction = GetDirection(directionName);
        var result = MoveDirection.GetAxisDirections(direction);
        Assert.Equal(MainDiagonalDirections, result);
    }
 
    [Theory]
    [InlineData(nameof(Position.TryMoveLeftDown))]
    [InlineData(nameof(Position.TryMoveRightUp))]
    public void GetAxisDirections_AntiDiagonalDirection_ReturnsAntiDiagonalAxis(string directionName)
    {
        var direction = GetDirection(directionName);
        var result = MoveDirection.GetAxisDirections(direction);
        Assert.Equal(AntiDiagonalDirections, result);
    }
}