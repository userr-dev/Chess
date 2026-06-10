using Chess.Core.Pieces;

namespace Chess.Core.Board;

public sealed class ChessBoard
{
    private const int Rows = 8, Columns = 8;
    
    private readonly Square[,] _squares;

    private readonly PieceSet _lightPieces;
    private readonly PieceSet _darkPieces;

    private readonly CheckState _lightCheckState = new();
    private readonly CheckState _darkCheckState = new();
    
    private ChessBoard(PieceSet lightPieces, PieceSet darkPieces)
    {
        _squares = new Square[Rows, Columns];
        GenerateSquares();
        
        _lightPieces = lightPieces;
        _darkPieces = darkPieces;
        
        SetupPieces(_lightPieces);
        SetupPieces(_darkPieces);
        
        InitializeBoardState();
    }
    
    public Square this[Position position] => _squares[position.Row, (int)position.Column];

    private void SetupPieces(PieceSet pieceSet)
    {
        foreach (var piece in pieceSet)
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

    private void InitializeBoardState()
    {
        RecalculatePins(Color.Light);
        RecalculatePins(Color.Dark);
    
        _lightCheckState.Update(this, Color.Dark);
        _darkCheckState.Update(this, Color.Light);
    }
    
    internal King? GetKing(Color color) => GetPieceSet(color).King;

    internal IEnumerable<Rook> GetCastlingRooks(Color color) => GetPieceSet(color).CastlingRooks;
    
    public IEnumerable<Piece> GetPieces(Color color)
    {
        return GetPieceSet(color);
    }

    private PieceSet GetPieceSet(Color color)
    {
        return color == Color.Light ? _lightPieces : _darkPieces;
    }

    public CheckState GetCheckState(Color color)
    {
        return color == Color.Light ? _lightCheckState : _darkCheckState;
    }
    
    private void RecalculatePins(Color color)
    {
        foreach (var piece in GetPieces(color))
            piece.AllowedDirections = null;

        var king = GetKing(color);
        if (king is null) return;
        
        var enemyColor = color.Opposite();

        foreach (var slider in GetPieceSet(enemyColor).SlidingPieces)
            slider.FindPinnedPiece(this, king);
    }
    
    internal void UpdateBoardState(Color movedPieceColor)
    {
        var enemyColor = movedPieceColor.Opposite();
        
        RecalculatePins(movedPieceColor);
        RecalculatePins(enemyColor);
        
        GetCheckState(enemyColor).Update(this, movedPieceColor);
    }
    
    // Moves
    internal void MovePiece(Piece movedPiece, Position to)
    {
        this[movedPiece.Position].Piece = null;
        this[to].Piece = movedPiece;
    }

    internal void CapturePiece(Piece piece)
    {
        this[piece.Position].Piece = null;
        var enemyPieceSet = GetPieceSet(piece.Color);
        enemyPieceSet.Remove(piece);
    }
    
    internal void PromotePawn(Pawn pawn, Piece piece)
    {
        var square = this[pawn.Position];
        square.Piece = piece;

        var pieceSet = GetPieceSet(pawn.Color);
        pieceSet.Remove(pawn);
        pieceSet.Add(piece);
        
        UpdateBoardState(pawn.Color);
    }

    internal void Castle(King king, Position from, Position to)
    {
        var isKingSideRook = to.Column > from.Column;
        
        var rook = GetCastlingRooks(king.Color).First(r =>
            isKingSideRook
                ? r.Position.Column > from.Column
                : r.Position.Column < from.Column);
        var rookNewColumn = king.Position.Column.Shift(isKingSideRook ? -1 : 1);

        var rookToPosition = Position.Create(rookNewColumn, king.Position.Row);

        rook.Move(this, rookToPosition);
    }
    
    public static ChessBoard Create(IEnumerable<Piece> lightPieces, IEnumerable<Piece> darkPieces)
    {
        var lightSet = PieceSet.Create(Color.Light, lightPieces);
        var darkSet = PieceSet.Create(Color.Dark, darkPieces);

        return new ChessBoard(lightSet, darkSet);
    }
}