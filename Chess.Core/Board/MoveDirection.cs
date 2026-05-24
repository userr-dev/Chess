namespace Chess.Core.Board;

public delegate bool MoveDirection(ref Position position);

public static class MoveDirectionExtensions
{
    private static readonly MoveDirection[] VerticalDirections = [Position.TryMoveDown, Position.TryMoveUp]; 
    private static readonly MoveDirection[] HorizontalDirections = [Position.TryMoveLeft, Position.TryMoveRight];
    private static readonly MoveDirection[] MainDiagonalDirections = [Position.TryMoveLeftUp, Position.TryMoveRightDown];
    private static readonly MoveDirection[] AntiDiagonalDirections = [Position.TryMoveLeftDown, Position.TryMoveRightUp];
    
    private static readonly Dictionary<MoveDirection, MoveDirection[]> AxisMap = new()
    {
        { Position.TryMoveDown, VerticalDirections },
        { Position.TryMoveUp, VerticalDirections },
        { Position.TryMoveLeft, HorizontalDirections },
        { Position.TryMoveRight, HorizontalDirections },
        { Position.TryMoveLeftUp, MainDiagonalDirections },
        { Position.TryMoveRightDown, MainDiagonalDirections },
        { Position.TryMoveLeftDown, AntiDiagonalDirections },
        { Position.TryMoveRightUp, AntiDiagonalDirections }
    };
    
    extension(MoveDirection)
    {
        public static MoveDirection[] GetAxisDirections(MoveDirection moveDirection)
        {
            return AxisMap[moveDirection];
        }
    }
}