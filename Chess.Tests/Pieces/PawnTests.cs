namespace Chess.Tests.Pieces;

public class PawnTests
{
    private static Piece? GetPieceToPromote(string name, Color color, Position to)
    {
        return name switch
        {
            nameof(Knight) => new Knight(color, to),
            nameof(Bishop) => new Bishop(color, to),
            nameof(Rook) => new Rook(color, to),
            nameof(Queen) => new Queen(color, to),
            _ => null
        };
    }
    
    // GetAttackedPositions
    [Fact]
    public void GetAttackedPositions_OnEdge_ReturnOneSquare()
    {
        var lightPawn = new Pawn(Color.Light, A2);

        var board = ChessBoard.Create([lightPawn], []);

        var attackedPositions = lightPawn.GetAttackedPositions(board);
        Assert.Single(attackedPositions);
    }
    
    [Fact]
    public void GetAttackedPositions_NotOnEdge_ReturnTwoSquare()
    {
        var lightPawn = new Pawn(Color.Light, E2);

        var board = ChessBoard.Create([lightPawn], []);

        var attackedPositions = lightPawn.GetAttackedPositions(board);
        Assert.Equal(2, attackedPositions.Count());
    }

    [Fact]
    public void GetAttackedPositions_FriendlyBlocks_Included()
    {
        var lightPawn = new Pawn(Color.Light, E2);
        var lightBishop = new Bishop(Color.Light, F3);
        
        var board = ChessBoard.Create([lightPawn, lightBishop], []);

        var attackedPositions = lightPawn.GetAttackedPositions(board);
        Assert.Contains(attackedPositions, p => p.Equals(lightBishop.Position));
    }

    [Fact]
    public void GetAttackedPositions_EnemyBlocks_Included()
    {
        var lightPawn = new Pawn(Color.Light, E2);
        var darkBishop = new Bishop(Color.Dark, F3);

        var board = ChessBoard.Create([lightPawn], [darkBishop]);

        var attackedPositions = lightPawn.GetAttackedPositions(board);
        Assert.Contains(attackedPositions, p => p.Equals(darkBishop.Position));
    }
    
    // GetAvailableMoves
    [Fact]
    public void GetAvailableMoves_CanDoubleAdvance_ReturnTwoSquares()
    {
        var lightPawn = new Pawn(Color.Light, E2);

        var board = ChessBoard.Create([lightPawn], []);

        var moves = lightPawn.GetAvailableMoves(board).Moves;
        
        Assert.Equal(2, moves.Count);
    }
    
    [Fact]
    public void GetAvailableMoves_DarkPawnCanDoubleAdvance_MovesDown()
    {
        var darkPawn = new Pawn(Color.Dark, D7);
        var board = ChessBoard.Create([], [darkPawn]);

        var moves = darkPawn.GetAvailableMoves(board).Moves;

        Assert.Equal(2, moves.Count);
        Assert.All(moves, p => Assert.True(p.Row < D7.Row));
    }
    
    [Fact]
    public void GetAvailableMoves_CannotDoubleAdvance_ReturnOneSquare()
    {
        var lightPawn = new Pawn(Color.Light, E3);

        var board = ChessBoard.Create([lightPawn], []);

        var moves = lightPawn.GetAvailableMoves(board).Moves;

        Assert.Single(moves);
    }

    [Fact]
    public void GetAvailableMoves_CanDoubleAdvance_SecondSquareBlocked_ReturnOneSquare()
    {
        var lightPawn = new Pawn(Color.Light, E2);
        var lightBishop = new Bishop(Color.Light, E4);
        
        var board = ChessBoard.Create([lightPawn, lightBishop], []);

        var moves = lightPawn.GetAvailableMoves(board).Moves;

        Assert.Single(moves);
    }
    
    [Fact]
    public void GetAvailableMoves_SquareBlockedAhead_ReturnsEmpty()
    {
        var lightPawn = new Pawn(Color.Light, E2);
        var lightBishop = new Bishop(Color.Light, E3);
        
        var board = ChessBoard.Create([lightPawn, lightBishop], []);

        var moves = lightPawn.GetAvailableMoves(board).Moves;
        
        Assert.Empty(moves);
    }

