using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Svg.Skia;
using Chess.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Color = Chess.Core.Color;

namespace Chess.Ui.ViewModels;

public partial class PromotionMenuViewModel : ViewModelBase
{
    private TaskCompletionSource<PromotionType>? _tcs;
    [ObservableProperty] public partial bool IsMenuOpened { get; private set; } = false;
    
    public ObservableCollection<PromotionMenuOptionViewModel> OptionViewModels { get; } =
    [
        new (PromotionType.Queen),
        new (PromotionType.Rook),
        new (PromotionType.Bishop),
        new (PromotionType.Knight),
    ];

    public Task<PromotionType> ShowAsync(Color color)
    {
        _tcs = new TaskCompletionSource<PromotionType>();
        
        foreach (var optionViewModel in OptionViewModels)
        {
            optionViewModel.Update(color);
        }
        
        IsMenuOpened = true;

        return _tcs.Task;
    }
    
    [RelayCommand]
    private void SelectPromotionType(PromotionMenuOptionViewModel optionViewModel)
    {
        _tcs?.TrySetResult(optionViewModel.Type);
        
        IsMenuOpened = false;
    }
}

public partial class PromotionMenuOptionViewModel(PromotionType type) : ViewModelBase
{
    public PromotionType Type { get; } = type;
    
    [ObservableProperty] 
    public partial IImage? PieceImage { get; private set; }

    internal void Update(Color color)
    {
        var path = PieceIconMapper.GetResourcePath(color, Type);
        PieceImage = string.IsNullOrEmpty(path)
            ? null
            : new SvgImage { Source = SvgSource.Load(path) };
    }
}