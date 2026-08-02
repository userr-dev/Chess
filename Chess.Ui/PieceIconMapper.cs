using Chess.Core.Pieces;

namespace Chess.Ui;

public static class PieceIconMapper
{
    public static string GetResourcePath(Piece? piece)
    {
        if (piece is null) return "";

        var colorPrefix = piece.Color.ToString().ToLowerInvariant();
        var typeName = piece.GetType().Name.ToLowerInvariant();
        
        return $"avares://Chess.Ui/Assets/Pieces/{colorPrefix}_{typeName}.svg";
    }
}