namespace Chess.Tests.Pieces;

public class QueenTests
{
    // GetAttackedPositions
    [Fact]
    public void GetAttackedPositions_FromCenter_ReturnsTwentySevenSquares()
    {
        var lightQueen = new Queen(Color.Light, D4);
        var darkKing = new King(Color.Dark, A8);

        var board = ChessBoard.Create([lightQueen], [darkKing]);

        var attackedPositions = lightQueen.GetAttackedPositions(board);
        
        Assert.Equal(27, attackedPositions.Count());
    }

    [Fact]
    public void GetAttackedPositions_FromCorner_ReturnsTwentyOneSquares()
    {
        var lightQueen = new Queen(Color.Light, A1);
        var darkKing = new King(Color.Dark, B8);

        var board = ChessBoard.Create([lightQueen], [darkKing]);

        var attackedPositions = lightQueen.GetAttackedPositions(board);
        
        Assert.Equal(21, attackedPositions.Count());
    }

    [Fact]
    public void GetAttackedPositions_FriendlyBlocksDiagonal_Included()
    {
        var lightQueen = new Queen(Color.Light, D4);
        var lightBishop = new Bishop(Color.Light, F6);
        var darkKing = new King(Color.Dark, A8);

        var board = ChessBoard.Create([lightQueen, lightBishop], [darkKing]);

        var attackedPositions = lightQueen.GetAttackedPositions(board);
        Assert.Contains(attackedPositions, p => p.Equals(lightBishop.Position));
    }
    
    [Fact]
    public void GetAttackedPositions_FriendlyBlocksVertical_Included()
    {
        var lightQueen = new Queen(Color.Light, D4);
        var lightBishop = new Bishop(Color.Light, D6);
        var darkKing = new King(Color.Dark, A8);

        var board = ChessBoard.Create([lightQueen, lightBishop], [darkKing]);

        var attackedPositions = lightQueen.GetAttackedPositions(board);
        Assert.Contains(attackedPositions, p => p.Equals(lightBishop.Position));
    }
    
    [Fact]
    public void GetAttackedPositions_EnemyBlocksDiagonal_Included()
    {
        var lightQueen = new Queen(Color.Light, D4);
        var darkKing = new King(Color.Dark, A8);
        var darkBishop = new Bishop(Color.Dark, F6);

        var board = ChessBoard.Create([lightQueen], [darkKing, darkBishop]);

        var attackedPositions = lightQueen.GetAttackedPositions(board);
        Assert.Contains(attackedPositions, p => p.Equals(darkBishop.Position));
    }
    
    [Fact]
    public void GetAttackedPositions_EnemyBlocksVertical_Included()
    {
        var lightQueen = new Queen(Color.Light, D4);
        var darkKing = new King(Color.Dark, A8);
        var darkBishop = new Bishop(Color.Dark, D6);

        var board = ChessBoard.Create([lightQueen], [darkKing, darkBishop]);

        var attackedPositions = lightQueen.GetAttackedPositions(board);
        Assert.Contains(attackedPositions, p => p.Equals(darkBishop.Position));
    }

    [Fact]
    public void GetAttackedPositions_AlongEnemyKingDiagonal_Included()
    {
        var lightQueen = new Queen(Color.Light, D4);
        var darkKing = new King(Color.Dark, F6);

        var board = ChessBoard.Create([lightQueen], [darkKing]);

        var attackedPositions = lightQueen.GetAttackedPositions(board).ToList();
        
        Assert.Contains(attackedPositions, p => p.Equals(darkKing.Position));
        Assert.Contains(attackedPositions, p => p.Equals(G7));
        Assert.Contains(attackedPositions, p => p.Equals(H8));
    }
    
