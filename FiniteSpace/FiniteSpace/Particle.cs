using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FiniteSpace {
    class Particle : Sprite {
        private Vector2 _acceleration;
        private float _maxSpeed;
        private int _initialDuration;
        private int _remainingDuration;
        private Color _initialColor;
        private Color _finalColor;


        /// <summary>
        /// Creates a particle for particle effects
        /// </summary>
        /// <param name="location">The location where the particle effect should begin it's animation</param>
        /// <param name="texture">The sprite sheet</param>
        /// <param name="initialFrame">The first frame of the animation</param>
        /// <param name="velocity">The velocity with which it will explode at outwards?</param>
        /// <param name="acceleration">The acceleration of the explosion?</param>
        /// <param name="maxSpeed">The max speed of the explosion</param>
        /// <param name="duration">The duration of the particle effect</param>
        /// <param name="initialColor">The initial color of the tint to the sprite</param>
        /// <param name="finalColor">The final color of the tint to the sprite</param>
        public Particle(Vector2 location, Texture2D texture, Rectangle initialFrame, Vector2 velocity, Vector2 acceleration, float maxSpeed, int duration, Color initialColor, Color finalColor)
            : base(location, texture, initialFrame, velocity) {
            _initialDuration = duration;
            _remainingDuration = duration;
            _acceleration = acceleration;
            _initialColor = initialColor;
            _maxSpeed = maxSpeed;
            _finalColor = finalColor;
        }


        /// <summary>
        /// Update!
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            if (isActive) {
                _velocity += _acceleration;

                if (_velocity.Length() > _maxSpeed) {
                    _velocity.Normalize();
                    _velocity *= _maxSpeed;
                }

                TintColor = Color.Lerp(_initialColor, _finalColor, DurationProgress);
                _remainingDuration--;
                base.Update(gameTime);
            }
        }



        /// <summary>
        /// Draw!
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch) {
            if (isActive)
                base.Draw(spriteBatch);
        }



        /// <summary>
        /// The amount of time that has gone by
        /// </summary>
        public int ElapsedDuration {
            get { return _initialDuration - _remainingDuration; }
        } // end elapsedDuration


        /// <summary>
        /// The progress of where the duration is currently at
        /// </summary>
        public float DurationProgress {
            get { return (float)ElapsedDuration / (float)_initialDuration; }
        } // end durationProgress


        /// <summary>
        /// If the particle effect is still active
        /// </summary>
        public bool isActive {
            get { return (_remainingDuration > 0); }
        } // end isActive
    }
}
