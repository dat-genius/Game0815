using System;
using Microsoft.Xna.Framework;

namespace ProjectGame.Voorbeeld
{
    public class SharedCollisionBehaviourVB : IBehaviour
    {
        public GameObject GameObject { get; set; }

        public void OnUpdate(GameTime gameTime)
        {
        }

        public void OnMessage(IMessage message)
        {
            switch (message.MessageType)
            {
                case MessageType.CollisionEnter:
                {
                    var collisionEnterMessage = message as CollisionEnterMessage;
                    if (collisionEnterMessage == null) return;

                    // The object has entered a collision with:
                    // var other = collisionEnterMessage.CollidingObject;
                    // Check if the other object is an enemy:
                    // other.HasBehaviourOfType(typeof(SomeFuckingBigAssMonsterBehaviour));

                    // In this example though the code is shared between the monster and the player, so we'll just set the color:
                    //GameObject.Color = Color.Red;


                }
                break;

                case MessageType.CollisionExit:
                {
                    var collisionExitMessage = message as CollisionExitMessage;
                    if (collisionExitMessage == null) return;

                    // The object has exited a collision with:
                    // var other = collisionExitMessage.CollidingObject;
                    // Check if the other object is an enemy:
                    // other.HasBehaviourOfType(typeof (SomeFuckingBigAssMonsterBehaviour));

                    // In this example though the code is shared between the monster and the player, so we'll just set the color:
                    GameObject.Color = Color.White;
                }
                break;
            }
        }
    }
}