    [Fact]
    public void GetAttackedPositions_AlongEnemyKingVertical_Included()
    {
        var lightQueen = new Queen(Color.Light, D4);
        var darkKing = new King(Color.Dark, D6);

        var board = ChessBoard.Create([lightQueen], [darkKing]);

        var attackedPositions = lightQueen.GetAttackedPositions(board).ToList();
        
        Assert.Contains(attackedPositions, p => p.Equals(darkKing.Position));
        Assert.Contains(attackedPositions, p => p.Equals(D7));
        Assert.Contains(attackedPositions, p => p.Equals(D8));
    }
    
    // GetAvailableMoves
    [Fact]
    public void GetAvailableMoves_FromCenter_ReturnTwentySevenSquares()
    {
        var lightQueen = new Queen(Color.Light, D4);
        var darkKing = new King(Color.Dark, A8);

        var board = ChessBoard.Create([lightQueen], [darkKing]);

        var moves = lightQueen.GetAvailableMoves(board).Moves;
        
        Assert.Equal(27, moves.Count);
    }
    
    [Fact]
    public void GetAvailableMoves_FromCorner_ReturnsTwentyOneSquares()
    {
        var lightQueen = new Queen(Color.Light, A1);
        var darkKing = new King(Color.Dark, B8);

        var board = ChessBoard.Create([lightQueen], [darkKing]);

        var moves = lightQueen.GetAvailableMoves(board).Moves;
        
        Assert.Equal(21, moves.Count);
    }

    [Fact]
    public void GetAvailableMoves_FriendlyBlocksDiagonal_NotIncluded()
    {
        var lightQueen = new Queen(Color.Light, D4);
        var lightBishop = new Bishop(Color.Light, F6);
        var darkKing = new King(Color.Dark, A8);

        var board = ChessBoard.Create([lightQueen, lightBishop], [darkKing]);

        var moves = lightQueen.GetAvailableMoves(board).Moves;
        
        Assert.DoesNotContain(moves, p => p.Equals(lightBishop.Position));
    }
    
    [Fact]
    public void GetAvailableMoves_FriendlyBlocksDiagonal_SquaresBeyondNotIncluded()
    {
        var lightQueen = new Queen(Color.Light, D4);
        var lightBishop = new Bishop(Color.Light, F6);
        var darkKing = new King(Color.Dark, A8);

        var board = ChessBoard.Create([lightQueen, lightBishop], [darkKing]);

        var moves = lightQueen.GetAvailableMoves(board).Moves;
        
        Assert.DoesNotContain(moves, p => p.Equals(G7));
    }
    
    [Fact]
    public void GetAvailableMoves_FriendlyBlocksVertical_NotIncluded()
    {
        var lightQueen = new Queen(Color.Light, D4);
        var lightBishop = new Bishop(Color.Light, D6);
        var darkKing = new King(Color.Dark, A8);

        var board = ChessBoard.Create([lightQueen, lightBishop], [darkKing]);

        var moves = lightQueen.GetAvailableMoves(board).Moves;
        
        Assert.DoesNotContain(moves, p => p.Equals(lightBishop.Position));
    }
    
    [Fact]
    public void GetAvailableMoves_FriendlyBlocksVertical_SquaresBeyondNotIncluded()
    {
        var lightQueen = new Queen(Color.Light, D4);
        var lightBishop = new Bishop(Color.Light, D6);
        var darkKing = new King(Color.Dark, A8);

        var board = ChessBoard.Create([lightQueen, lightBishop], [darkKing]);

        var moves = lightQueen.GetAvailableMoves(board).Moves;
        
        Assert.DoesNotContain(moves, p => p.Equals(D7));
    }

    [Fact]
    public void GetAvailableAttacks_EnemyOnDiagonal_ReturnsAttackSquare()
    {
        var lightQueen = new Queen(Color.Light, D4);
        var darkKing = new King(Color.Dark, A8);
        var darkBishop = new Bishop(Color.Dark, F6);

        var board = ChessBoard.Create([lightQueen], [darkKing, darkBishop]);

        var attacks = lightQueen.GetAvailableMoves(board).Attacks;

        Assert.Contains(attacks, p => p.Equals(darkBishop.Position));
    }
    
