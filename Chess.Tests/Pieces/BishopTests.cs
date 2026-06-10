namespace Chess.Tests.Pieces;

public class BishopTests
{
    // GetAttackedPositions    
    [Fact]
    public void GetAttackedPositions_FromCenter_ReturnsThirteenSquares()
    {
        var lightBishop = new Bishop(Color.Light, D4);
        var darkKing = new King(Color.Dark, A8);
        
        var board = ChessBoard.Create([lightBishop], [darkKing]);

        var attackedPositions = lightBishop.GetAttackedPositions(board);
        Assert.Equal(13, attackedPositions.Count());
    }
    
    [Fact]
    public void GetAttackedPositions_FromCorner_ReturnsSevenSquares()
    {
        var lightBishop = new Bishop(Color.Light, A1);
        var darkKing = new King(Color.Dark, A8);
        
        var board = ChessBoard.Create([lightBishop], [darkKing]);

        var attackedPositions = lightBishop.GetAttackedPositions(board);

        Assert.Equal(7, attackedPositions.Count());
    }

    [Fact]
    public void GetAttackedPositions_FriendlyBlocksDiagonal_Included()
    {
        var lightBishop1 = new Bishop(Color.Light, D4);
        var lightBishop2 = new Bishop(Color.Light, F6);
        var darkKing = new King(Color.Dark, A8);
        
        var board = ChessBoard.Create([lightBishop1, lightBishop2], [darkKing]);

        var attackedPositions = lightBishop1.GetAttackedPositions(board);

        Assert.Contains(attackedPositions, p => p.Equals(lightBishop2.Position));
    }
    
    [Fact]
    public void GetAttackedPositions_EnemyBlocksDiagonal_Included()
    {
        var lightBishop = new Bishop(Color.Light, D4);
        var darkBishop = new Bishop(Color.Dark, F6);
        var darkKing = new King(Color.Dark, A8);
        
        var board = ChessBoard.Create([lightBishop], [darkKing, darkBishop]);

        var attackedPositions = lightBishop.GetAttackedPositions(board);

        Assert.Contains(attackedPositions, p => p.Equals(darkBishop.Position));
    }

    [Fact]
    public void GetAttackedPositions_AlongEnemyKing_Included()
    {
        var lightBishop = new Bishop(Color.Light, D4);
        var darkKing = new King(Color.Dark, F6);
        
        var board = ChessBoard.Create([lightBishop], [darkKing]);

        var attackedPositions = lightBishop.GetAttackedPositions(board).ToList();

        Assert.Contains(attackedPositions, p => p.Equals(darkKing.Position));
        Assert.Contains(attackedPositions, p => p.Equals(G7));
        Assert.Contains(attackedPositions, p => p.Equals(H8));
    }
    
    // GetAvailableMoves
    [Fact]
    public void GetAvailableMoves_FromCenter_ReturnsThirteenSquares()
    {
        var lightBishop = new Bishop(Color.Light, D4);
        var darkKing = new King(Color.Dark, A8);
        
        var board = ChessBoard.Create([lightBishop], [darkKing]);

        var moves = lightBishop.GetAvailableMoves(board).Moves;

        Assert.Equal(13, moves.Count);
    }

    [Fact]
    public void GetAvailableMoves_FromCorner_ReturnsSevenSquares()
    {
        var lightBishop = new Bishop(Color.Light, A1);
        var darkKing = new King(Color.Dark, A8);
        
        var board = ChessBoard.Create([lightBishop], [darkKing]);

        var moves = lightBishop.GetAvailableMoves(board).Moves;

        Assert.Equal(7, moves.Count);
    }

    [Fact]
    public void GetAvailableMoves_FriendlyBlocksDiagonal_NotIncluded()
    {
        var lightBishop1 = new Bishop(Color.Light, D4);
        var lightBishop2 = new Bishop(Color.Light, F6);
        var darkKing = new King(Color.Dark, A8);
        
        var board = ChessBoard.Create([lightBishop1, lightBishop2], [darkKing]);

        var moves = lightBishop1.GetAvailableMoves(board).Moves;

        Assert.DoesNotContain(moves, p => p.Equals(lightBishop2.Position));
    }

    [Fact]
    public void GetAvailableMoves_FriendlyBlocksDiagonal_SquaresBeyondNotIncluded()
    {
        var lightBishop1 = new Bishop(Color.Light, D4);
        var lightBishop2 = new Bishop(Color.Light, F6);
        var darkKing = new King(Color.Dark, A8);
        
        var board = ChessBoard.Create([lightBishop1, lightBishop2], [darkKing]);

        var moves = lightBishop1.GetAvailableMoves(board).Moves;

        Assert.DoesNotContain(moves, p => p.Equals(G7));
    }

