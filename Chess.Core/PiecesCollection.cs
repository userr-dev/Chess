using System.Collections;
using Chess.Core.Pieces;

namespace Chess.Core;

public class PiecesCollection : IEnumerable<Piece>
{
    private readonly List<Piece> _pieces = new(16);

    public void Add(Piece piece)
    {
        _pieces.Add(piece);
    }

    public void Remove(Piece piece)
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
}