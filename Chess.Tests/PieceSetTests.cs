namespace Chess.Tests;

public class PieceSetTests
{
    // Create
    [Fact]
    public void Create_WithPieces_ContainsAllPieces()
    {
        var pawn = new Pawn(Color.Light, A2);
        var rook = new Rook(Color.Light, A1);

        var pieceSet = PieceSet.Create(Color.Light, [pawn, rook]);

        Assert.Contains(pawn, pieceSet);
        Assert.Contains(rook, pieceSet);
    }

    [Fact]
    public void Create_Empty_ContainsNoPieces()
    {
        var pieceSet = PieceSet.Create(Color.Light, []);

        Assert.Empty(pieceSet);
    }

    [Fact]
    public void Create_WrongColorPiece_ThrowsArgumentException()
    {
        var darkPawn = new Pawn(Color.Dark, A2);

        Assert.Throws<ArgumentException>(() => PieceSet.Create(Color.Light, [darkPawn]));
    }

    // Remove
    [Fact]
    public void Remove_ExistingPiece_DoesNotContainPiece()
    {
        var pawn = new Pawn(Color.Light, A7);
        var pieceSet = PieceSet.Create(Color.Light, [pawn]);

        var board = ChessBoard.Create([pawn], []);
        
        pawn.Move(board, A8);
        Assert.DoesNotContain(pawn, board.GetPieces(pawn.Color));
    }

    // King
    [Fact]
    public void King_WhenKingAdded_ReturnsKing()
    {
        var king = new King(Color.Light, E1);
        var pieceSet = PieceSet.Create(Color.Light, [king]);

        Assert.Equal(king, pieceSet.King);
    }

    [Fact]
    public void King_WhenNoKing_ReturnsNull()
    {
        var pieceSet = PieceSet.Create(Color.Light, []);

        Assert.Null(pieceSet.King);
    }

    // CastlingRooks
    [Fact]
    public void CastlingRooks_StandardPosition_ReturnsBoth()
    {
        var rookA = new Rook(Color.Light, A1);
        var rookH = new Rook(Color.Light, H1);
        var pieceSet = PieceSet.Create(Color.Light, [rookA, rookH]);

        Assert.Equal(2, pieceSet.CastlingRooks.Count());
    }

    [Fact]
    public void CastlingRooks_NonStandardPosition_ReturnsEmpty()
    {
        var rook = new Rook(Color.Light, E4);
        var pieceSet = PieceSet.Create(Color.Light, [rook]);

        Assert.Empty(pieceSet.CastlingRooks);
    }

    [Fact]
    public void CastlingRooks_AfterRookMoved_NotIncluded()
    {
        var rook = new Rook(Color.Light, A1);
        var king = new King(Color.Light, E1);
        var lightSet = PieceSet.Create(Color.Light, [rook, king]);
        var board = ChessBoard.Create(lightSet, PieceSet.Create(Color.Dark, []));

        rook.Move(board, A3);

        Assert.DoesNotContain(rook, lightSet.CastlingRooks);
    }

    // SlidingPieces
    [Fact]
    public void SlidingPieces_RookIncluded()
    {
        var rook = new Rook(Color.Light, A1);
        var pieceSet = PieceSet.Create(Color.Light, [rook]);

        Assert.Contains(rook, pieceSet.SlidingPieces);
    }

    [Fact]
    public void SlidingPieces_BishopIncluded()
    {
        var bishop = new Bishop(Color.Light, C1);
        var pieceSet = PieceSet.Create(Color.Light, [bishop]);

        Assert.Contains(bishop, pieceSet.SlidingPieces);
    }

    [Fact]
    public void SlidingPieces_QueenIncluded()
    {
        var queen = new Queen(Color.Light, D1);
        var pieceSet = PieceSet.Create(Color.Light, [queen]);

        Assert.Contains(queen, pieceSet.SlidingPieces);
    }

    [Fact]
    public void SlidingPieces_KnightNotIncluded()
    {
        var knight = new Knight(Color.Light, B1);
        var pieceSet = PieceSet.Create(Color.Light, [knight]);

        Assert.Empty(pieceSet.SlidingPieces);
    }

    [Fact]
    public void SlidingPieces_Empty_ReturnsEmpty()
    {
        var pieceSet = PieceSet.Create(Color.Light, []);

        Assert.Empty(pieceSet.SlidingPieces);
    }
}