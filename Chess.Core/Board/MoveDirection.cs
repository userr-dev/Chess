using System.Diagnostics.CodeAnalysis;

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
    
    private static readonly Dictionary<(int ColumnSign, int RowSign), MoveDirection> DirectionMap = new()
    {
        { (0, 1), Position.TryMoveUp },
        { (0, -1), Position.TryMoveDown },
        { (-1, 0), Position.TryMoveLeft },
        { (1, 0), Position.TryMoveRight },
        { (-1, 1), Position.TryMoveLeftUp },
        { (-1, -1), Position.TryMoveLeftDown },
        { (1, 1), Position.TryMoveRightUp },
        { (1, -1), Position.TryMoveRightDown }
    };
    
    extension(MoveDirection)
    {
        public static MoveDirection[] GetAxisDirections(MoveDirection moveDirection)
        {
            return AxisMap[moveDirection];
        }
        
        public static bool TryGetDirection(int columnOffset, int rowOffset,[MaybeNullWhen(false)] out MoveDirection direction)
        {
            var isAligned = columnOffset == 0 || rowOffset == 0 || Math.Abs(columnOffset) == Math.Abs(rowOffset);

            if (isAligned && DirectionMap.TryGetValue((Math.Sign(columnOffset), Math.Sign(rowOffset)), out direction))
                return true;
            
            direction = null;
            return false;
        }
    }
}