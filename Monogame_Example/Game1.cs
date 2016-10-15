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
        private Texture2D circleTexture;
        SpriteBatch batch;
        private Texture2D whitePixel;
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

            //graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width;
            //graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;

            graphics.IsFullScreen = true;

            //graphics.ApplyChanges();
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

            Dictionary<string, object> load_data = CreatureModule.Utils.LoadCreatureJSONData(basePath + /*"\\Content\\iceDemonExport_character_data.json"*/"\\Content\\iceDemonExport_character_data.json");
           // Dictionary<string, object> load_data = CreatureModule.Utils.LoadCreatureFlatDataFromBytes(basePath + /*"\\Content\\iceDemonExport_character_data.json"*/"\\Content\\gHorsemanExport_character_data.bytes");
            curCreature = new CreatureModule.Creature(ref load_data);
            curManager = new CreatureModule.CreatureManager(curCreature);
            batch = new SpriteBatch(this.GraphicsDevice);


            curManager.CreateAnimation(ref load_data, "default");

            curAnimationName = "default";
            curManager.SetActiveAnimationName(curAnimationName);
            curManager.SetIsPlaying(true);
            curManager.should_loop = true;

            //curManager.use_custom_time_range = true;
            //curManager.custom_start_time = 10;
            //curManager.custom_end_time = 30;


            Texture2D curTexture;
            curTexture = Content.Load<Texture2D>("ice.png");

            curRenderer = new CreatureRenderer.Renderer(graphics.GraphicsDevice, curManager, ref curTexture);
            curRenderer.world = Matrix.CreateScale(new Vector3(25.0f, 25.0f, 1.0f));

            this.circleTexture = CreateCircle(30);

            whitePixel = new Texture2D(GraphicsDevice, 1, 1);
            whitePixel.SetData<Color>(
                new Color[] { Color.White });
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
            //   this.
            // curManager.SetIsPlaying(false);

            base.Update(gameTime);
        }

        //public Matrix world = Matrix.CreateTranslation(0.5F, 0.5F, 0);
        //public Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        //public Matrix projection = Matrix.CreateOrthographicOffCenter(0, 256, 240, 0, -10, 10);// Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.01f, 100f);


        //scale(40/viewport.Height) * scale(1, -1) * translate(viewport.Width/2, viewport.Height/2)

        /// <summary>
        /// This is called when the game should draw itself. 
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Clear the backbuffer
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);


            //    }

            //TODO: Add your drawing code here
            base.Draw(gameTime);

            curRenderer.DoUpdate(graphics.GraphicsDevice);



            batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, null);
            //foreach (var r in curCreature.render_composition.regions)
            //{
            //    if (r.outline_polygon != null)
            //        for (int i = 0; i < r.outline_polygon.Length; i++)// var pt in r.outline_polygon)
            //        {
            //            // batch.Draw(circleTexture, pt, Color.Blue);
            //            batch.DrawLine(curRenderer.Project(new Vector3(r.outline_polygon[i], 0)), i == r.outline_polygon.Length - 1 ? curRenderer.Project(new Vector3(r.outline_polygon[0], 0)) : curRenderer.Project(new Vector3(r.outline_polygon[i + 1], 0)), Color.Green, 2F);
            //        }
            //    //if (r.name == "torso")
            //    //    foreach (var p in r.indices_by_vertex)
            //    //    {
            //    //        Color c = Color.Black;

            //    //        switch (p.Value.Count)
            //    //        {
            //    //            case 5: c = Color.Brown; break;
            //    //            case 4: c = Color.Purple; break;
            //    //            case 3: c = Color.Yellow; break;
            //    //            case 2: c = Color.Orange; break;
            //    //            case 1: c = Color.Green; break;
            //    //        }
            //    //        batch.DrawCircle(curRenderer.Project(new Vector3(r.store_rest_pts[p.Key * 3], r.store_rest_pts[p.Key * 3 + 1], 0)), 2F, 5, c);
            //    //    }
            //}


            //foreach(var r  in curCreature.render_composition.regions)
            //{
            //    foreach(var )
            //}

            batch.End();

        }


        public Texture2D CreateCircle(int radius)
        {
            int outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds
            Texture2D texture = new Texture2D(GraphicsDevice, outerRadius, outerRadius);

            Color[] data = new Color[outerRadius * outerRadius];

            // Colour the entire texture transparent first.
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Transparent;

            // Work out the minimum step necessary using trigonometry + sine approximation.
            double angleStep = 1f / radius;

            for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
            {
                // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
                int x = (int)Math.Round(radius + radius * Math.Cos(angle));
                int y = (int)Math.Round(radius + radius * Math.Sin(angle));

                data[y * outerRadius + x + 1] = Color.White;
            }

            texture.SetData(data);
            return texture;
        }


        #endregion
    }
}