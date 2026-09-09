namespace Chess.Tests.Pieces;

public class RookTests
{
    [Fact]
    public void AnnotationSymbol()
    {
        var rook = new Rook(Color.Light, C1);
        
        Assert.Equal('R', rook.AnnotationSymbol);
    }
    
    [Fact]
    public void CanCastle_StandardPosition()
    {
        var lightRook = new Rook(Color.Light, A1);
        
        Assert.True(lightRook.CanCastle);
    }
    
    [Fact]
    public void CanCastle_NotStandardPosition()
    {
        var lightRook = new Rook(Color.Light, E4);
        
        Assert.False(lightRook.CanCastle);
    }
    
    // GetAttackedPositions
    [Fact]
    public void GetAttackedPositions_FromCenter_ReturnFourteenSquares()
    {
        var lightRook = new Rook(Color.Light, D4);
        var darkKing = new King(Color.Dark, A8);

        var board = ChessBoard.Create([lightRook], [darkKing]);

        var attackedPositions = lightRook.GetAttackedPositions(board);
        Assert.Equal(14, attackedPositions.Count());
    }
    
    [Fact]
    public void GetAttackedPositions_FromCorner_ReturnFourteenSquares()
    {
        var lightRook = new Rook(Color.Light, H1);
        var darkKing = new King(Color.Dark, A8);

        var board = ChessBoard.Create([lightRook], [darkKing]);

        var attackedPositions = lightRook.GetAttackedPositions(board);
        Assert.Equal(14, attackedPositions.Count());
    }
    
    [Fact]
    public void GetAttackedPositions_FriendlyBlocksLine_Included()
    {
        var lightRook1 = new Rook(Color.Light, D4);
        var lightRook2 = new Rook(Color.Light, D6);
        var darkKing = new King(Color.Dark, A8);

        var board = ChessBoard.Create([lightRook1, lightRook2], [darkKing]);

        var attackedPositions = lightRook1.GetAttackedPositions(board);

        Assert.Contains(attackedPositions, p => p.Equals(lightRook2.Position));
    }
    
    [Fact]
    public void GetAttackedPositions_EnemyBlocksLine_Included()
    {
        var lightRook = new Rook(Color.Light, D4);
        var darkKing = new King(Color.Dark, A8);
        var darkRook = new Rook(Color.Dark, D6);

        var board = ChessBoard.Create([lightRook], [darkKing, darkRook]);

        var attackedPositions = lightRook.GetAttackedPositions(board);

        Assert.Contains(attackedPositions, p => p.Equals(darkRook.Position));
    }
    
    [Fact]
    public void GetAttackedPositions_AlongEnemyKing_Included()
    {
        var lightRook = new Rook(Color.Light, D4);
        var darkKing = new King(Color.Dark, D6);
        
        var board = ChessBoard.Create([lightRook], [darkKing]);

        var attackedPositions = lightRook.GetAttackedPositions(board).ToList();

        Assert.Contains(attackedPositions, p => p.Equals(darkKing.Position));
        Assert.Contains(attackedPositions, p => p.Equals(D7));
        Assert.Contains(attackedPositions, p => p.Equals(D8));
    }
    
    // GetAvailableMoves
    [Fact]
    public void GetAvailableMoves_FromCenter_ReturnsFourteenSquares()
    {
        var lightRook = new Rook(Color.Light, D4);
        var darkKing = new King(Color.Dark, A8);

        var board = ChessBoard.Create([lightRook], [darkKing]);

        var moves = lightRook.GetAvailableMoves(board).Moves;
        Assert.Equal(14, moves.Count);
    }
    
    [Fact]
    public void GetAvailableMoves_FromCorner_ReturnsFourteenSquares()
    {
        var lightRook = new Rook(Color.Light, H1);
        var darkKing = new King(Color.Dark, A8);

        var board = ChessBoard.Create([lightRook], [darkKing]);

        var moves = lightRook.GetAvailableMoves(board).Moves;
        Assert.Equal(14, moves.Count);
    }
    
    [Fact]
    public void GetAvailableMoves_FriendlyBlocksLine_NotIncluded()
    {
        var lightRook1 = new Rook(Color.Light, D4);
        var lightRook2 = new Rook(Color.Light, D6);
        var darkKing = new King(Color.Dark, A8);
        
        var board = ChessBoard.Create([lightRook1, lightRook2], [darkKing]);

        var moves = lightRook1.GetAvailableMoves(board).Moves;

        Assert.DoesNotContain(moves, p => p.Equals(lightRook2.Position));
    }
    
