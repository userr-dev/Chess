using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Chess.Core;
using Chess.Core.Board;
using Chess.Core.Pieces;
using CommunityToolkit.Mvvm.Input;

namespace Chess.Ui.ViewModels;

public partial class BoardViewModel : ViewModelBase
{
    private readonly ChessBoard _board;
    private readonly Game _game;
    private readonly Dictionary<Position, SquareViewModel> _squareViewModels = new();
    
    public ObservableCollection<SquareViewModel> Squares { get; } = [];

    public PromotionMenuViewModel PromotionMenuViewModel { get; } = new();
    
    private Piece? _selectedPiece;

    public ObservableCollection<int> Rows { get; } = [.. Enumerable.Range(1, 8).Reverse()];
    public ObservableCollection<Column> Columns { get; } = [.. Enum.GetValues<Column>()];
    
    public BoardViewModel(Game game)
    {
        _game = game;
        _board = _game.ChessBoard;
        BuildSquares();
        RefreshFromBoard();
        _board.BoardStateUpdated += UpdateBoard;
        _game.PlayerChanged += GameOnPlayerChanged;
    }
    
    private void GameOnPlayerChanged(Color currentPlayer)
    {
        RotateBoard(currentPlayer);
    }

    private void BuildSquares()
    {
        for (var row = 7; row >= 0; row--)
        {
            for (var col = 0; col < 8; col++)
            {
                var position = Position.Create((Column)col, row);
                var isLight = _board[position].Color == Color.Light;
                var squareViewModel = new SquareViewModel(position, isLight);
                _squareViewModels.Add(position, squareViewModel);
                Squares.Add(squareViewModel);
            }
        }
    }
    
    private void RefreshFromBoard()
    {
        foreach (var squareVm in Squares)
            squareVm.UpdateFrom(_board[squareVm.Position]);
    }

    private void HighlightsMoves(AvailableMoves availableMoves)
    {
        foreach (var move in availableMoves.Moves)
        {
            _squareViewModels[move].CanMovable = true;
        }
        foreach (var move in availableMoves.Attacks)
        {
            _squareViewModels[move].CanAttack = true;
        }
    }

    private void HighlightKingCheck(Color color)
    {
        var isKingChecked = _board.GetCheckState(color).IsChecked;
        var kingPosition = _board.GetKingPosition(color);
        if (kingPosition is null) return;
        _squareViewModels[kingPosition.Value].IsKingChecked = isKingChecked;
    }
    
    private void HideMovesHighlights()
    {
        foreach (var squareVm in Squares)
            squareVm.ClearHighlights();
    }

    private void HideKingCheckHighlight()
    {
        foreach (var squareVm in Squares)
            squareVm.IsKingChecked = false;
    }
    
    [RelayCommand]
    private async Task SelectSquare(SquareViewModel squareViewModel)
    {
        var position = squareViewModel.Position;

        if (_selectedPiece is not null)
        {
            var currentMoves = _selectedPiece.GetAvailableMoves(_board);
            if (currentMoves.Contains(position) && position.IsPromotionRow(_selectedPiece.Color) && _selectedPiece is Pawn pawn)
            {
                var promotionType = await PromotionMenuViewModel.ShowAsync(pawn.Color);
                pawn.Promoted += (_, args) =>
                {
                    args.PromotedPiece = promotionType.GetPromotionPiece(args.Color, args.Position);
                };
            }
            
            if (_game.TryMove(_selectedPiece, position))
            {
                return;
            }

            var isAgainSelectPiece = _selectedPiece == _board[position].Piece;
            UnSelectPiece();
            if (isAgainSelectPiece) return;
        }

        SelectPiece(_board[position]);
    }

    private void UpdateBoard()
    {
        HideKingCheckHighlight();
        UnSelectPiece();
                
        RefreshFromBoard();
        HighlightKingCheck(_game.CurrentPlayer.Opposite());
        HighlightKingCheck(_game.CurrentPlayer);
    }
    
    private void UnSelectPiece()
    {
        HideMovesHighlights();
        _selectedPiece = null;
    }
    
    private void SelectPiece(Square square)
    {
        if (!square.HasPiece || square.Piece?.Color != _game.CurrentPlayer || !_game.IsGameStarted || _game.IsGamePaused) return;
        
        _selectedPiece = square.Piece;
        var availableMoves = _selectedPiece.GetAvailableMoves(_board);
        HighlightsMoves(availableMoves);
    }

    private void RotateBoard(Color currentPlayer)
    {
        RotateSquaresOnBoard(currentPlayer);
        ReverseAxis(Rows);
        ReverseAxis(Columns);
    }

    private void RotateSquaresOnBoard(Color currentPlayer)
    {
        var positions = currentPlayer is Color.Light 
            ? _board.GetPositionsFromLightPerspective() 
            : _board.GetPositionsFromDarkPerspective();
        var index = 0;
        foreach (var position in positions)
        {
            var targetViewModel = _squareViewModels[position];
            var currentIndex = Squares.IndexOf(targetViewModel);

            if (currentIndex != index)
            {
                Squares.Move(currentIndex, index);
            }
            index++;
        }
    }

    private void ReverseAxis<T>(ObservableCollection<T> collection)
    {
        var length = collection.Count;
        for (int i = 0; i < length; i++)
        {
            collection.Move(0, length - 1 - i);
        }
    }
}