namespace Chess.Tests.Pieces;

public class KnightTests
{
    // GetAttackedPositions
    [Fact]
    public void GetAttackedPositions_FromCenter_ReturnsEightSquares()
    {
        var lightKnight = new Knight(Color.Light, D4);

        var board = ChessBoard.Create([lightKnight], []);

        var attackedPositions = lightKnight.GetAttackedPositions(board);
        Assert.Equal(8, attackedPositions.Count());
    }

    [Fact]
    public void GetAttackedPositions_FromCorner_ReturnsTwoSquares()
    {
        var lightKnight = new Knight(Color.Light, A1);

        var board = ChessBoard.Create([lightKnight], []);

        var attackedPositions = lightKnight.GetAttackedPositions(board);
        Assert.Equal(2, attackedPositions.Count());
    }

    [Fact]
    public void GetAttackedPositions_FromMiddleOnEdge_ReturnsFourSquares()
    {
        var lightKnight = new Knight(Color.Light, A5);

        var board = ChessBoard.Create([lightKnight], []);

        var attackedPositions = lightKnight.GetAttackedPositions(board);
        Assert.Equal(4, attackedPositions.Count());
    }

    [Fact]
    public void GetAttackedPositions_FriendlyOnWays_Included()
    {
        var lightKnight = new Knight(Color.Light, D4);
        var lightBishop = new Bishop(Color.Light, F5);
        
        var board = ChessBoard.Create([lightKnight, lightBishop], []);

        var attackedPositions = lightKnight.GetAttackedPositions(board);
        Assert.Equal(8, attackedPositions.Count());
    }
    
    [Fact]
    public void GetAttackedPositions_EnemyOnWays_Included()
    {
        var lightKnight = new Knight(Color.Light, D4);
        var darkBishop = new Bishop(Color.Dark, F5);
        
        var board = ChessBoard.Create([lightKnight], [darkBishop]);

        var attackedPositions = lightKnight.GetAttackedPositions(board);
        Assert.Equal(8, attackedPositions.Count());
    }
    
    // GetAvailableMoves
    [Fact]
    public void GetAvailableMoves_FromCenter_ReturnsEightSquares()
    {
        var lightKnight = new Knight(Color.Light, D4);

        var board = ChessBoard.Create([lightKnight], []);

        var moves = lightKnight.GetAvailableMoves(board).Moves;
        Assert.Equal(8, moves.Count);
    }
    
    [Fact]
    public void GetAvailableMoves_FromCorner_ReturnsEightSquares()
    {
        var lightKnight = new Knight(Color.Light, A1);

        var board = ChessBoard.Create([lightKnight], []);

        var moves = lightKnight.GetAvailableMoves(board).Moves;
        Assert.Equal(2, moves.Count);
    }

    [Fact]
    public void GetAvailableMoves_FromMiddleOnEdge_ReturnsEightSquares()
    {
        var lightKnight = new Knight(Color.Light, A5);

        var board = ChessBoard.Create([lightKnight], []);

        var moves = lightKnight.GetAvailableMoves(board).Moves;
        Assert.Equal(4, moves.Count);
    }

    [Fact]
    public void GetAvailableMoves_FriendlyBlocksMove_NotIncluded()
    {
        var lightKnight = new Knight(Color.Light, D4);
        var lightPawn = new Pawn(Color.Light, F5);
        
        var board = ChessBoard.Create([lightKnight, lightPawn], []);

        var moves = lightKnight.GetAvailableMoves(board).Moves;
        Assert.DoesNotContain(moves, p => p.Equals(lightPawn.Position));
    }

    [Fact]
    public void GetAvailableAttacks_EnemyOnWay_ReturnsAttackSquare()
    {
        var lightKnight = new Knight(Color.Light, D4);
        var darkPawn = new Pawn(Color.Dark, F5);
        
        var board = ChessBoard.Create([lightKnight], [darkPawn]);

        var attacks = lightKnight.GetAvailableMoves(board).Attacks;
        Assert.Contains(attacks, p => p.Equals(darkPawn.Position));
    }
    
