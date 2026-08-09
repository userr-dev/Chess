using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    private AvailableMoves? _moveResult;
    
    public BoardViewModel(Game game)
    {
        _game = game;
        _board = _game.ChessBoard;
        BuildSquares();
        RefreshFromBoard();
        _board.UpdatedBoardState += UpdateBoard;
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
            if (_moveResult!.Contains(position) && position.IsPromotionRow(_selectedPiece.Color) && _selectedPiece is Pawn pawn)
            {
                var promotionType = await PromotionMenuViewModel.ShowAsync(pawn.Color);
                _game.TryPromoteMove(pawn, _moveResult!, position, promotionType);
                return;
            }
            
            if (_game.TryMove(_selectedPiece, _moveResult!, position))
            {
                return;
            }

            var isAgainSelectPiece = _selectedPiece == _board[position].Piece;
            ClearSelectedPiece();
            if (isAgainSelectPiece) return;
        }

        SelectPiece(_board[position]);
    }

    private void UpdateBoard()
    {
        HideKingCheckHighlight();
        ClearSelectedPiece();
                
        RefreshFromBoard();
        HighlightKingCheck(_game.CurrentPlayer.Opposite());
        HighlightKingCheck(_game.CurrentPlayer);
    }
    
    private void ClearSelectedPiece()
    {
        HideMovesHighlights();
        _selectedPiece = null;
        _moveResult = null;
    }
    
    private void SelectPiece(Square square)
    {
        if (!square.HasPiece || square.Piece?.Color != _game.CurrentPlayer) return;
        
        _selectedPiece = square.Piece;
        _moveResult = _selectedPiece.GetAvailableMoves(_board);
        HighlightsMoves(_moveResult);
    }
}