using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FiniteSpace {
    class PlayerSprite : Sprite {
        private SpriteEffects _effect = SpriteEffects.None;
        private Vector2 _lastLocation;
        private int _centerFrame = 0;

        public PlayerSprite(Vector2 location, Texture2D texture, Rectangle initialFrame, Vector2 velocity, int frameCount)
            : base(location, texture, initialFrame, velocity) {

            _lastLocation = location;
            _centerFrame = frameCount / 2; // this will get us our base frame
        }



        public override void Update(GameTime gameTime) {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _timeForCurrentFrame += elapsed;


            _lastLocation = _location;
            _location += (Velocity * elapsed);

            if (_timeForCurrentFrame >= _frameTime) {


                // the player is not moving in the x-axis
                if (_lastLocation.X == _location.X) {
                    if (Frame < _centerFrame)
                        Frame += 1;

                    if (Frame > _centerFrame)
                        Frame -= 1;
                }

                // the player is moving to the left
                if (_location.X < _lastLocation.X) {
                    Frame -= 1;
                }

                // the player is moving to the right
                if (_location.X > _lastLocation.X) {
                    Frame += 1;
                }

                _timeForCurrentFrame = 0.0f;
            }

        }


        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, Center, Source, TintColor, Rotation,
              new Vector2(_frameWidth / 2, _frameHeight / 2),
              1.0f, _effect, 0.0f);
        }
    }
}