    [Fact]
    public void GetAvailableAttacks_EnemyOnDiagonal_ReturnsAttackSquare()
    {
        var lightBishop = new Bishop(Color.Light, D4);
        var darkKing = new King(Color.Dark, A8);
        var darkBishop = new Bishop(Color.Dark, F6);
        
        var board = ChessBoard.Create([lightBishop], [darkBishop, darkKing]);

        var attacks = lightBishop.GetAvailableMoves(board).Attacks;

        Assert.Contains(attacks, p => p.Equals(darkBishop.Position));
    }

    [Fact]
    public void GetAvailableAttacks_EnemyBehindFriendly_NotIncluded()
    {
        var lightBishop1 = new Bishop(Color.Light, D4);
        var lightBishop2 = new Bishop(Color.Light, E5);
        var darkKing = new King(Color.Dark, A8);
        var darkBishop = new Bishop(Color.Dark, F6);
        
        var board = ChessBoard.Create([lightBishop1, lightBishop2], [darkBishop, darkKing]);

        var attacks = lightBishop1.GetAvailableMoves(board).Attacks;

        Assert.DoesNotContain(attacks, p => p.Equals(darkBishop.Position));
    }

    [Fact]
    public void GetAvailableAttacks_FriendlyOnDiagonal_NotIncluded()
    {
        var lightBishop1 = new Bishop(Color.Light, D4);
        var lightBishop2 = new Bishop(Color.Light, F6);
        var darkKing = new King(Color.Dark, A8);
        
        var board = ChessBoard.Create([lightBishop1, lightBishop2], [darkKing]);

        var attacks = lightBishop1.GetAvailableMoves(board).Attacks;

        Assert.DoesNotContain(attacks, s => s.Equals(lightBishop2.Position));
    }

    [Fact]
    public void GetAvailableAttacks_NoEnemyPiecesOnDiagonals_ReturnsEmpty()
    {
        var lightBishop = new Bishop(Color.Light, D4);
        var darkKing = new King(Color.Dark, A8);
        var board = ChessBoard.Create([lightBishop], [darkKing]);

        var attacks = lightBishop.GetAvailableMoves(board).Attacks;

        Assert.Empty(attacks);
    }

    [Fact]
    public void GetAvailableMoves_PinnedAlongSameDiagonal_CanMoveAndAttackAlongIt()
    {
        var lightKing = new King(Color.Light, A1);
        var lightBishop = new Bishop(Color.Light, C3);
        var darkKing = new King(Color.Dark, B8);
        var darkQueen = new Queen(Color.Dark, F6);

        var board = ChessBoard.Create([lightKing, lightBishop], [darkKing, darkQueen]);

        var (moves, attacks) = lightBishop.GetAvailableMoves(board);

        Assert.NotEmpty(moves);
        Assert.NotEmpty(attacks);
        Assert.Equal(3, moves.Count);
        Assert.Single(attacks);
    }
    
    [Fact]
    public void GetAvailableMoves_PinnedByRookVertically_ReturnsEmpty()
    {
        var lightKing = new King(Color.Light, E1);
        var lightBishop = new Bishop(Color.Light, E4);
        var darkKing = new King(Color.Dark, A8);
        var darkRook = new Rook(Color.Dark, E8);
        
        var board = ChessBoard.Create([lightKing, lightBishop], [darkRook, darkKing]);

        var moves = lightBishop.GetAvailableMoves(board);

        Assert.Empty(moves.Moves);
        Assert.Empty(moves.Attacks);
    }
    
    // GetAvailableMoves when friendly king checked

    [Fact]
    public void GetAvailableMoves_KingInCheck_CanBlockWithBishop()
    {
        var lightKing = new King(Color.Light, E1);
        var lightBishop = new Bishop(Color.Light, D5);
        var darkKing = new King(Color.Dark, B8);
        var darkRook = new Rook(Color.Dark, E8);

        var board = ChessBoard.Create([lightKing, lightBishop], [darkKing, darkRook]);

        var moves = lightBishop.GetAvailableMoves(board).Moves;
        
        Assert.Equal(2, moves.Count);
    }

    [Fact]
    public void GetAvailableMoves_CanCaptureAttacker()
    {
        var lightKing = new King(Color.Light, E1);
        var lightBishop = new Bishop(Color.Light, B5);
        var darkKing = new King(Color.Dark, B8);
        var darkRook = new Rook(Color.Dark, E8);

        var board = ChessBoard.Create([lightKing, lightBishop], [darkKing, darkRook]);
        var checkState = board.GetCheckState(lightKing.Color);

        var attacks = lightBishop.GetAvailableMoves(board).Attacks;
        
        Assert.Single(attacks);
        Assert.Contains(attacks, p => p == checkState.Attackers[0].Position);
    }

