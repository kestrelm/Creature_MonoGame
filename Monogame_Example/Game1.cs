#region File Description
//-----------------------------------------------------------------------------
// SampleGame.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using MeshBoneUtil;
using CreatureModule;
using CreatureRenderer;
using System.Collections.Generic;


#endregion

namespace Monogame_Example
{
    /// <summary>
    /// Default Project Template
    /// </summary>
    public class Game1 : Game
    {

        CreatureModule.Creature curCreature;
        CreatureModule.CreatureManager curManager;
        CreatureRenderer.Renderer curRenderer;
        String curAnimationName;

        #region Fields

        GraphicsDeviceManager graphics;
        SpriteBatch batch;
        #endregion

        #region Initialization

        public Game1()
        {

            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Overridden from the base Game.Initialize. Once the GraphicsDevice is setup,
        /// we'll use the viewport to initialize some values.
        /// </summary>
        protected override void Initialize()
        {

            graphics.IsFullScreen = true;
            base.Initialize();
        }

        string GetAppDir()
        {
            return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        /// <summary>
        /// Load your graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            var basePath = GetAppDir();

            Dictionary<string, object> load_data = CreatureModule.Utils.LoadCreatureJSONData(basePath + "\\Content\\iceDemonExport_character_data.json");
       
            curCreature = new CreatureModule.Creature(ref load_data);
            curManager = new CreatureModule.CreatureManager(curCreature);
            batch = new SpriteBatch(this.GraphicsDevice);


            curManager.CreateAnimation(ref load_data, "default");

            curAnimationName = "default";
            curManager.SetActiveAnimationName(curAnimationName);
            curManager.SetIsPlaying(true);
            curManager.should_loop = true;

            Texture2D curTexture;
            curTexture = Content.Load<Texture2D>("ice.png");

            curRenderer = new CreatureRenderer.Renderer(graphics.GraphicsDevice, curManager, ref curTexture);
            curRenderer.world = Matrix.CreateScale(new Vector3(25.0f, 25.0f, 1.0f));

        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here		
            curManager.Update(0.025f);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself. 
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            curRenderer.DoUpdate(graphics.GraphicsDevice);

            batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, null);

            batch.End();
            base.Draw(gameTime);
        }


        #endregion
    }
}