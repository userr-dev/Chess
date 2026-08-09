namespace Chess.Core;

public enum PromotionType
{
    Queen, Rook, Bishop, Knight
}

public static class PromotionTypeExtension
{
    extension(PromotionType promotionType)
    {
        public Piece GetPromotionPiece(Color color, Position position)
        {
            return promotionType switch
            {
                PromotionType.Queen => new Queen(color, position),
                PromotionType.Rook => new Rook(color, position),
                PromotionType.Bishop => new Bishop(color, position),
                PromotionType.Knight => new Knight(color, position),
                _ => new Queen(color, position)
            };
        }
    }
}