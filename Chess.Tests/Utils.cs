namespace Chess.Tests;

public static class Utils
{
    public static ChessBoard CreateChessBoard(IEnumerable<Piece> lightPieces, IEnumerable<Piece> darkPieces)
    {
        var lightPieceSet = PieceSet.Create(Color.Light, lightPieces);
        var darkPieceSet = PieceSet.Create(Color.Dark, darkPieces);

        return ChessBoard.Create(lightPieceSet, darkPieceSet);
    }
}