    // GetAvailableMoves when friendly king checked
    [Fact]
    public void GetAvailableMoves_KingInCheck_CanBlockWithKnight()
    {
        var lightKing = new King(Color.Light, E1);
        var lightKnight = new Knight(Color.Light, D4);
        var darkRook = new Rook(Color.Dark, E8);

        var board = ChessBoard.Create([lightKing, lightKnight], [darkRook]);

        var moves = lightKnight.GetAvailableMoves(board).Moves;
        
        Assert.Equal(2, moves.Count);
    }

    [Fact]
    public void GetAvailableMoves_CanCaptureAttacker()
    {
        var lightKing = new King(Color.Light, E1);
        var lightKnight = new Knight(Color.Light, D6);
        var darkRook = new Rook(Color.Dark, E8);

        var board = ChessBoard.Create([lightKing, lightKnight], [darkRook]);
        var checkState = board.GetCheckState(lightKing.Color);

        var attacks = lightKnight.GetAvailableMoves(board).Attacks;

        Assert.Single(attacks);
        Assert.Contains(attacks, p => p == checkState.Attackers[0].Position);
    }

    [Fact]
    public void GetAvailableMoves_WhenPinned_CannotBlocksWithKnight()
    {
        var lightKing = new King(Color.Light, E1);
        var lightKnight = new Knight(Color.Light, C3);
        var darkRook = new Rook(Color.Dark, E8);
        var darkBishop = new Bishop(Color.Dark, A5);
        
        var board = ChessBoard.Create([lightKing, lightKnight], [darkRook, darkBishop]);

        var moves = lightKnight.GetAvailableMoves(board).Moves;
        
        Assert.True(lightKnight.IsPinned);
        Assert.Empty(moves);
    }

    [Fact]
    public void GetAvailableMoves_WhenPinned_CannotCaptureAttacker()
    {
        var lightKing = new King(Color.Light, E1);
        var lightKnight = new Knight(Color.Light, C3);
        var darkRook = new Rook(Color.Dark, E4);
        var darkBishop = new Bishop(Color.Dark, A5);
        
        var board = ChessBoard.Create([lightKing, lightKnight], [darkRook, darkBishop]);

        var attacks = lightKnight.GetAvailableMoves(board).Attacks;
        
        Assert.True(lightKnight.IsPinned);
        Assert.Empty(attacks);
    }
    
    [Fact]
    public void GetAvailableMoves_WhenFriendlyKingIsDoubleChecked_CannotMove()
    {
        var lightKing = new King(Color.Light, E1);
        var lightKnight = new Knight(Color.Light, C3);
        var darkRook = new Rook(Color.Dark, E4);
        var darkBishop = new Bishop(Color.Dark, H4);
        
        var board = ChessBoard.Create([lightKing, lightKnight], [darkRook, darkBishop]);
        
        var checkState = board.GetCheckState(lightKing.Color);

        var (moves, attacks) = lightKnight.GetAvailableMoves(board);
        
        Assert.True(checkState.IsDoubleChecked);
        Assert.Empty(moves);
        Assert.Empty(attacks);
    }

    [Fact]
    public void GetAvailableMoves_WhenPinned_CannotMove()
    {
        var lightKing = new King(Color.Light, E1);
        var lightKnight = new Knight(Color.Light, E4);
        var darkRook = new Rook(Color.Dark, E8);
        
        var board = ChessBoard.Create([lightKing, lightKnight], [darkRook]);

        var moves = lightKnight.GetAvailableMoves(board).Moves;
        
        Assert.True(lightKnight.IsPinned);
        Assert.Empty(moves);
    }
    
    // Move
    [Theory]
    [InlineData("D2")]
    [InlineData("F2")]
    [InlineData("C3")]
    [InlineData("G3")]
    [InlineData("C5")]
    [InlineData("G5")]
    [InlineData("D6")]
    [InlineData("F6")]
    public void Move(string positionTo)
    {
        var positionToMove = Position.Parse(positionTo);
        
        var lightKnight = new Knight(Color.Light, E4);

        var board = ChessBoard.Create([lightKnight], []);
        
        lightKnight.Move(board, positionToMove);
        
        Assert.Null(board[E4].Piece);
        Assert.Equal(positionToMove, lightKnight.Position);
        Assert.Equal(lightKnight, board[positionToMove].Piece);
    }
}