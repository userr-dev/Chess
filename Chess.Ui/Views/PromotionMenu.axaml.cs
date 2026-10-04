using Avalonia.Controls;
using Avalonia.Input;
using Chess.Ui.ViewModels;

namespace Chess.Ui.Views;

public partial class PromotionMenu : UserControl
{
    public PromotionMenu()
    {
        InitializeComponent();
    }

    private void Option_OnTapped(object? sender, TappedEventArgs e)
    {
        if (sender is Control { DataContext: PromotionMenuOptionViewModel option }
            && DataContext is PromotionMenuViewModel vm)
        {
            vm.SelectPromotionTypeCommand.Execute(option);
        }
    }
}