    [Fact]
    public void GetAvailableMoves_WhenPinned_CannotBlocksWithBishop()
    {
        var lightKing = new King(Color.Light, E3);
        var lightBishop = new Bishop(Color.Light, C5);
        var darkKing = new King(Color.Dark, A8);
        var darkRook = new Rook(Color.Dark, E8);
        var darkBishop = new Bishop(Color.Dark, A7);
        
        var board = ChessBoard.Create([lightKing, lightBishop], [darkKing, darkRook, darkBishop]);

        var moves = lightBishop.GetAvailableMoves(board).Moves;
        
        Assert.True(lightBishop.IsPinned);
        Assert.Empty(moves);
    }
    
    [Fact]
    public void GetAvailableMoves_WhenPinned_CannotCaptureAttacker()
    {
        var lightKing = new King(Color.Light, E2);
        var lightBishop = new Bishop(Color.Light, B5);
        var darkKing = new King(Color.Dark, B8);
        var darkRook = new Rook(Color.Dark, E8);
        var darkBishop = new Bishop(Color.Dark, A6);
        
        var board = ChessBoard.Create([lightKing, lightBishop], [darkKing, darkRook, darkBishop]);

        var attacks = lightBishop.GetAvailableMoves(board).Attacks;
        
        Assert.True(lightBishop.IsPinned);
        Assert.Empty(attacks);
    }

    [Fact]
    public void GetAvailableMoves_WhenFriendlyKingIsDoubleChecked_CannotMove()
    {
        var lightKing = new King(Color.Light, E1);
        var lightBishop = new Bishop(Color.Light, B5);
        var darkKing = new King(Color.Dark, B8);
        var darkRook = new Rook(Color.Dark, E8);
        var darkBishop = new Bishop(Color.Dark, A5);
        
        var board = ChessBoard.Create([lightKing, lightBishop], [darkKing, darkRook, darkBishop]);
        var checkState = board.GetCheckState(lightKing.Color);

        var (moves, attacks) = lightBishop.GetAvailableMoves(board);
        
        Assert.True(checkState.IsDoubleChecked);
        Assert.Empty(moves);
        Assert.Empty(attacks);
    }
    
    // FindPinnedPiece
    [Fact]
    public void FindPinnedPiece_PieceOnLineToKing_IsPinned()
    {
        var lightKing = new King(Color.Light, H1);
        var lightRook = new Rook(Color.Light, D5);
        var darkBishop = new Bishop(Color.Dark, A8);

        var board = ChessBoard.Create([lightKing, lightRook], [darkBishop]);
        
        Assert.True(lightRook.IsPinned);
    }

    [Fact]
    public void FindPinnedPiece_FriendlyPieceBlocksRay_NothingIsPinned()
    {
        var lightKing = new King(Color.Light, H1);
        var lightRook = new Rook(Color.Light, D5);
        var lightBishop = new Bishop(Color.Light, E4);
        var darkBishop = new Bishop(Color.Dark, A8);
        
        var board = ChessBoard.Create([lightKing, lightRook, lightBishop], [darkBishop]);
        
        Assert.False(lightRook.IsPinned);
        Assert.False(lightBishop.IsPinned);
    }
    // Move
    [Theory]
    [InlineData("A1")]
    [InlineData("A7")]
    [InlineData("F2")]
    [InlineData("G7")]
    public void Move(string positionTo)
    {
        var positionToMove = Position.Parse(positionTo);
        
        var lightBishop = new Bishop(Color.Light, D4);
        var darkKing = new King(Color.Dark, E8);

        var board = ChessBoard.Create([lightBishop], [darkKing]);
        
        lightBishop.Move(board, positionToMove);
        
        Assert.Null(board[D4].Piece);
        Assert.Equal(positionToMove, lightBishop.Position);
        Assert.Equal(lightBishop, board[positionToMove].Piece);
    }
    
    [Fact]
    public void Move_CaptureEnemyPiece()
    {
        var lightBishop = new Bishop(Color.Light, D4);
        var darkKing = new King(Color.Dark, E8);
        var darkKnight = new Knight(Color.Dark, F6);

        var board = ChessBoard.Create([lightBishop], [darkKing, darkKnight]);
        var darkPieces = board.GetPieces(darkKnight.Color);
        
        lightBishop.Move(board, darkKnight.Position);
        
        Assert.DoesNotContain(darkKnight, darkPieces);
    }
}