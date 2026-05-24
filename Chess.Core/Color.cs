namespace Chess.Core;

public enum Color
{
    Light, Dark
}

public static class ColorExtension
{
    public static Color Opposite(this Color color) => color == Color.Light ? Color.Dark : Color.Light;
}