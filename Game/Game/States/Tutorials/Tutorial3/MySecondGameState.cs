﻿namespace Dive.Game.States.Tutorials.Tutorial3
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
    using Dive.Entity;
    using Dive.Script;
    using SFML.Graphics;
    
    /// <summary>
    /// Used in tutorial 3 of the Dive Engine tutorial series.
    /// </summary>
    [GameState]
    public class MySecondGameState : AbstractGameState
    {
        /// <summary>
        /// Called when the state starts.
        /// </summary>
        /// <param name="previous">Previous game state.</param>
        public override void Start(IGameState previous)
        {
            Entity myEntity = GameEngine.Instance.EntityManager.CreateEntity(); // Create the entity
            TransformComponent transform = myEntity.AddComponent<TransformComponent>(); // Add the components
            SpriteComponent sprite = myEntity.AddComponent<SpriteComponent>();

            Texture myTexture = GameEngine.Instance.AssetManager.Load<Texture>("content/textures/test.png"); // Load a texture
            sprite.Drawable.Texture = myTexture; // Set the texture

            // Set the position
            transform.SetPosition(
                (GameEngine.Instance.Window.Size.X / 2) - (myTexture.Size.X / 2) + 100,
                (GameEngine.Instance.Window.Size.Y / 2) - (myTexture.Size.Y / 2));
        }

        /// <summary>
        /// Called when the state ends.
        /// </summary>
        /// <param name="next">Next game state.</param>
        public override void Stop(IGameState next)
        {
            GameEngine.Instance.EntityManager.Clear();
        }
    }
}