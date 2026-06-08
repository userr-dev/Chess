using Chess.Core.Board;
using Chess.Core.Pieces;

namespace Chess.Core;

public class StandardPiecesFactory
{
    public static PieceSet CreateCollection(Color color)
    {
        var collection = PieceSet.Create(color);

        var mainRow = color == Color.Light ? 0 : 7;
        var pawnRow = color == Color.Light ? 1 : 6;

        AddPawns(collection, color, pawnRow);
        AddPair(collection, color, [Column.A, Column.H], mainRow, (c, p) => new Rook(c, p));
        AddPair(collection, color, [Column.B, Column.G], mainRow, (c, p) => new Knight(c, p));
        AddPair(collection, color, [Column.C, Column.F], mainRow, (c, p) => new Bishop(c, p));
        AddRoyals(collection, color, mainRow);
        
        return collection;
    }

    private static void AddPawns(PieceSet pieceSet, Color color, int row)
    {
        foreach (var column in Enum.GetValues<Column>())
        {
            pieceSet.Add(new Pawn(color, Position.Create(column, row)));
        }
    }

    private static void AddPair(PieceSet pieceSet, Color color, Column[] columns, int row,
        Func<Color, Position, Piece> factory)
    {
        foreach (var column in columns)
        {
            pieceSet.Add(factory(color, Position.Create(column, row)));
        }
    }

    private static void AddRoyals(PieceSet pieceSet, Color color, int row)
    {
        pieceSet.Add(new Queen(color, Position.Create(Column.D, row)));
        pieceSet.Add(new King(color, Position.Create(Column.E, row)));
    }
}