    [Fact]
    public void GetAvailableAttacks_EnemyOnVertical_ReturnsAttackSquare()
    {
        var lightQueen = new Queen(Color.Light, D4);
        var darkKing = new King(Color.Dark, A8);
        var darkBishop = new Bishop(Color.Dark, D6);

        var board = ChessBoard.Create([lightQueen], [darkKing, darkBishop]);

        var attacks = lightQueen.GetAvailableMoves(board).Attacks;

        Assert.Contains(attacks, p => p.Equals(darkBishop.Position));
    }

    [Fact]
    public void GetAvailableAttacks_EnemyBehindFriendlyOnDiagonal_NotIncluded()
    {
        var lightQueen = new Queen(Color.Light, D4);
        var lightBishop = new Bishop(Color.Light, E5);
        var darkKing = new King(Color.Dark, A8);
        var darkBishop = new Bishop(Color.Dark, F6);

        var board = ChessBoard.Create([lightQueen, lightBishop], [darkKing, darkBishop]);

        var attacks = lightQueen.GetAvailableMoves(board).Attacks;

        Assert.DoesNotContain(attacks, p => p.Equals(darkBishop.Position));
    }
    
    [Fact]
    public void GetAvailableAttacks_EnemyBehindFriendlyOnVertical_NotIncluded()
    {
        var lightQueen = new Queen(Color.Light, D4);
        var lightBishop = new Bishop(Color.Light, D5);
        var darkKing = new King(Color.Dark, A8);
        var darkBishop = new Bishop(Color.Dark, D6);

        var board = ChessBoard.Create([lightQueen, lightBishop], [darkKing, darkBishop]);

        var attacks = lightQueen.GetAvailableMoves(board).Attacks;

        Assert.DoesNotContain(attacks, p => p.Equals(darkBishop.Position));
    }

    [Fact]
    public void GetAvailableAttacks_FriendlyOnDiagonal_NotIncluded()
    {
        var lightQueen = new Queen(Color.Light, D4);
        var lightBishop = new Bishop(Color.Light, F6);
        var darkKing = new King(Color.Dark, A8);

        var board = ChessBoard.Create([lightQueen, lightBishop], [darkKing]);

        var attacks = lightQueen.GetAvailableMoves(board).Attacks;

        Assert.DoesNotContain(attacks, p => p.Equals(lightBishop.Position));
    }
    
    [Fact]
    public void GetAvailableAttacks_FriendlyOnVertical_NotIncluded()
    {
        var lightQueen = new Queen(Color.Light, D4);
        var lightBishop = new Bishop(Color.Light, D6);
        var darkKing = new King(Color.Dark, A8);

        var board = ChessBoard.Create([lightQueen, lightBishop], [darkKing]);

        var attacks = lightQueen.GetAvailableMoves(board).Attacks;

        Assert.DoesNotContain(attacks, p => p.Equals(lightBishop.Position));
    }

    [Fact]
    public void GetAvailableAttacks_NoEnemyPiecesOnDiagonals_ReturnsEmpty()
    {
        var lightQueen = new Queen(Color.Light, D4);
        var darkKing = new King(Color.Dark, A8);

        var board = ChessBoard.Create([lightQueen], [darkKing]);

        var attacks = lightQueen.GetAvailableMoves(board).Attacks;
        
        Assert.Empty(attacks);
    }
    
    [Fact]
    public void GetAvailableMoves_PinnedAlongSameDiagonal_CanMoveAndAttackAlongIt()
    {
        var lightKing = new King(Color.Light, A1);
        var lightQueen = new Queen(Color.Light, C3);
        var darkKing = new King(Color.Dark, B8);
        var darkQueen = new Queen(Color.Dark, F6);

        var board = ChessBoard.Create([lightKing, lightQueen], [darkKing, darkQueen]);
        board.UpdateBoardState(darkQueen.Color);
        
        var (moves, attacks) = lightQueen.GetAvailableMoves(board);

        Assert.NotEmpty(moves);
        Assert.NotEmpty(attacks);
        Assert.Equal(3, moves.Count);
        Assert.Single(attacks);
    }
    
