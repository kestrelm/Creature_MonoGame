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

namespace Sample
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
		SpriteBatch spriteBatch;
		Texture2D logoTexture;

		#endregion

		#region Initialization

		public Game1 ()
		{

			graphics = new GraphicsDeviceManager (this);
			
			Content.RootDirectory = "Assets";

			graphics.IsFullScreen = false;
		}

		/// <summary>
		/// Overridden from the base Game.Initialize. Once the GraphicsDevice is setup,
		/// we'll use the viewport to initialize some values.
		/// </summary>
		protected override void Initialize ()
		{
			base.Initialize ();
		}


		/// <summary>
		/// Load your graphics content.
		/// </summary>
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be use to draw textures.
			spriteBatch = new SpriteBatch (graphics.GraphicsDevice);
			
			// TODO: use this.Content to load your game content here eg.
			logoTexture = Content.Load<Texture2D> ("logo");

			Dictionary<string, object> load_data = CreatureModule.Utils.LoadCreatureJSONData("mageTest.json");
			curCreature = new CreatureModule.Creature(ref load_data);
			curManager = new CreatureModule.CreatureManager (curCreature);

			curManager.CreateAnimation (ref load_data, "default");
			curManager.CreateAnimation (ref load_data, "standing");

			curAnimationName = "default";
			curManager.SetActiveAnimationName (curAnimationName);
			curManager.SetIsPlaying (true);
			curManager.should_loop = true;
			/*
			curManager.use_custom_time_range = true;
			curManager.custom_start_time = 10;
			curManager.custom_end_time = 30;
			*/

			Texture2D curTexture;
			curTexture = Content.Load<Texture2D> ("character-mage");

			curRenderer = new CreatureRenderer.Renderer (graphics.GraphicsDevice, curManager, ref curTexture);
			curRenderer.world = Matrix.CreateScale (new Vector3(35.0f, 35.0f, 1.0f));
		}

		#endregion

		#region Update and Draw

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			// TODO: Add your update logic here		
			curManager.Update (0.025f);
            		
			base.Update (gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself. 
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			// Clear the backbuffer
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);

			spriteBatch.Begin ();

			// draw the logo
			spriteBatch.Draw (logoTexture, new Vector2 (130, 200), Color.White);

			spriteBatch.End ();

			//TODO: Add your drawing code here
			base.Draw (gameTime);

			curRenderer.DoUpdate (graphics.GraphicsDevice);
		}

		#endregion
	}
}
