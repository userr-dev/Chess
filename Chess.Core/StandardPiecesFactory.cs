namespace Chess.Core;

public class StandardPiecesFactory
{
    private static readonly Column[] RookColumns = [Column.A, Column.H];
    private static readonly Column[] KnightColumns = [Column.B, Column.G];
    private static readonly Column[] BishopColumns = [Column.C, Column.F];
    
    public static PieceSet CreateCollection(Color color)
    {
        var collection = PieceSet.Create(color);

        var mainRow = color == Color.Light ? 0 : 7;
        var pawnRow = color == Color.Light ? 1 : 6;

        AddPawns(collection, color, pawnRow);
        AddPair(collection, color, RookColumns, mainRow, (c, p) => new Rook(c, p));
        AddPair(collection, color, KnightColumns, mainRow, (c, p) => new Knight(c, p));
        AddPair(collection, color, BishopColumns, mainRow, (c, p) => new Bishop(c, p));
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