    [Fact]
    public void GetAvailableMoves_PinnedAlongSameLine_CanMoveAndAttackAlongIt()
    {
        var lightKing = new King(Color.Light, C1);
        var lightQueen = new Queen(Color.Light, C3);
        var darkKing = new King(Color.Dark, B8);
        var darkQueen = new Queen(Color.Dark, C6);

        var board = ChessBoard.Create([lightKing, lightQueen], [darkKing, darkQueen]);
        board.UpdateBoardState(darkQueen.Color);
        
        var (moves, attacks) = lightQueen.GetAvailableMoves(board);

        Assert.NotEmpty(moves);
        Assert.NotEmpty(attacks);
        Assert.Equal(3, moves.Count);
        Assert.Single(attacks);
    }
    
    // GetAvailableMoves when friendly king checked
    [Fact]
    public void GetAvailableMoves_KingIsCheck_CanBlockWithQueen()
    {
        var lightKing = new King(Color.Light, E1);
        var lightQueen = new Queen(Color.Light, D5);
        var darkKing = new King(Color.Dark, A8);
        var darkRook = new Rook(Color.Dark, E8);

        var board = ChessBoard.Create([lightKing, lightQueen], [darkKing, darkRook]);
        board.UpdateBoardState(darkRook.Color);

        var moves = lightQueen.GetAvailableMoves(board).Moves;
        
        Assert.Equal(3, moves.Count);
    }

    [Fact]
    public void GetAvailableMoves_CanCaptureAttacker()
    {
        var lightKing = new King(Color.Light, E1);
        var lightQueen = new Queen(Color.Light, B5);
        var darkKing = new King(Color.Dark, A8);
        var darkRook = new Rook(Color.Dark, E8);

        var board = ChessBoard.Create([lightKing, lightQueen], [darkKing, darkRook]);
        board.UpdateBoardState(darkRook.Color);
        var checkState = board.GetCheckState(lightKing.Color);

        var attacks = lightQueen.GetAvailableMoves(board).Attacks;

        Assert.Single(attacks);
        Assert.Contains(attacks, p => p == checkState.Attackers[0].Position);
    }

    [Fact]
    public void GetAvailableMoves_WhenPinned_CannotBlockWithQueen()
    {
        var lightKing = new King(Color.Light, E4);
        var lightQueen = new Queen(Color.Light, D5);
        var darkKing = new King(Color.Dark, A8);
        var darkRook = new Rook(Color.Dark, E8);
        var darkBishop = new Bishop(Color.Dark, B7);
        
        var board = ChessBoard.Create([lightKing, lightQueen], [darkKing, darkRook, darkBishop]);
        board.UpdateBoardState(darkRook.Color);

        var moves = lightQueen.GetAvailableMoves(board).Moves;
        
        Assert.True(lightQueen.IsPinned);
        Assert.Empty(moves);
    }

    [Fact]
    public void GetAvailableMoves_WhenPinned_CannotCaptureAttacker()
    {
        var lightKing = new King(Color.Light, E4);
        var lightQueen = new Queen(Color.Light, D5);
        var darkKing = new King(Color.Dark, A8);
        var darkRook = new Rook(Color.Dark, E8);
        var darkBishop = new Bishop(Color.Dark, B7);
        
        var board = ChessBoard.Create([lightKing, lightQueen], [darkKing, darkRook, darkBishop]);
        board.UpdateBoardState(darkRook.Color);

        var attacks = lightQueen.GetAvailableMoves(board).Attacks;
        
        Assert.True(lightQueen.IsPinned);
        Assert.Empty(attacks);
    }