    [Fact]
    public void GetAvailableMoves_FriendlyBlocksLine_SquaresBeyondNotIncluded()
    {
        var lightRook1 = new Rook(Color.Light, D4);
        var lightRook2 = new Rook(Color.Light, D6);
        var darkKing = new King(Color.Dark, A8);
        
        var board = ChessBoard.Create([lightRook1, lightRook2], [darkKing]);

        var moves = lightRook1.GetAvailableMoves(board).Moves;

        Assert.DoesNotContain(moves, p => p.Equals(D7));
    }
    
    [Fact]
    public void GetAvailableAttacks_EnemyOnLine_ReturnsAttackSquare()
    {
        var lightRook = new Rook(Color.Light, D4);
        var darkKing = new King(Color.Dark, A8);
        var darkRook = new Rook(Color.Dark, D6);
        
        var board = ChessBoard.Create([lightRook], [darkRook, darkKing]);

        var attacks = lightRook.GetAvailableMoves(board).Attacks;

        Assert.Contains(attacks, p => p.Equals(darkRook.Position));
    }
    
    [Fact]
    public void GetAvailableAttacks_EnemyBehindFriendly_NotIncluded()
    {
        var lightRook1 = new Rook(Color.Light, D4);
        var lightRook2 = new Rook(Color.Light, D5);
        var darkKing = new King(Color.Dark, A8);
        var darkRook = new Rook(Color.Dark, D6);
        
        var board = ChessBoard.Create([lightRook1, lightRook2], [darkRook, darkKing]);

        var attacks = lightRook1.GetAvailableMoves(board).Attacks;

        Assert.DoesNotContain(attacks, p => p.Equals(darkRook.Position));
    }
    
    [Fact]
    public void GetAvailableAttacks_FriendlyOnLine_NotIncluded()
    {
        var lightRook1 = new Rook(Color.Light, D4);
        var lightRook2 = new Rook(Color.Light, D6);
        var darkKing = new King(Color.Dark, A8);
        
        var board = ChessBoard.Create([lightRook1, lightRook2], [darkKing]);

        var attacks = lightRook1.GetAvailableMoves(board).Attacks;

        Assert.DoesNotContain(attacks, s => s.Equals(lightRook2.Position));
    }
    
    [Fact]
    public void GetAvailableAttacks_NoEnemyPiecesOnLines_ReturnsEmpty()
    {
        var lightRook = new Rook(Color.Light, D4);
        var darkKing = new King(Color.Dark, A8);
        var board = ChessBoard.Create([lightRook], [darkKing]);

        var attacks = lightRook.GetAvailableMoves(board).Attacks;

        Assert.Empty(attacks);
    }
    
    [Fact]
    public void GetAvailableMoves_PinnedAlongSameLine_CanMoveAndAttackAlongIt()
    {
        var lightKing = new King(Color.Light, E1);
        var lightRook = new Rook(Color.Light, E3);
        var darkKing = new King(Color.Dark, A8);
        var darkRook = new Rook(Color.Dark, E8);

        var board = ChessBoard.Create([lightKing, lightRook], [darkKing, darkRook]);

        var (moves, attacks) = lightRook.GetAvailableMoves(board);
        Assert.NotEmpty(moves);
        Assert.NotEmpty(attacks);
        Assert.Equal(5, moves.Count);
        Assert.Single(attacks);
    }
    
    [Fact]
    public void GetAvailableMoves_PinnedByBishop_ReturnsEmpty()
    {
        var lightKing = new King(Color.Light, E1);
        var lightRook = new Rook(Color.Light, C3);
        var darkKing = new King(Color.Dark, A8);
        var darkBishop = new Bishop(Color.Dark, A5);
        
        var board = ChessBoard.Create([lightKing, lightRook], [darkBishop, darkKing]);

        var moves = lightRook.GetAvailableMoves(board);

        Assert.Empty(moves.Moves);
        Assert.Empty(moves.Attacks);
    }
    
    // GetAvailableMoves when friendly king checked
    [Fact]
    public void GetAvailableMoves_KingInCheck_CanBlockWithRook()
    {
        var lightKing = new King(Color.Light, E1);
        var lightRook = new Rook(Color.Light, D3);
        var darkKing = new King(Color.Dark, A8);
        var darkBishop = new Bishop(Color.Dark, A5);

        var board = ChessBoard.Create([lightKing, lightRook], [darkKing, darkBishop]);

        var moves = lightRook.GetAvailableMoves(board).Moves;
        
        Assert.Equal(2, moves.Count);
    }
    
