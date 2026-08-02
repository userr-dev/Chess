using Chess.Core;

namespace Chess.Ui.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly Game _game;
    public BoardViewModel Board { get; }

    public MainWindowViewModel()
    {
        _game = new Game();
        Board = new BoardViewModel(_game);
    }
}