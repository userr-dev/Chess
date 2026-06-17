namespace Chess.Tests.Board;

public class SquareTests
{
    private static Square GetSquare()
    {
        return new Square(Color.Light, A1);
    }
 
    [Fact]
    public void HasPiece_WhenPieceSet_ReturnTrue()
    {
        var square = GetSquare();
        var piece = new Pawn(Color.Light, square.Position);
        square.Piece = piece;
 
        Assert.True(square.HasPiece);
    }
 
    [Fact]
    public void HasPiece_WhenEmpty_ReturnFalse()
    {
        var square = GetSquare();
 
        Assert.False(square.HasPiece);
    }
 
    [Theory]
    [InlineData(Color.Light)]
    [InlineData(Color.Dark)]
    public void HasFriendlyPiece_WhenPieceIsFriendly_ReturnTrue(Color color)
    {
        var square = GetSquare();
        square.Piece = new Pawn(color, square.Position);
 
        Assert.True(square.HasFriendlyPiece(color));
    }
 
    [Theory]
    [InlineData(Color.Light)]
    [InlineData(Color.Dark)]
    public void HasFriendlyPiece_WhenPieceIsEnemy_ReturnFalse(Color color)
    {
        var square = GetSquare();
        square.Piece = new Pawn(color.Opposite(), square.Position);
 
        Assert.False(square.HasFriendlyPiece(color));
    }
 
    [Theory]
    [InlineData(Color.Light)]
    [InlineData(Color.Dark)]
    public void HasFriendlyPiece_WhenEmpty_ReturnFalse(Color color)
    {
        var square = GetSquare();
 
        Assert.False(square.HasFriendlyPiece(color));
    }
 
    [Theory]
    [InlineData(Color.Light)]
    [InlineData(Color.Dark)]
    public void HasEnemyPiece_WhenPieceIsEnemy_ReturnTrue(Color color)
    {
        var square = GetSquare();
        square.Piece = new Pawn(color.Opposite(), square.Position);
 
        Assert.True(square.HasEnemyPiece(color));
    }
 
    [Theory]
    [InlineData(Color.Light)]
    [InlineData(Color.Dark)]
    public void HasEnemyPiece_WhenPieceIsFriendly_ReturnFalse(Color color)
    {
        var square = GetSquare();
        square.Piece = new Pawn(color, square.Position);
 
        Assert.False(square.HasEnemyPiece(color));
    }
 
    [Theory]
    [InlineData(Color.Light)]
    [InlineData(Color.Dark)]
    public void HasEnemyPiece_WhenEmpty_ReturnFalse(Color color)
    {
        var square = GetSquare();
 
        Assert.False(square.HasEnemyPiece(color));
    }
 
    [Fact]
    public void ToString_WhenEmpty_ReturnsEmptyString()
    {
        var square = GetSquare();
 
        Assert.Equal(string.Empty, square.ToString());
    }
 
    [Fact]
    public void ToString_WhenHasPiece_ReturnsPieceString()
    {
        var square = GetSquare();
        var piece = new Pawn(Color.Light, square.Position);
        square.Piece = piece;
 
        Assert.Equal(piece.ToString(), square.ToString());
    }

}