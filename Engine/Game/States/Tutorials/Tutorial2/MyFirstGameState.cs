namespace Dive.Game.States.Tutorials.Tutorial2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dive.Engine;
    using Dive.Engine.Attributes;
    using Dive.Engine.Components;
    using Dive.Engine.Components.Graphics;
    using Dive.Engine.Scheduler;
    using Dive.Entity;
    using log4net;
    using SFML.Graphics;
    
    /// <summary>
    /// Used in tutorial 2 of the Dive Engine tutorial series.
    /// </summary>
    [GameState]
    public class MyFirstGameState : AbstractGameState
    {
        /// <summary>
        /// Called when the state starts.
        /// </summary>
        /// <param name="previous">Previous game state.</param>
        public override void Start(IGameState previous)
        {
            Entity myEntity = this.Engine.EntityManager.CreateEntity(); // Create the entity
            TransformComponent transform = myEntity.AddComponent<TransformComponent>(); // Add the components
            SpriteComponent sprite = myEntity.AddComponent<SpriteComponent>();

            Texture myTexture = this.Engine.AssetManager.Load<Texture>("content/textures/test.png"); // Load a texture
            sprite.Drawable.Texture = myTexture; // Set the texture

            // Set the position
            transform.SetPosition(
                (this.Engine.Window.Size.X / 2) - (myTexture.Size.X / 2),
                (this.Engine.Window.Size.Y / 2) - (myTexture.Size.Y / 2));

            TaskInfo myTask = new TaskInfo()
            {
                ExecuteAfter = 5f,
                Task = () =>
                {
                    Console.WriteLine("Hello from myTask!");
                    this.Engine.StateManager.ChangeState<MySecondGameState>();
                }
            };

            this.Engine.Scheduler.ScheduleTask(myTask);
        }

        /// <summary>
        /// Called when the state ends.
        /// </summary>
        /// <param name="next">Next game state.</param>
        public override void Stop(IGameState next)
        {
            this.Engine.EntityManager.Clear();
        }
    }
}
