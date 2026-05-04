namespace Chess.Core.Board;

public class ChessBoard
{
    private const int Rows = 8, Columns = 8;
    
    private readonly Square[,] _squares;

    private ChessBoard()
    {
        _squares = new Square[Rows, Columns];
        for (int col = 0; col < Columns; col++)
        {
            var startColor = col % 2 == 0 ? Color.Dark : Color.Light;
            GenerateSquares((Column)col, startColor);
        }
    }

    public Square this[Position position]
    {
        get => _squares[position.Row, (int)position.Column];
        set => _squares[position.Row, (int)position.Column] = value;
    }
    
    private void GenerateSquares(Column column, Color startSquareColor)
    {
        for (int row = 0; row < Rows; row++)
        {
            _squares[row, (int)column] = new Square(startSquareColor, Position.Create(column, row));
            startSquareColor = startSquareColor == Color.Dark ? Color.Light : Color.Dark;
        }
    }

    public static ChessBoard Create() => new();
}