using Chess.Core.Board;
using Chess.Core.Pieces;

namespace Chess.Core;

public class StandardPiecesFactory
{
    public static PiecesCollection CreateCollection(Color color)
    {
        var collection = new PiecesCollection();

        var mainRow = color == Color.Light ? 0 : 7;
        var pawnRow = color == Color.Light ? 1 : 6;

        AddPawns(collection, color, pawnRow);
        AddPair(collection, color, [Column.A, Column.H], mainRow, (c, p) => new Rook(c, p));
        AddPair(collection, color, [Column.B, Column.G], mainRow, (c, p) => new Knight(c, p));
        AddPair(collection, color, [Column.C, Column.F], mainRow, (c, p) => new Bishop(c, p));
        AddRoyals(collection, color, mainRow);
        
        return collection;
    }

    private static void AddPawns(PiecesCollection piecesCollection, Color color, int row)
    {
        foreach (var column in Enum.GetValues<Column>())
        {
            piecesCollection.Add(new Pawn(color, Position.Create(column, row)));
        }
    }

    private static void AddPair(PiecesCollection piecesCollection, Color color, Column[] columns, int row,
        Func<Color, Position, Piece> factory)
    {
        foreach (var column in columns)
        {
            piecesCollection.Add(factory(color, Position.Create(column, row)));
        }
    }

    private static void AddRoyals(PiecesCollection piecesCollection, Color color, int row)
    {
        piecesCollection.Add(new Queen(color, Position.Create(Column.D, row)));
        piecesCollection.Add(new King(color, Position.Create(Column.E, row)));
    }
}