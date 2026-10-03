using Avalonia.Media;
using Chess.Core.Board;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chess.Ui.ViewModels;

public partial class SquareViewModel : ViewModelBase
{
    public Position Position { get; }
    public bool IsLightSquare { get; }

    [ObservableProperty]
    public partial bool CanMovable { get; set; }

    [ObservableProperty]
    public partial bool CanAttack { get; set; }

    [ObservableProperty]
    public partial bool IsKingChecked { get; set; }

    [ObservableProperty]
    public partial IImage? PieceImage { get; set; }

    public SquareViewModel(Position position, bool isLightSquare)
    {
        Position = position;
        IsLightSquare = isLightSquare;
    }
    
    public void UpdateFrom(Square square)
    {
        PieceImage = PieceIconMapper.GetPieceImage(square.Piece);
    }

    internal void ClearHighlights()
    {
        CanMovable = false;
        CanAttack = false;
    }
}