    [Fact]
    public void GetAvailableMoves_WhenFriendlyKingIsDoubleChecked_CannotMove()
    {
        var lightKing = new King(Color.Light, E4);
        var lightQueen = new Queen(Color.Light, H5);
        var darkKing = new King(Color.Dark, A8);
        var darkRook = new Rook(Color.Dark, E8);
        var darkBishop = new Bishop(Color.Dark, B7);
        
        var board = ChessBoard.Create([lightKing, lightQueen], [darkKing, darkRook, darkBishop]);
        board.UpdateBoardState(darkRook.Color);
        
        var checkState = board.GetCheckState(lightKing.Color);

        var (moves, attacks) = lightQueen.GetAvailableMoves(board);
        
        Assert.True(checkState.IsDoubleChecked);
        Assert.Empty(moves);
        Assert.Empty(attacks);
    }
    
    // FindPinnedPiece
    [Fact]
    public void FindPinnedPiece_PieceOnDiagonalLineToKing_IsPinned()
    {
        var lightKing = new King(Color.Light, H1);
        var lightRook = new Rook(Color.Light, D5);
        var darkQueen = new Queen(Color.Dark, A8);

        var board = ChessBoard.Create([lightKing, lightRook], [darkQueen]);
        board.UpdateBoardState(darkQueen.Color);
        
        Assert.True(lightRook.IsPinned);
    }
    
    [Fact]
    public void FindPinnedPiece_PieceOnVerticalLineToKing_IsPinned()
    {
        var lightKing = new King(Color.Light, E1);
        var lightBishop = new Bishop(Color.Light, E4);
        var darkQueen = new Queen(Color.Dark, E8);

        var board = ChessBoard.Create([lightKing, lightBishop], [darkQueen]);
        board.UpdateBoardState(darkQueen.Color);
        
        Assert.True(lightBishop.IsPinned);
    }
    
    [Fact]
    public void FindPinnedPiece_FriendlyPieceBlocksRayOnDiagonal_NothingIsPinned()
    {
        var lightKing = new King(Color.Light, H1);
        var lightBishop = new Bishop(Color.Light, F3);
        var lightRook = new Rook(Color.Light, E4);
        var darkQueen = new Queen(Color.Dark, A8);

        var board = ChessBoard.Create([lightKing, lightBishop, lightRook], [darkQueen]);
        board.UpdateBoardState(darkQueen.Color);
        
        Assert.False(lightBishop.IsPinned);
        Assert.False(lightRook.IsPinned);
    }
    
    [Fact]
    public void FindPinnedPiece_FriendlyPieceBlocksRayOnVertical_NothingIsPinned()
    {
        var lightKing = new King(Color.Light, E1);
        var lightBishop = new Bishop(Color.Light, E4);
        var lightRook = new Rook(Color.Light, E3);
        var darkQueen = new Queen(Color.Dark, E8);

        var board = ChessBoard.Create([lightKing, lightBishop, lightRook], [darkQueen]);
        board.UpdateBoardState(darkQueen.Color);
        
        Assert.False(lightBishop.IsPinned);
        Assert.False(lightRook.IsPinned);
    }
    
    // Move
    [Theory]
    [InlineData("E1")]
    [InlineData("E6")]
    [InlineData("B4")]
    [InlineData("G3")]
    [InlineData("C2")]
    [InlineData("B7")]
    [InlineData("G6")]
    [InlineData("H1")]
    public void Move(string positionTo)
    {
        var positionToMove = Position.Parse(positionTo);
        
        var lightQueen = new Queen(Color.Light, E4);
        var darkKing = new King(Color.Dark, D8);
        
        var board = ChessBoard.Create([lightQueen], [darkKing]);
        
        lightQueen.Move(board, positionToMove);
        
        Assert.Null(board[E4].Piece);
        Assert.Equal(positionToMove, lightQueen.Position);
        Assert.Equal(lightQueen, board[positionToMove].Piece);
    }
}