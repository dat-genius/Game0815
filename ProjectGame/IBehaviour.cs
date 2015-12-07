using Microsoft.Xna.Framework;

namespace ProjectGame
{
    public interface IBehaviour
    {
        GameObject GameObject { get; set; }
        void OnUpdate(GameTime gameTime);
        void OnMessage(IMessage message);
    }
}