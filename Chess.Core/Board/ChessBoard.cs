using Chess.Core.Pieces;

namespace Chess.Core.Board;

public sealed class ChessBoard
{
    private const int Rows = 8, Columns = 8;
    
    private readonly Square[,] _squares;

    private readonly PiecesCollection _lightPieces;
    private readonly PiecesCollection _darkPieces;

    private readonly CheckState _lightCheckState = new();
    private readonly CheckState _darkCheckState = new();
    
    private ChessBoard(PiecesCollection lightPieces, PiecesCollection darkPieces)
    {
        _squares = new Square[Rows, Columns];
        GenerateSquares();
        
        _lightPieces = lightPieces;
        _darkPieces = darkPieces;
        
        SetupPieces(_lightPieces);
        SetupPieces(_darkPieces);
    }
    
    public Square this[Position position] => _squares[position.Row, (int)position.Column];

    private void SetupPieces(PiecesCollection piecesCollection)
    {
        foreach (var piece in piecesCollection)
        {
            this[piece.Position].Piece = piece;
        }
    }

    private void GenerateSquares()
    {
        for (int column = 0; column < Columns; column++)
        {
            for (int row = 0; row < Rows; row++)
            {
                var color = (column + row) % 2 == 0 ? Color.Dark : Color.Light;
                _squares[row, column] = new Square(color, Position.Create((Column)column, row));
            }
        }
    }

    public IEnumerable<Piece> GetPieces(Color color)
    {
        return color == Color.Light ? _lightPieces : _darkPieces;
    }

    public CheckState GetCheckState(Color color)
    {
        return color == Color.Light ? _lightCheckState : _darkCheckState;
    }
    
    public void MovePiece(Piece piece, Position to)
    {
        this[piece.Position].Piece = null;
        this[to].Piece = piece;
        RecalculatePins(piece.Color);
        RecalculatePins(piece.Color.Opposite());
        
        GetCheckState(piece.Color.Opposite()).Update(this, piece.Color);
    }
    
    public void PromotePawn(Pawn pawn, Piece piece)
    {
        var square = this[pawn.Position];
        square.Piece = piece;

        var collection = (PiecesCollection)GetPieces(pawn.Color);
        collection.Remove(pawn);
        collection.Add(piece);
    }

    public void Castling(King king, Rook rook)
    {
        var rookNewColumn = king.Position.Column > rook.Position.Column
            ? king.Position.Column.Shift(1)
            : king.Position.Column.Shift(-1);

        var rookToPosition = Position.Create(rookNewColumn, king.Position.Row);

        rook.Move(this, rookToPosition);
    }
    
    private void RecalculatePins(Color color)
    {
        foreach (var piece in GetPieces(color))
            piece.AllowedDirections = null;

        var king = GetPieces(color).OfType<King>().FirstOrDefault();
        if (king is null) return;
        
        var enemyColor = color.Opposite();

        foreach (var slider in GetPieces(enemyColor).OfType<SlidingPiece>())
            slider.FindPinnedPiece(this, king);
    }
    
    public static ChessBoard Create(PiecesCollection lightPieces, PiecesCollection darkPieces) =>
        new(lightPieces, darkPieces);
}