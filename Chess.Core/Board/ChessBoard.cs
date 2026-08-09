using System.Diagnostics.CodeAnalysis;

namespace Chess.Core.Board;

public sealed class ChessBoard
{
    private const int Rows = 8, Columns = 8;
    
    private readonly Square[,] _squares;

    private readonly PieceSet _lightPieces;
    private readonly PieceSet _darkPieces;

    private readonly CheckState _lightCheckState = new(Color.Light);
    private readonly CheckState _darkCheckState = new(Color.Dark);

    public event Action? UpdatedBoardState;
    
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
    
        _lightCheckState.Update(this);
        _darkCheckState.Update(this);
    }
    
    internal bool TryGetKing(Color color, [MaybeNullWhen(false)] out King king)
    {
        var foundedKing = GetPieceSet(color).King;
        king = foundedKing;
        return foundedKing is not null;
    }

    public Position? GetKingPosition(Color color)
    {
        return TryGetKing(color, out var king) ? king.Position : null;
    }
    
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

    internal HashSet<Position> GetAttackedPositions(Color color)
    {
        return GetPieces(color)
                .SelectMany(p => p.GetAttackedPositions(this))
                .ToHashSet();
    }
    
    private void RecalculatePins(Color color)
    {
        foreach (var piece in GetPieces(color))
            piece.Unpin();

        if (!TryGetKing(color, out var king)) return;
        
        var enemyColor = color.Opposite();

        foreach (var slider in GetPieceSet(enemyColor).SlidingPieces)
            slider.DetectAndMarkPin(this, king);
    }
    
    internal void UpdateBoardState(Color movedPieceColor)
    {
        var enemyColor = movedPieceColor.Opposite();
        
        RecalculatePins(movedPieceColor);
        RecalculatePins(enemyColor);
        
        GetCheckState(movedPieceColor).Update(this);
        GetCheckState(enemyColor).Update(this);
        
        UpdatedBoardState?.Invoke();
    }

    public bool IsCheckmate(Color color)
    {
        return GetCheckState(color).IsChecked 
               && GetPieces(color).All(p => !p.GetAvailableMoves(this).HasMoves);
    }
    
    public bool IsStalemate(Color color)
    {
        return !GetCheckState(color).IsChecked 
               && GetPieces(color).All(p => !p.GetAvailableMoves(this).HasMoves);
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
        if (pawn.Color != piece.Color)
        {
            throw new ArgumentException("Cannot promote to enemy color.");
        }

        if (piece is King or Pawn)
        {
            throw new ArgumentException("Cannot promote to King or Pawn.");
        }
        
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
    
    public static ChessBoard CreateStandard()
    {
        var lightSet = StandardPiecesFactory.CreateCollection(Color.Light);
        var darkSet = StandardPiecesFactory.CreateCollection(Color.Dark);

        return new ChessBoard(lightSet, darkSet);
    }
}