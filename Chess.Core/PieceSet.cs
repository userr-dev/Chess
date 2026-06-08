using System.Collections;
using Chess.Core.Pieces;

namespace Chess.Core;

public class PieceSet : IEnumerable<Piece>
{
    private const int MaxPiecesCount = 16;
    private readonly HashSet<Piece> _pieces = new(MaxPiecesCount);
    private readonly Color _color;
    
    public King? King { get; private set; }
    public IEnumerable<Rook> CastlingRooks => _pieces.OfType<Rook>().Where(r => r.CanCastle);
    public IEnumerable<SlidingPiece> SlidingPieces => _pieces.OfType<SlidingPiece>();
    
    private PieceSet(Color color)
    {
        _color = color;
    }
    
    internal void Add(Piece piece)
    {
        if (piece.Color != _color) 
            throw new ArgumentException($"Cannot add {piece.Color} piece to {_color} set.");
        
        var isAdded = _pieces.Add(piece);
        
        if (isAdded && piece is King king)
        {
            King = king;
        }
    }

    internal void Remove(Piece piece)
    {
        _pieces.Remove(piece);
    }
    
    public IEnumerator<Piece> GetEnumerator()
    {
        return _pieces.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public static PieceSet Create(Color color, params IEnumerable<Piece> pieces)
    {
        var pieceSet = new PieceSet(color);

        foreach (var piece in pieces)
        {
            pieceSet.Add(piece);
        }
        
        return pieceSet;
    }
}