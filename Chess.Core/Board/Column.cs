namespace Chess.Core.Board;

public enum Column
{
    A,B,C,D,E,F,G,H
}

public static class ColumnExtensions
{
    extension(Column column)
    {
        public int Delta(Column other)
        {
            return (int)column - (int)other;
        }
        
        public int DistanceTo(Column other)
        {
            return Math.Abs(column.Delta(other));
        }
        
        public Column Shift(int columnOffset)
        {
            if (!Column.TryShift(column, columnOffset, out var newColumn))
            {
                throw new ArgumentOutOfRangeException(nameof(columnOffset),
                    $"Shifting column '{column}' by {columnOffset} goes out of board range.");
            }
            
            return newColumn;
        }

        public static bool TryShift(Column currentColumn, int columnOffset, out Column result)
        {
            var newColumn = (int)currentColumn + columnOffset;
            if (!Enum.IsDefined(typeof(Column), newColumn))
            {
                result = currentColumn;
                return false;
            }

            result = (Column)newColumn; 
            return true;
        }
    }
}