    [Fact]
    public void GetAvailableMoves_CanCaptureAttacker()
    {
        var lightKing = new King(Color.Light, E1);
        var lightRook = new Rook(Color.Light, D5);
        var darkKing = new King(Color.Dark, A8);
        var darkBishop = new Bishop(Color.Dark, A5);

        var board = ChessBoard.Create([lightKing, lightRook], [darkKing, darkBishop]);
        var checkState = board.GetCheckState(lightKing.Color);

        var attacks = lightRook.GetAvailableMoves(board).Attacks;

        Assert.Single(attacks);
        Assert.Contains(attacks, p => p == checkState.Attacker?.Position);
    }

    [Fact]
    public void GetAvailableMoves_WhenPinned_CannotBlockWithRook()
    {
        var lightKing = new King(Color.Light, E1);
        var lightRook = new Rook(Color.Light, E2);
        var darkKing = new King(Color.Dark, A8);
        var darkBishop = new Bishop(Color.Dark, A5);
        var darkRook = new Rook(Color.Dark, E8);

        var board = ChessBoard.Create([lightKing, lightRook], [darkKing, darkBishop, darkRook]);

        var moves = lightRook.GetAvailableMoves(board).Moves;

        Assert.True(lightRook.IsPinned);
        Assert.Empty(moves);
    }
    
    [Fact]
    public void GetAvailableMoves_WhenPinned_CannotCaptureAttacker()
    {
        var lightKing = new King(Color.Light, E1);
        var lightRook = new Rook(Color.Light, E5);
        var darkKing = new King(Color.Dark, A8);
        var darkBishop = new Bishop(Color.Dark, A5);
        var darkRook = new Rook(Color.Dark, E8);

        var board = ChessBoard.Create([lightKing, lightRook], [darkKing, darkBishop, darkRook]);

        var attacks = lightRook.GetAvailableMoves(board).Attacks;

        Assert.True(lightRook.IsPinned);
        Assert.Empty(attacks);
    }
    
    [Fact]
    public void GetAvailableMoves_WhenFriendlyKingIsDoubleChecked_CannotMove()
    {
        var lightKing = new King(Color.Light, E1);
        var lightRook = new Rook(Color.Light, E4);
        var darkKing = new King(Color.Dark, A8);
        var darkBishop1 = new Bishop(Color.Dark, A5);
        var darkBishop2 = new Bishop(Color.Dark, H4);

        var board = ChessBoard.Create([lightKing, lightRook], [darkKing, darkBishop1, darkBishop2]);
        var checkState = board.GetCheckState(lightKing.Color);

        var (moves, attacks) = lightRook.GetAvailableMoves(board);
        
        Assert.True(checkState.IsDoubleChecked);
        Assert.Empty(moves);
        Assert.Empty(attacks);
    }
    
    // FindPinnedPiece
    [Fact]
    public void FindPinnedPiece_PieceOnLineToKing_IsPinned()
    {
        var lightKing = new King(Color.Light, E1);
        var lightBishop = new Bishop(Color.Light, E4);
        var darkRook = new Rook(Color.Dark, E8);

        var board = ChessBoard.Create([lightKing, lightBishop], [darkRook]);
        
        Assert.True(lightBishop.IsPinned);
    }
    
    [Fact]
    public void FindPinnedPiece_FriendlyPieceBlocksRay_NothingIsPinned()
    {
        var lightKing = new King(Color.Light, E1);
        var lightBishop = new Bishop(Color.Light, E4);
        var lightRook = new Rook(Color.Light, E3);
        var darkRook = new Rook(Color.Dark, E8);

        var board = ChessBoard.Create([lightKing, lightBishop, lightRook], [darkRook]);
        
        Assert.False(lightBishop.IsPinned);
        Assert.False(lightRook.IsPinned);
    }
    
    // Move
    [Theory]
    [InlineData("E1")]
    [InlineData("E6")]
    [InlineData("C4")]
    [InlineData("H4")]
    public void Move(string positionTo)
    {
        var positionToMove = Position.Parse(positionTo);
        
        var lightRook = new Rook(Color.Light, E4);
        var darkKing = new King(Color.Dark, D8);

        var board = ChessBoard.Create([lightRook], [darkKing]);
        
        lightRook.Move(board, positionToMove);
        
        Assert.Null(board[E4].Piece);
        Assert.Equal(positionToMove, lightRook.Position);
        Assert.Equal(lightRook, board[positionToMove].Piece);
    }
    
    [Fact]
    public void Move_CaptureEnemyPiece()
    {
        var lightRook = new Rook(Color.Light, E4);
        var darkKing = new King(Color.Dark, D8);
        var darkKnight = new Knight(Color.Dark, E7);
        
        var board = ChessBoard.Create([lightRook], [darkKing, darkKnight]);
        var darkPieces = board.GetPieces(darkKnight.Color);
        
        lightRook.Move(board, darkKnight.Position);
        
        Assert.DoesNotContain(darkKnight, darkPieces);
    }
}