    [Fact]
    public void GetAvailableAttacks_BothDiagonalsEnemy_ReturnsTwoSquares()
    {
        var lightPawn = new Pawn(Color.Light, E3);
        var darkKnight = new Knight(Color.Dark, F4);
        var darkRook = new Rook(Color.Dark, D4);
        
        var board = ChessBoard.Create([lightPawn], [darkKnight, darkRook]);

        var attacks = lightPawn.GetAvailableMoves(board).Attacks;

        Assert.Contains(attacks, p => p.Equals(darkKnight.Position));
        Assert.Contains(attacks, p => p.Equals(darkRook.Position));
    }

    [Fact]
    public void GetAvailableAttacks_BothDiagonalFriendly_ReturnsEmpty()
    {
        var lightPawn = new Pawn(Color.Light, E3);
        var lightKnight = new Knight(Color.Light, F4);
        var lightRook = new Rook(Color.Light, D4);
        
        var board = ChessBoard.Create([lightPawn, lightKnight, lightRook], []);

        var attacks = lightPawn.GetAvailableMoves(board).Attacks;

        Assert.DoesNotContain(attacks, p => p.Equals(lightKnight.Position));
        Assert.DoesNotContain(attacks, p => p.Equals(lightRook.Position));
    }
    
    [Fact]
    public void GetAvailableAttacks_NoEnemyPieces_ReturnsEmpty()
    {
        var lightPawn = new Pawn(Color.Light, E3);
        
        var board = ChessBoard.Create([lightPawn], []);

        var attacks = lightPawn.GetAvailableMoves(board).Attacks;
        
        Assert.Empty(attacks);
    }

    [Fact]
    public void GetAvailableMoves_PinnedAlongDiagonal_CannotMove()
    {
        var lightKing = new King(Color.Light, F1);
        var lightPawn = new Pawn(Color.Light, D3);
        var darkBishop = new Bishop(Color.Dark, B5);
        
        var board = ChessBoard.Create([lightKing, lightPawn], [darkBishop]);
        board.UpdateBoardState(darkBishop.Color);
        
        var (moves, attacks) = lightPawn.GetAvailableMoves(board);
        
        Assert.Empty(moves);
        Assert.Empty(attacks);
    }
    
    [Fact]
    public void GetAvailableMoves_PinnedCloseAlongDiagonal_CanCapture()
    {
        var lightKing = new King(Color.Light, F1);
        var lightPawn = new Pawn(Color.Light, D3);
        var darkBishop = new Bishop(Color.Dark, C4);
        
        var board = ChessBoard.Create([lightKing, lightPawn], [darkBishop]);
        board.UpdateBoardState(darkBishop.Color);
        
        var attacks = lightPawn.GetAvailableMoves(board).Attacks;
        
        Assert.Single(attacks);
    }

    [Fact]
    public void GetAvailableMoves_PinnedAlongHorizontal_CannotMove()
    {
        var lightKing = new King(Color.Light, G4);
        var lightPawn = new Pawn(Color.Light, F4);
        var darkRook = new Rook(Color.Dark, B4);
        
        var board = ChessBoard.Create([lightKing, lightPawn], [darkRook]);
        board.UpdateBoardState(darkRook.Color);
        
        var (moves, attacks) = lightPawn.GetAvailableMoves(board);
        
        Assert.Empty(moves);
        Assert.Empty(attacks);
    }
    
    [Fact]
    public void GetAvailableMoves_PinnedAheadVertically_CanMove()
    {
        var lightKing = new King(Color.Light, E1);
        var lightPawn = new Pawn(Color.Light, E4);
        var darkRook = new Rook(Color.Dark, E8);
        
        var board = ChessBoard.Create([lightKing, lightPawn], [darkRook]);
        board.UpdateBoardState(darkRook.Color);
        
        var moves = lightPawn.GetAvailableMoves(board).Moves;
        
        Assert.Single(moves);
    }
    
    [Fact]
    public void GetAvailableMoves_PinnedAheadAlongVertical_CannotMove()
    {
        var lightKing = new King(Color.Light, E1);
        var lightPawn = new Pawn(Color.Light, E4);
        var darkRook = new Rook(Color.Dark, E5);
        
        var board = ChessBoard.Create([lightKing, lightPawn], [darkRook]);
        board.UpdateBoardState(darkRook.Color);
        
        var moves = lightPawn.GetAvailableMoves(board).Moves;
        
        Assert.Empty(moves);
    }
    
