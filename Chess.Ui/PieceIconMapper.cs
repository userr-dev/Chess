using System;
using System.Collections.Generic;
using Avalonia.Svg.Skia;
using Chess.Core;
using Chess.Core.Pieces;

namespace Chess.Ui;

public static class PieceIconMapper
{
    private static readonly Color[] Colors = [Color.Light, Color.Dark];
    private static readonly Dictionary<string, SvgImage> PieceImages = new();

    static PieceIconMapper()
    {
        AddToDictionary(typeof(Pawn));
        AddToDictionary(typeof(Rook));
        AddToDictionary(typeof(Knight));
        AddToDictionary(typeof(Bishop));
        AddToDictionary(typeof(Queen));
        AddToDictionary(typeof(King));
    }

    private static void AddToDictionary(Type pieceType)
    {
        var typeName = pieceType.Name.ToLowerInvariant();

        foreach (var color in Colors)
        {
            var resourceKey = GetKey(color, typeName);
            PieceImages.Add(resourceKey, new SvgImage { Source = SvgSource.Load(GetResourcePath(color, typeName))});
        }
    }
    
    private static string GetKey(Color color, string typeName) =>
        $"{color.ToString().ToLowerInvariant()}_{typeName}";
    
    private static string GetResourcePath(Color color, string pieceTypeName)
    {
        var colorPrefix = color.ToString().ToLowerInvariant();
        
        return $"avares://Chess.Ui/Assets/Pieces/{colorPrefix}_{pieceTypeName}.svg";
    }
    
    public static SvgImage? GetPieceImage(Piece? piece)
    {
        if (piece is null) return null;

        var key = GetKey(piece.Color, piece.GetType().Name.ToLowerInvariant());
        return PieceImages.GetValueOrDefault(key);
    }
    
    public static SvgImage? GetPieceImage(Color color, PromotionType promotionType)
    {
        var key = GetKey(color, promotionType.ToString().ToLowerInvariant());
        return PieceImages.GetValueOrDefault(key);
    }
}