using MarketPlace.Domain.Entities;

namespace MarketPlace.Domain.Extensions
{
    public static class PictureRule
    {
        public static bool HasCorrectSize(this Picture picture) =>
            picture != null &&
            picture.Size.Width >= 800 &&
            picture.Size.Height >= 600;
    }
}
