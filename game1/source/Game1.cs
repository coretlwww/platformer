﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using TiledSharp;

namespace game1.source
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        #region Player
        private Player _player;
        #endregion

        #region Tilemap
        private TmxMap map;
        private TilemapManager tilemapManager;
        private Texture2D tileset;
        private List<Rectangle> collisionRectangles;
        private Rectangle startRect;
        private Rectangle endRect;
        #endregion

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            #region Player
            _player = new Player(
                new Vector2(startRect.X, startRect.Y),
                Content.Load<Texture2D>("idle"),
                Content.Load<Texture2D>("run"),
                Content.Load<Texture2D>("jump"),
                Content.Load<Texture2D>("jump"));
            #endregion

            #region Tilemap
            map = new TmxMap("Content\\lvl1.tmx");
            tileset = Content.Load<Texture2D>("for_map\\" + map.Tilesets[0].Name.ToString());
            int tileWidth = map.Tilesets[0].TileWidth;
            int tileHeight = map.Tilesets[0].TileHeight;
            int tilesetTileWidth = tileset.Width / tileWidth;

            tilemapManager = new TilemapManager(map, tileset, tilesetTileWidth, tileWidth, tileHeight);
            #endregion

            collisionRectangles = new List<Rectangle>();

            foreach (var obj in map.ObjectGroups["Collisions"].Objects)
            {
                if (obj.Name == "")
                {
                    collisionRectangles.Add(new Rectangle((int)obj.X, (int)obj.Y,(int) obj.Width, (int)obj.Height));
                }
                if (obj.Name == "Start")
                {
                    startRect = new Rectangle((int)obj.X, (int)obj.Y, (int)obj.Width, (int)obj.Height);
                }
                if (obj.Name == "End")
                {
                    startRect = new Rectangle((int)obj.X, (int)obj.Y, (int)obj.Width, (int)obj.Height);
                }
            }

            
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var initialPosition = _player.position;
           
            _player.Update();

            #region Player Collisions
            //y-axis
            foreach (var rect in collisionRectangles)
            {
                if (!_player.isJumping)
                _player.isFalling = true;
                if (rect.Intersects(_player.playerFallRect))
                {
                    _player.isFalling = false;
                    break;
                }
            }

            //x-axis
            foreach (var rect in collisionRectangles)
            {
                if (rect.Intersects(_player.hitBox))
                {
                    _player.position.X = initialPosition.X;
                    _player.velocity.X = initialPosition.X;
                    break;
                }
            }
            #endregion

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            tilemapManager.Draw(_spriteBatch);
            _player.Draw(_spriteBatch, gameTime);
            _spriteBatch.End();
          
            base.Draw(gameTime);
        }
    }
}