using Microsoft.Xna.Framework;

namespace ProjectGame
{
    public interface ICamera
    {
        Vector2 Position { get; set; }
        Matrix ViewMatrix { get; }

        void Update(GameTime gameTime);
    }
}