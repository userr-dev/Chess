namespace Chess.Tests.Board;

public class CheckStateTests
{
    [Fact]
    public void IsChecked_ByRookOnColumn()
    {
        var lightKing = new King(Color.Light, E1);
        var darkRook = new Rook(Color.Dark, E8);

        var board = ChessBoard.Create([lightKing], [darkRook]);
        board.UpdateBoardState(darkRook.Color);
        
        var checkState = board.GetCheckState(lightKing.Color);
        
        Assert.True(checkState.IsChecked);
    }

    [Fact]
    public void IsChecked_ByBishopOnDiagonal()
    {
        var lightKing = new King(Color.Light, E1);
        var darkBishop = new Bishop(Color.Dark, A5);

        var board = ChessBoard.Create([lightKing], [darkBishop]);
        board.UpdateBoardState(darkBishop.Color);
        
        var checkState = board.GetCheckState(lightKing.Color);
        
        Assert.True(checkState.IsChecked);
    }
    
    [Fact]
    public void IsDoubleChecked_ByTwoPieces()
    {
        var lightKing = new King(Color.Light, E1);
        var darkBishop = new Bishop(Color.Dark, A5);
        var darkRook = new Rook(Color.Dark, E8);
        
        var board = ChessBoard.Create([lightKing], [darkBishop, darkRook]);
        board.UpdateBoardState(darkBishop.Color);
        
        var checkState = board.GetCheckState(lightKing.Color);
        
        Assert.True(checkState.IsDoubleChecked);
    }

    [Fact]
    public void Attackers_WhenSingleCheck_HasOneAttacker()
    {
        var lightKing = new King(Color.Light, E1);
        var darkRook = new Rook(Color.Dark, E8);

        var board = ChessBoard.Create([lightKing], [darkRook]);
        board.UpdateBoardState(darkRook.Color);
        
        var checkState = board.GetCheckState(lightKing.Color);

        Assert.Single(checkState.Attackers);
    }
    
    [Fact]
    public void Attackers_WhenDoubleCheck_HasTwoAttackers()
    {
        var lightKing = new King(Color.Light, E1);
        var darkBishop = new Bishop(Color.Dark, A5);
        var darkRook = new Rook(Color.Dark, E8);
        
        var board = ChessBoard.Create([lightKing], [darkBishop, darkRook]);
        board.UpdateBoardState(darkBishop.Color);
        
        var checkState = board.GetCheckState(lightKing.Color);
        
        Assert.Equal(2, checkState.Attackers.Count);
    }

    [Fact]
    public void BlockingPositions_WhenRookOnColumn_ReturnsInterveningSquares()
    {
        var lightKing = new King(Color.Light, E1);
        var darkRook = new Rook(Color.Dark, E8);

        var board = ChessBoard.Create([lightKing], [darkRook]);
        board.UpdateBoardState(darkRook.Color);
        
        var checkState = board.GetCheckState(lightKing.Color);
        
        Assert.Equal(6, checkState.BlockingPositions.Count);
    }
}