namespace Chess.Tests.Pieces;

public class KingTests
{
    // GetAttackedPositions
    [Fact]
    public void GetAttackedPosition_FromCenter_ReturnEightSquare()
    {
        var lightKing = new King(Color.Light, E4);

        var board = ChessBoard.Create([lightKing], []);

        var attackedPosition = lightKing.GetAttackedPositions(board);
        
        Assert.Equal(8, attackedPosition.Count());
    }
    
    [Fact]
    public void GetAttackedPositions_FromCorner_ReturnThreeSquare()
    {
        var lightKing = new King(Color.Light, A1);

        var board = ChessBoard.Create([lightKing], []);

        var attackedPosition = lightKing.GetAttackedPositions(board);
        
        Assert.Equal(3, attackedPosition.Count());
    }
    
    [Fact]
    public void GetAttackedPositions_OnEdge_ReturnFiveSquare()
    {
        var lightKing = new King(Color.Light, E1);

        var board = ChessBoard.Create([lightKing], []);

        var attackedPosition = lightKing.GetAttackedPositions(board);
        
        Assert.Equal(5, attackedPosition.Count());
    }

    [Fact]
    public void GetAttackedPositions_FriendlyBlocks_Included()
    {
        var lightKing = new King(Color.Light, E4);
        var lightPawn = new Pawn(Color.Light, E5);
        
        var board = ChessBoard.Create([lightKing, lightPawn], []);

        var attackedPosition = lightKing.GetAttackedPositions(board);

        Assert.Contains(attackedPosition, p => p.Equals(lightPawn.Position));
    }

    [Fact]
    public void GetAttackedPositions_EnemyBlocks_Included()
    {
        var lightKing = new King(Color.Light, E4);
        var darkPawn = new Pawn(Color.Dark, E5);
        
        var board = ChessBoard.Create([lightKing], [darkPawn]);

        var attackedPosition = lightKing.GetAttackedPositions(board);

        Assert.Contains(attackedPosition, p => p.Equals(darkPawn.Position));
    }
    
    // GetAvailableMoves
    [Fact]
    public void GetAvailableMoves_FromCenter_ReturnEightSquare()
    {
        var lightKing = new King(Color.Light, E4);

        var board = ChessBoard.Create([lightKing], []);

        var moves = lightKing.GetAvailableMoves(board).Moves;
        
        Assert.Equal(8, moves.Count);
    }
    
    [Fact]
    public void GetAvailableMoves_FromCorner_ReturnThreeSquare()
    {
        var lightKing = new King(Color.Light, A1);

        var board = ChessBoard.Create([lightKing], []);

        var moves = lightKing.GetAvailableMoves(board).Moves;
        
        Assert.Equal(3, moves.Count);
    }
    
    [Fact]
    public void GetAvailableMoves_OnEdge_ReturnFiveSquare()
    {
        var lightKing = new King(Color.Light, E1);

        var board = ChessBoard.Create([lightKing], []);

        var moves = lightKing.GetAvailableMoves(board).Moves;
        
        Assert.Equal(5, moves.Count);
    }

    [Fact]
    public void GetAvailableMoves_FriendlyBlocks_NotIncluded()
    {
        var lightKing = new King(Color.Light, E4);
        var lightPawn = new Pawn(Color.Light, E5);
        
        var board = ChessBoard.Create([lightKing, lightPawn], []);

        var moves = lightKing.GetAvailableMoves(board).Moves;

        Assert.DoesNotContain(moves, p => p.Equals(lightPawn.Position));
    }

    [Fact]
    public void GetAvailableMoves_EnemyBlocks_NotIncluded()
    {
        var lightKing = new King(Color.Light, E4);
        var darkPawn = new Pawn(Color.Dark, E5);
        
        var board = ChessBoard.Create([lightKing], [darkPawn]);

        var moves = lightKing.GetAvailableMoves(board).Moves;

        Assert.DoesNotContain(moves, p => p.Equals(darkPawn.Position));
    }
    
    [Fact]
    public void GetAvailableAttacks_Friendly_NotIncluded()
    {
        var lightKing = new King(Color.Light, E4);
        var lightPawn = new Pawn(Color.Light, E5);
        
        var board = ChessBoard.Create([lightKing, lightPawn], []);

        var attacks = lightKing.GetAvailableMoves(board).Attacks;

        Assert.DoesNotContain(attacks, p => p.Equals(lightPawn.Position));
    }

    [Fact]
    public void GetAvailableAttacks_EnemyBlocks_Included()
    {
        var lightKing = new King(Color.Light, E4);
        var darkPawn = new Pawn(Color.Dark, E5);
        
        var board = ChessBoard.Create([lightKing], [darkPawn]);

        var attacks = lightKing.GetAvailableMoves(board).Attacks;

        Assert.Contains(attacks, p => p.Equals(darkPawn.Position));
    }
    
    [Fact]
    public void GetAvailableAttacks_NoEnemy_Included()
    {
        var lightKing = new King(Color.Light, E4);
        var darkPawn = new Pawn(Color.Dark, E5);
        
        var board = ChessBoard.Create([lightKing], [darkPawn]);

        var attacks = lightKing.GetAvailableMoves(board).Attacks;

        Assert.Contains(attacks, p => p.Equals(darkPawn.Position));
    }

