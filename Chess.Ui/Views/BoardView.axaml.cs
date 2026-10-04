using Avalonia.Controls;
using Avalonia.Input;
using Chess.Ui.ViewModels;

namespace Chess.Ui.Views;

public partial class BoardView : UserControl
{
    public BoardView()
    {
        InitializeComponent();
    }

    private void Square_OnTapped(object? sender, TappedEventArgs e)
    {
        if (sender is Control { DataContext: SquareViewModel square }
            && DataContext is BoardViewModel vm
            && vm.SelectSquareCommand.CanExecute(square))
        {
            vm.SelectSquareCommand.Execute(square);
        }
    }
}