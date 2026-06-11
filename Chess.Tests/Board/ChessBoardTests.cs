namespace Chess.Tests.Board;

public class ChessBoardTests
{
    // IsCheckmate
    [Fact]
    public void IsCheckmate_BackRankMate_ReturnsTrue()
    {
        var lightKing = new King(Color.Light, H1);
        var darkRook1 = new Rook(Color.Dark, H8);
        var darkRook2 = new Rook(Color.Dark, G7);

        var board = ChessBoard.Create(
            [lightKing],
            [darkRook1, darkRook2]);

        Assert.True(board.IsCheckmate(Color.Light));
    }

    [Fact]
    public void IsCheckmate_KingInCheck_CanEscape_ReturnsFalse()
    {
        var lightKing = new King(Color.Light, E1);
        var darkRook = new Rook(Color.Dark, E8);

        var board = ChessBoard.Create([lightKing], [darkRook]);

        Assert.False(board.IsCheckmate(Color.Light));
    }

    [Fact]
    public void IsCheckmate_KingInCheck_OtherPieceCanBlock_ReturnsFalse()
    {
        var lightKing = new King(Color.Light, E1);
        var lightRook = new Rook(Color.Light, D3);
        var darkRook = new Rook(Color.Dark, E8);

        var board = ChessBoard.Create([lightKing, lightRook], [darkRook]);

        Assert.False(board.IsCheckmate(Color.Light));
    }

    [Fact]
    public void IsCheckmate_KingInCheck_OtherPieceCanCapture_ReturnsFalse()
    {
        var lightKing = new King(Color.Light, E1);
        var lightRook = new Rook(Color.Light, A8);
        var darkRook = new Rook(Color.Dark, E8);

        var board = ChessBoard.Create([lightKing, lightRook], [darkRook]);

        Assert.False(board.IsCheckmate(Color.Light));
    }

    [Fact]
    public void IsCheckmate_NotInCheck_ReturnsFalse()
    {
        var lightKing = new King(Color.Light, E1);

        var board = ChessBoard.Create([lightKing], []);

        Assert.False(board.IsCheckmate(Color.Light));
    }

    // IsStalemate
    [Fact]
    public void IsStalemate_KingHasNoMoves_NotInCheck_ReturnsTrue()
    {
        var darkKing = new King(Color.Dark, A8);
        var lightKing = new King(Color.Light, B6);
        var lightQueen = new Queen(Color.Light, C7);

        var board = ChessBoard.Create([lightKing, lightQueen], [darkKing]);

        Assert.True(board.IsStalemate(Color.Dark));
    }

    [Fact]
    public void IsStalemate_KingInCheck_ReturnsFalse()
    {
        var lightKing = new King(Color.Light, E1);
        var darkRook = new Rook(Color.Dark, E8);

        var board = ChessBoard.Create([lightKing], [darkRook]);

        Assert.False(board.IsStalemate(Color.Light));
    }

    [Fact]
    public void IsStalemate_KingHasMoves_ReturnsFalse()
    {
        var lightKing = new King(Color.Light, E4);

        var board = ChessBoard.Create([lightKing], []);

        Assert.False(board.IsStalemate(Color.Light));
    }
}