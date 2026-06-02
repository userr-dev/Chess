using Chess.Core;
using Chess.Core.Board;
using Chess.Core.Pieces;
using static Chess.Tests.Positions;

namespace Chess.Tests.Board;

public class SquareTests
{
    private static readonly Position Position = A1;
    
    private static Square GetSquare(Position position)
    {
        return new Square(Color.Light, position);
    }
 
    [Fact]
    public void HasPiece_WhenPieceSet_ReturnTrue()
    {
        
        var piece = new Pawn(Color.Light, Position);
        var square = GetSquare(Position);
        square.Piece = piece;
 
        Assert.True(square.HasPiece);
    }
 
    [Fact]
    public void HasPiece_WhenEmpty_ReturnFalse()
    {
        
        var square = GetSquare(Position);
 
        Assert.False(square.HasPiece);
    }
 
    [Theory]
    [InlineData(Color.Light)]
    [InlineData(Color.Dark)]
    public void HasFriendlyPiece_WhenPieceIsFriendly_ReturnTrue(Color color)
    {
        
        var square = GetSquare(Position);
        square.Piece = new Pawn(color, Position);
 
        Assert.True(square.HasFriendlyPiece(color));
    }
 
    [Theory]
    [InlineData(Color.Light)]
    [InlineData(Color.Dark)]
    public void HasFriendlyPiece_WhenPieceIsEnemy_ReturnFalse(Color color)
    {
        
        var square = GetSquare(Position);
        square.Piece = new Pawn(color.Opposite(), Position);
 
        Assert.False(square.HasFriendlyPiece(color));
    }
 
    [Theory]
    [InlineData(Color.Light)]
    [InlineData(Color.Dark)]
    public void HasFriendlyPiece_WhenEmpty_ReturnFalse(Color color)
    {
        
        var square = GetSquare(Position);
 
        Assert.False(square.HasFriendlyPiece(color));
    }
 
    [Theory]
    [InlineData(Color.Light)]
    [InlineData(Color.Dark)]
    public void HasEnemyPiece_WhenPieceIsEnemy_ReturnTrue(Color color)
    {
        
        var square = GetSquare(Position);
        square.Piece = new Pawn(color.Opposite(), Position);
 
        Assert.True(square.HasEnemyPiece(color));
    }
 
    [Theory]
    [InlineData(Color.Light)]
    [InlineData(Color.Dark)]
    public void HasEnemyPiece_WhenPieceIsFriendly_ReturnFalse(Color color)
    {
        
        var square = GetSquare(Position);
        square.Piece = new Pawn(color, Position);
 
        Assert.False(square.HasEnemyPiece(color));
    }
 
    [Theory]
    [InlineData(Color.Light)]
    [InlineData(Color.Dark)]
    public void HasEnemyPiece_WhenEmpty_ReturnFalse(Color color)
    {
        
        var square = GetSquare(Position);
 
        Assert.False(square.HasEnemyPiece(color));
    }
 
    [Fact]
    public void ToString_WhenEmpty_ReturnsEmptyString()
    {
        var square = GetSquare(A1);
 
        Assert.Equal(string.Empty, square.ToString());
    }
 
    [Fact]
    public void ToString_WhenHasPiece_ReturnsPieceString()
    {
        
        var piece = new Pawn(Color.Light, Position);
        var square = GetSquare(Position);
        square.Piece = piece;
 
        Assert.Equal(piece.ToString(), square.ToString());
    }

}