    [Fact]
    public void GetAvailableMoves_PinnedBehindVertically_CanMove()
    {
        var lightKing = new King(Color.Light, E1);
        var lightPawn = new Pawn(Color.Light, E4);
        var darkRook = new Rook(Color.Dark, E8);
        
        var board = ChessBoard.Create([lightKing, lightPawn], [darkRook]);
        board.UpdateBoardState(darkRook.Color);
        
        var moves = lightPawn.GetAvailableMoves(board).Moves;
        
        Assert.Single(moves);
    }
    [Fact]
    public void GetAvailableMoves_PinnedVertically_CannotAttack()
    {
        var lightKing = new King(Color.Light, E1);
        var lightPawn = new Pawn(Color.Light, E4);
        var darkRook = new Rook(Color.Dark, E8);
        var darkKnight = new Knight(Color.Dark, D5);

        var board = ChessBoard.Create([lightKing, lightPawn], [darkRook, darkKnight]);
        board.UpdateBoardState(darkRook.Color);

        var attacks = lightPawn.GetAvailableMoves(board).Attacks;

        Assert.Empty(attacks);
    }
    
    // GetAvailableMoves when friendly king checked
    
    [Fact]
    public void GetAvailableMoves_KingInCheck_CanBlockWithPawn()
    {
        var lightKing = new King(Color.Light, G4);
        var lightPawn = new Pawn(Color.Light, F2);
        var darkRook = new Rook(Color.Dark, B4);
        
        var board = ChessBoard.Create([lightKing, lightPawn], [darkRook]);
        board.UpdateBoardState(darkRook.Color);
        
        var moves = lightPawn.GetAvailableMoves(board).Moves;
        
        Assert.Single(moves);
    }
    
    [Fact]
    public void GetAvailableMoves_KingInCheck_CanCaptureAttacker()
    {
        var lightKing = new King(Color.Light, G4);
        var lightPawn = new Pawn(Color.Light, C3);
        var darkRook = new Rook(Color.Dark, B4);
        
        var board = ChessBoard.Create([lightKing, lightPawn], [darkRook]);
        board.UpdateBoardState(darkRook.Color);
        var checkState = board.GetCheckState(lightKing.Color);
        
        var attacks = lightPawn.GetAvailableMoves(board).Attacks;
        
        Assert.Single(attacks);
        Assert.Contains(attacks, p => p == checkState.Attackers[0].Position);
    }

    [Fact]
    public void GetAvailableMoves_WhenPinned_CannotBlocksWithPawn()
    {
        var lightKing = new King(Color.Light, G4);
        var lightPawn = new Pawn(Color.Light, E2);
        var darkRook = new Rook(Color.Dark, B4);
        var darkBishop = new Bishop(Color.Dark, D1);
        
        var board = ChessBoard.Create([lightKing, lightPawn], [darkRook, darkBishop]);
        board.UpdateBoardState(darkBishop.Color);

        var moves = lightPawn.GetAvailableMoves(board).Moves;
        
        Assert.True(lightPawn.IsPinned);
        Assert.Empty(moves);
    }
    
    [Fact]
    public void GetAvailableMoves_WhenPinned_CannotCaptureAttacker()
    {
        var lightKing = new King(Color.Light, F3);
        var lightPawn = new Pawn(Color.Light, E2);
        var darkRook = new Rook(Color.Dark, D3);
        var darkBishop = new Bishop(Color.Dark, D1);
        
        var board = ChessBoard.Create([lightKing, lightPawn], [darkRook, darkBishop]);
        board.UpdateBoardState(darkRook.Color);

        var attacks = lightPawn.GetAvailableMoves(board).Attacks;
        
        Assert.True(lightPawn.IsPinned);
        Assert.Empty(attacks);
    }

