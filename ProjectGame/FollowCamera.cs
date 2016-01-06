using Microsoft.Xna.Framework;

namespace ProjectGame
{
    public class FollowCamera : ICamera
    {
        private GameObject target;
        public GameObject Target
        {
            private get { return target; }
            set
            {
                target = value;
                position = target.Position;
                viewMatrix = Matrix.CreateTranslation(Offset.X - (target.Position.X + target.Size.X/4.0f),
                    Offset.Y - (target.Position.Y + target.Size.Y/4.0f), 0);
            }
        }

        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set
            {
                if (value == position) return;
                var difference = position - value;
                viewMatrix *= Matrix.CreateTranslation(new Vector3(difference.X, difference.Y, 0)) * 
                            Matrix.CreateRotationZ(0) * 
                            Matrix.CreateScale(1.0f);
                position = value;
            }
        }

        private Matrix viewMatrix;
        public Matrix ViewMatrix
        {
            get { return viewMatrix; }
        }

        public float LerpFactor { get; set; }
        public Vector2 Offset { get; set; }


        public FollowCamera(float lerpFactor = 0.1f)
        {
            LerpFactor = lerpFactor;
        }


        public void Update(GameTime gameTime)
        {
            Position = Vector2.Lerp(Position, Target.Position, LerpFactor);
        }
    }
}