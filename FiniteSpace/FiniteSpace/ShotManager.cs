using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FiniteSpace {
    class ShotManager {
        public List<Sprite> Shots = new List<Sprite>();
        private Rectangle _screenBounds;
        private static Texture2D _texture;
        private static Rectangle _initialFrame;
        private static int _frameCount;
        private float _shotSpeed;
        private static int _collitionRadius;

        public ShotManager(Texture2D texture, Rectangle initialFrame, int frameCount, int collisionRadius, float shotSpeed, Rectangle screenBounds) {
            _texture = texture;
            _initialFrame = initialFrame;
            _collitionRadius = collisionRadius;
            _shotSpeed = shotSpeed;
            _screenBounds = screenBounds;
            _frameCount = frameCount;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime) {
            for(int x = 0; x < Shots.Count; x++) {
                Shots[x].Update(gameTime);
                if(!_screenBounds.Intersects(Shots[x].Destination)) {
                    Shots.RemoveAt(x);
                }
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            foreach(Sprite shot in Shots) {
                shot.Draw(spriteBatch);
            }
        }



        /// <summary>
        /// Creates a shot fired by an enemy
        /// </summary>
        /// <param name="location">The initial location where the shot is fired from</param>
        /// <param name="velocity">The velocity of the shot</param>
        /// <param name="playerFired"></param>
        public void FireShot(Vector2 location, Vector2 velocity, bool playerFired) {
            Sprite thisShot = new Sprite(location, _texture, _initialFrame, velocity);
            thisShot.Velocity *= _shotSpeed;

            for(int x = 1; x < _frameCount; x++) {
                thisShot.AddFrame(new Rectangle(
                    _initialFrame.X + (_initialFrame.Width + x),
                    _initialFrame.Y,
                    _initialFrame.Width,
                    _initialFrame.Height));
            }

            thisShot.collisionRadius = _collitionRadius;
            Shots.Add(thisShot);

            // play a shot sound effect
            if (playerFired)
                SoundManager.PlayPlayerShot();
            else
                SoundManager.PlayEnemyShot();
        } // end fireShot()
    }
}