    [Fact]
    public void GetAvailableMoves_WhenFriendlyKingIsDoubleChecked_CannotMove()
    {
        var lightKing = new King(Color.Light, E1);
        var lightPawn = new Pawn(Color.Light, C2);
        var darkBishop1 = new Bishop(Color.Dark, A5);
        var darkBishop2 = new Bishop(Color.Dark, H4);

        var board = ChessBoard.Create([lightPawn, lightKing], [darkBishop1, darkBishop2]);
        board.UpdateBoardState(darkBishop1.Color);
        var checkState = board.GetCheckState(lightKing.Color);

        var (moves, attacks) = lightPawn.GetAvailableMoves(board);
        Assert.True(checkState.IsDoubleChecked);
        Assert.Empty(moves);
        Assert.Empty(attacks);
    }
    
    // Move
    [Fact]
    public void Move_DoubleAdvance()
    {
        var lightPawn = new Pawn(Color.Light, A2);
        var board = ChessBoard.Create([lightPawn], []);

        var moves = lightPawn.GetAvailableMoves(board).Moves;
        var positionToMove = moves[^1];
        
        lightPawn.Move(board, positionToMove);

        Assert.Equal(A4, positionToMove);
        Assert.Null(board[A2].Piece);
        Assert.Equal(A4, lightPawn.Position);
        Assert.Equal(lightPawn, board[A4].Piece);
    }
    
    // Promoting
    [Theory]
    [InlineData(nameof(Knight), typeof(Knight))]
    [InlineData(nameof(Bishop), typeof(Bishop))]
    [InlineData(nameof(Rook), typeof(Rook))]
    [InlineData(nameof(Queen), typeof(Queen))]
    public void Move_PromoteToSelectedPiece(string pieceName, Type pieceType)
    {
        var lightPawn = new Pawn(Color.Light, A7);
        lightPawn.Promoted += (_, args) =>
            args.PromotedPiece = GetPieceToPromote(pieceName, args.Color, args.Position);
        var board = ChessBoard.Create([lightPawn], []);

        var moves = lightPawn.GetAvailableMoves(board).Moves;
        var positionToMove = moves[^1];
        
        lightPawn.Move(board, positionToMove);

        Assert.IsType(pieceType, board[positionToMove].Piece);
        Assert.Equal(lightPawn.Color, board[positionToMove].Piece?.Color);
    }
    
    [Theory]
    [InlineData(nameof(Knight))]
    [InlineData(nameof(Bishop))]
    [InlineData(nameof(Rook))]
    [InlineData(nameof(Queen))]
    public void Move_PromoteToWrongColor(string pieceName)
    {
        var lightPawn = new Pawn(Color.Light, A7);
        lightPawn.Promoted += (_, args) =>
            args.PromotedPiece = GetPieceToPromote(pieceName, args.Color.Opposite(), args.Position);
        var board = ChessBoard.Create([lightPawn], []);

        var moves = lightPawn.GetAvailableMoves(board).Moves;
        var positionToMove = moves[^1];

        Assert.Throws<ArgumentException>(() => lightPawn.Move(board, positionToMove));
    }
    
    [Fact]
    public void Move_PromoteToWrongType()
    {
        var lightPawn = new Pawn(Color.Light, A7);
        lightPawn.Promoted += (_, args) =>
            args.PromotedPiece = new King(args.Color, args.Position);
        var board = ChessBoard.Create([lightPawn], []);

        var moves = lightPawn.GetAvailableMoves(board).Moves;
        var positionToMove = moves[^1];

        Assert.Throws<ArgumentException>(() => lightPawn.Move(board, positionToMove));
    }
    
    [Theory]
    [InlineData(nameof(Rook))]
    [InlineData(nameof(Queen))]
    public void Move_PromoteToPieceForCheckEnemyKing(string pieceName)
    {
        var lightPawn = new Pawn(Color.Light, A7);
        lightPawn.Promoted += (_, args) =>
            args.PromotedPiece = GetPieceToPromote(pieceName, args.Color, args.Position);

        var darkKing = new King(Color.Dark, H8);
        
        var board = ChessBoard.Create([lightPawn], [darkKing]);
        
        var moves = lightPawn.GetAvailableMoves(board).Moves;
        lightPawn.Move(board, moves[^1]);
        
        var checkState = board.GetCheckState(darkKing.Color);
        Assert.True(checkState.IsChecked);
    }
}