    [Fact]
    public void GetAvailableMoves_CannotMoveToSquareAttackedByEnemyPiece()
    {
        var lightKing = new King(Color.Light, E4);
        var darkRook = new Rook(Color.Dark, F8);
        
        var board = ChessBoard.Create([lightKing], [darkRook]);

        var moves = lightKing.GetAvailableMoves(board).Moves;
        
        Assert.DoesNotContain(moves, p => p.Equals(F3));
        Assert.DoesNotContain(moves, p => p.Equals(F4));
        Assert.DoesNotContain(moves, p => p.Equals(F5));
    }

    [Fact]
    public void GetAvailableMoves_CannotMoveToSquareAttackedByEnemyKing()
    {
        var lightKing = new King(Color.Light, E4);
        var darkKing = new King(Color.Dark, E6);
        
        var board = ChessBoard.Create([lightKing], [darkKing]);

        var moves = lightKing.GetAvailableMoves(board).Moves;
        
        Assert.DoesNotContain(E5, moves);
        Assert.DoesNotContain(D5, moves);
        Assert.DoesNotContain(F5, moves);
    }
    
    [Fact]
    public void GetAvailableAttacks_CannotCaptureEnemyPieceOnAttackedSquare()
    {
        var lightKing = new King(Color.Light, E4);
        var darkRook = new Rook(Color.Dark, F8);
        var darkPawn = new Pawn(Color.Dark, F4);
        
        var board = ChessBoard.Create([lightKing], [darkRook, darkPawn]);

        var attacks = lightKing.GetAvailableMoves(board).Attacks;
        
        Assert.DoesNotContain(attacks, p => p.Equals(F4));
    }
    
    // Castling

    [Fact]
    public void GetAvailableMoves_IsChecked_CannotCastle()
    {
        var lightKing = new King(Color.Light, E1);
        var leftLightRook = new Rook(Color.Light, A1);
        var rightLightRook = new Rook(Color.Light, H1);

        var darkRook = new Rook(Color.Dark, E8);

        var board = ChessBoard.Create([lightKing, leftLightRook, rightLightRook], [darkRook]);
        
        var kingMoves = lightKing.GetAvailableMoves(board).Moves;

        Assert.Equal(4, kingMoves.Count);
    }
    
    [Fact]
    public void GetAvailableMoves_IsDoubleChecked_CannotCastle()
    {
        var lightKing = new King(Color.Light, E1);
        var leftLightRook = new Rook(Color.Light, A1);
        var rightLightRook = new Rook(Color.Light, H1);

        var darkRook = new Rook(Color.Dark, E8);
        var darkBishop = new Bishop(Color.Dark, B4);
        
        var board = ChessBoard.Create([lightKing, leftLightRook, rightLightRook], [darkRook, darkBishop]);
        
        var kingMoves = lightKing.GetAvailableMoves(board).Moves;

        Assert.Equal(3, kingMoves.Count);
    }
    
    [Fact]
    public void GetAvailableMoves_CanCastle()
    {
        var lightKing = new King(Color.Light, E1);
        var lightLeftRook = new Rook(Color.Light, A1);
        var lightRightRook = new Rook(Color.Light, H1);

        var board = ChessBoard.Create([lightKing, lightLeftRook, lightRightRook], []);

        var moves = lightKing.GetAvailableMoves(board).Moves;
        
        Assert.Contains(G1, moves);
        Assert.Contains(C1, moves);
    }
    
    // Move

    [Theory]
    [InlineData("C3")]
    [InlineData("C4")]
    [InlineData("C5")]
    [InlineData("D3")]
    [InlineData("D5")]
    [InlineData("E3")]
    [InlineData("E4")]
    [InlineData("E5")]
    public void Move(string positionTo)
    {
        var positionToMove = Position.Parse(positionTo);
        var lightKing = new King(Color.Light, D4);

        var board = ChessBoard.Create([lightKing], []);
        
        lightKing.Move(board, positionToMove);
        
        Assert.Null(board[D4].Piece);
        Assert.Equal(positionToMove, lightKing.Position);
        Assert.Equal(lightKing, board[positionToMove].Piece);
    }
    
    [Fact]
    public void Move_CastlingKingSide()
    {
        var lightKing = new King(Color.Light, E1);
        var lightQueenSideRook = new Rook(Color.Light, A1);
        var lightKingSideRook = new Rook(Color.Light, H1);

        var board = ChessBoard.Create([lightKing, lightQueenSideRook, lightKingSideRook], []);
        
        lightKing.Move(board, G1);
        
        Assert.Equal(G1, lightKing.Position);
        Assert.Equal(F1, lightKingSideRook.Position);
    }
    
    [Fact]
    public void Move_CastlingQueenSide()
    {
        var lightKing = new King(Color.Light, E1);
        var lightQueenSideRook = new Rook(Color.Light, A1);
        var lightKingSideRook = new Rook(Color.Light, H8);

        var board = ChessBoard.Create([lightKing, lightQueenSideRook, lightKingSideRook], []);
        
        lightKing.Move(board, C1);
        
        Assert.Equal(C1, lightKing.Position);
        Assert.Equal(D1, lightQueenSideRook.Position);
    }
}