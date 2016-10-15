// ------------------------------------------------------------------------------
//  Created by Kestrel Moon Studios. C# Engine. 
//  Copyright (c) 2015 Kestrel Moon Studios. All rights reserved.
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Collections.Generic;
using CreatureModule;
using MeshBoneUtil;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CreatureRenderer
{
	public class Renderer 
	{
		private VertexPositionNormalTexture[] renderData;
		private VertexBuffer renderBuffer;
		private IndexBuffer indexBuffer;
		private Texture2D renderTexture;
		public BasicEffect basicEffect;
		public CreatureManager creature_manager;

        public Matrix world = Matrix.Identity; // Matrix.CreateTranslation(0.5F, 0.5F, 0);
        public Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        public Matrix projection;// Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.01f, 100f);
        public Matrix spriteBatchConversionMatrix;
        private GraphicsDevice device;

        public Renderer(GraphicsDevice device,
			CreatureManager manager_in,
			ref Texture2D texture_in)
		{

          //  projection = Matrix.CreateOrthographic(40 * device.Viewport.AspectRatio, 40, 0, 1);
            //Matrix.CreateOrthographicOffCenter(0, 256, 240, 0, -10, 10);

          

            creature_manager = manager_in;
			renderTexture = texture_in;

			CreateRenderingData(device);
            
            spriteBatchConversionMatrix = Matrix.CreateScale(1, 1, 1) * Matrix.CreateTranslation(device.Viewport.Width / 2, device.Viewport.Height / 2, 0F) ;

            this.device = device;
        }


        public Vector2 Project(Vector3 v3)
        {
            var screenPositon = device.Viewport.Project(v3, projection, view, world);
            return new Vector2(screenPositon.X, screenPositon.Y);
        }

        //Vector2 ToScreenCoordinates(Vector2 worldCoords)
        //{
        //    var screenPositon = graphics.GraphicsDevice.Viewport.Project(new Vector3(worldCoords, 0),
        //            Projection, View, Matrix.Identity);
        //    return new Vector2(screenPositon.X, screenPositon.Y);
        //}


        /*
	void Start () {
		Reset ();
		
		// Testing
		string filepath = "/Users/jychong/Projects/EngineAppMedia/Test/default.json";
		Dictionary<string, object> load_data = CreatureModule.Utils.LoadCreatureJSONData (filepath);
		
		CreatureModule.Creature new_creature = new CreatureModule.Creature(ref load_data);
		CreatureModule.CreatureManager new_manager = new CreatureModule.CreatureManager (new_creature);
		new_manager.CreateAnimation (ref load_data, "default");
		//new_manager.CreateAnimation (ref load_data, "second");

		new_manager.SetActiveAnimationName ("default");
		new_manager.SetIsPlaying (true);
		
		creature_manager = new_manager;
	}
	*/


        private void CreateRenderingData(GraphicsDevice device)
		{
			indexBuffer = new IndexBuffer(device, 
				typeof(int), 
				creature_manager.target_creature.total_num_indices,
				BufferUsage.WriteOnly);
			List<int> render_indices = creature_manager.target_creature.global_indices;
			int[] index_data = new int[render_indices.Count];
			for (int i = 0; i < render_indices.Count; i++) 
			{
				index_data[i] = render_indices[i];
			}

			indexBuffer.SetData(index_data);

			renderData = new VertexPositionNormalTexture[creature_manager.target_creature.total_num_pts];
			renderBuffer = new DynamicVertexBuffer (device, VertexPositionNormalTexture.VertexDeclaration, renderData.Length, BufferUsage.WriteOnly);
			renderBuffer.SetData (renderData);										

			basicEffect = new BasicEffect(device);
			basicEffect.Texture = renderTexture;
			basicEffect.TextureEnabled = true;

			// Update matrices
			projection = Matrix.CreateOrthographic(device.Viewport.Width, device.Viewport.Height, 1.0f, 1000.0f);

            //projection = Matrix.CreateOrthographic(device.Viewport.Width, device.Viewport.Height, 1.0f, 1000.0f);

        }

		private void DrawRenderingData(GraphicsDevice device)
		{
			int pt_index = 0;
			int uv_index = 0;

			List<float> render_pts = creature_manager.target_creature.render_pts;
			List<float> render_uvs = creature_manager.target_creature.global_uvs;

			for(int i = 0; i < creature_manager.target_creature.total_num_pts; i++)
			{
				renderData[i].Position.X = render_pts[pt_index + 0];
				renderData[i].Position.Y = render_pts[pt_index + 1];
				renderData[i].Position.Z = render_pts[pt_index + 2];

				renderData[i].TextureCoordinate.X = render_uvs[uv_index + 0];
				renderData[i].TextureCoordinate.Y = render_uvs[uv_index + 1];
				/*
				renderData[i].Normal.X = 0;
				renderData[i].Normal.Y = 0;
				renderData[i].Normal.Z = 1;
				*/
				pt_index += 3;
				uv_index += 2;
			}


			// Render mesh
			basicEffect.World = world;
			basicEffect.View = view;
			basicEffect.Projection = projection;
			basicEffect.VertexColorEnabled = false;

			device.Indices = indexBuffer;
            device.BlendState = BlendState.AlphaBlend;

			renderBuffer.SetData (renderData);										
			device.SetVertexBuffer (renderBuffer);

			RasterizerState rasterizerState1 = new RasterizerState();
			rasterizerState1.CullMode = CullMode.None;
			device.RasterizerState = rasterizerState1;

			foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
			{
				pass.Apply ();
				device.DrawIndexedPrimitives (PrimitiveType.TriangleList, 
					0, 
					0,
					renderData.Length,
					0, 
					creature_manager.target_creature.total_num_indices / 3);

                
			}

			//debugDrawBones (creature_manager.target_creature.render_composition.getRootBone ());
		}

		public void debugDrawBones(MeshBone bone_in)
		{
			/*
		Vector4 pt1 = bone_in.world_start_pt;
		Vector4 pt2 = bone_in.world_end_pt;
		*/
        
			/*
			Debug.DrawLine (new Vector3((float)pt1.X, (float)pt1.Y, 0), 
				new Vector3((float)pt2.X, (float)pt2.Y, 0), Color.white);
			*/

			foreach(MeshBone cur_child in bone_in.children)
			{
				debugDrawBones(cur_child);
			}
		}

		public void DoUpdate(GraphicsDevice device)
		{
			DrawRenderingData(device);
		}

	}

}