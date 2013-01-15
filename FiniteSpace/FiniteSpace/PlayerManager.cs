using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace FiniteSpace {
    class PlayerManager {
        public Sprite PlayerSprite;
        public ShotManager PlayerShotManager;
        public long PlayerScore = 0;
        public int LivesRemaining = 3;
        public bool Destroyed = false;

        private float _playerSpeed = 160.0f;
        private Rectangle _playerAreaLimit;
        private Vector2 _gunOffset = new Vector2(25, 10);
        private float _shotTimer = 0.0f;
        private float _minShotTimer = 0.2f;
        private int _playerRadius = 15;

        public PlayerManager(Texture2D texture, Rectangle initialFrame, int frameCount, Rectangle screenBounds) {
            PlayerSprite = new Sprite(new Vector2(500, 500), texture, initialFrame, Vector2.Zero);
            PlayerShotManager = new ShotManager(texture, new Rectangle(0, 300, 5, 5), 4, 2, 250f, screenBounds);
            _playerAreaLimit = new Rectangle(0, screenBounds.Height / 2, screenBounds.Width, screenBounds.Height / 2);

            for(int x = 1; x < frameCount; x++) {
                PlayerSprite.AddFrame(new Rectangle(
                    initialFrame.X + (initialFrame.Width + x),
                    initialFrame.Y,
                    initialFrame.Width,
                    initialFrame.Height));
            }

            PlayerSprite.collisionRadius = _playerRadius;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime) {
            PlayerShotManager.Update(gameTime);
            
            if(!Destroyed) {
                PlayerSprite.Velocity = Vector2.Zero;
                _shotTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                // handle input
                HandleKeyboardInput(Keyboard.GetState());
                HandleGamepadInput(GamePad.GetState(PlayerIndex.One));

                PlayerSprite.Velocity.Normalize();
                PlayerSprite.Velocity *= _playerSpeed;
                PlayerSprite.Update(gameTime);
                ImposeMovementLimits();
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            PlayerShotManager.Draw(spriteBatch);
            
            if(!Destroyed) // if not dead, draw the player
                PlayerSprite.Draw(spriteBatch);
        }


        /// <summary>
        /// Restricts the users movements within the bounds
        /// </summary>
        private void ImposeMovementLimits() {
            Vector2 location = PlayerSprite.Location;
            
            if(location.X < _playerAreaLimit.X)
                location.X = _playerAreaLimit.X;

            if(location.X > (_playerAreaLimit.Right - PlayerSprite.Source.Width))
                location.X = _playerAreaLimit.Right - PlayerSprite.Source.Width;

            if(location.Y < _playerAreaLimit.Y)
                location.Y = _playerAreaLimit.Y;

            if(location.Y > (_playerAreaLimit.Bottom - PlayerSprite.Source.Height))
                location.Y = _playerAreaLimit.Bottom - PlayerSprite.Source.Height;

            PlayerSprite.Location = location;
        }



        /// <summary>
        /// Fires a shot from starboard side!
        /// </summary>
        private void FireShot() {
            if(_shotTimer >= _minShotTimer) {
                PlayerShotManager.FireShot(PlayerSprite.Location + _gunOffset, new Vector2(0, -1), true);
                _shotTimer = 0.0f;
            }
        }


        /// <summary>
        /// handles keyboard input
        /// </summary>
        private void HandleKeyboardInput(KeyboardState keyState) {
            if(keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
                PlayerSprite.Velocity += new Vector2(0, -1);

            if(keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
                PlayerSprite.Velocity += new Vector2(0, 1);

            if(keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))
                PlayerSprite.Velocity += new Vector2(-1, 0);

            if(keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))
                PlayerSprite.Velocity += new Vector2(1, 0);

            if(keyState.IsKeyDown(Keys.Space))
                FireShot();
        }



        /// <summary>
        /// Handles gamepad input
        /// </summary>
        /// <param name="gamePadState"></param>
        private void HandleGamepadInput(GamePadState gamePadState) {
            PlayerSprite.Velocity += new Vector2(gamePadState.ThumbSticks.Left.X, -gamePadState.ThumbSticks.Left.Y);

            if(gamePadState.Buttons.A == ButtonState.Pressed)
                FireShot();
        }
    } // end class
} // end namespace
