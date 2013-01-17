using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace FiniteSpace {
    class ExplosionManager {
        private Texture2D _texture;
        private List<Rectangle> _pieceRectangles = new List<Rectangle>();
        private Rectangle _pointRectangle;
        private int _minPieceCount = 3;
        private int _maxPieceCount = 6;
        private int _minPointCount = 20;
        private int _maxPointCount = 30;
        private int _durationCount = 90;
        private float _explosionMaxSpeed = 30f;
        private float _pieceSpeedScale = 6f;
        private int _pointSpeedMin = 15;
        private int _pointSpeedMax = 30;
        private Color _initialColor = new Color(1.0f, 0.3f, 0f) * 0.5f;
        private Color _finalColor = new Color(0f, 0f, 0f, 0f);
        Random rand = new Random();
        private List<Particle> ExplosionParticles = new List<Particle>();


        /// <summary>
        /// Creates a new manager instance with cached values for explosions
        /// </summary>
        /// <param name="texture">The sprite sheet</param>
        /// <param name="initialFrame">The first frame of the sprites</param>
        /// <param name="pieceCount">The number of individual explosion sprites</param>
        /// <param name="pointRectangle">no idea what this will be used for...</param>
        public ExplosionManager(Texture2D texture, Rectangle initialFrame, int pieceCount, Rectangle pointRectangle) {
            _texture = texture;

            for (int x = 0; x < pieceCount; x++)
                _pieceRectangles.Add(new Rectangle(initialFrame.X + (initialFrame.Width * x), initialFrame.Y, initialFrame.Width, initialFrame.Height));

            _pointRectangle = pointRectangle;
        } // end constructor



        /// <summary>
        /// Update!
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime) {
            for (int x = ExplosionParticles.Count - 1; x >= 0; x--) {
                if (ExplosionParticles[x].isActive)
                    ExplosionParticles[x].Update(gameTime);
                else
                    ExplosionParticles.RemoveAt(x);
            }
        }



        /// <summary>
        /// Draw!
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            foreach (Particle particle in ExplosionParticles)
                particle.Draw(spriteBatch);
        }



        /// <summary>
        /// Generates a random direction based on the scale
        /// </summary>
        /// <param name="scale">Scale betwee 0 and 1 with how big to make it go in the specified random direction</param>
        /// <returns></returns>
        public Vector2 RandomDirection(float scale) {
            Vector2 direction;
            do {
                direction = new Vector2(rand.Next(0, 101) - 50, rand.Next(0, 101) - 50);
            } while (direction.Length() == 0);

            direction.Normalize();
            direction *= scale;

            return direction;
        } // end RandomDirection()


        /// <summary>
        /// Adds an explosion to the particle effect queue
        /// </summary>
        /// <param name="location">Location where explosion should begin</param>
        /// <param name="momentum">The momentum of the explosion</param>
        public void AddExplosion(Vector2 location, Vector2 momentum) {
            // we want to place the middle of the explosion at the location specified, so get half the width and height
            Vector2 pieceLocation = location - new Vector2(_pieceRectangles[0].Width / 2, _pieceRectangles[0].Height / 2); 
            int pieces = rand.Next(_minPieceCount, _maxPieceCount + 1);

            // add a sprite animation piece for each number of random pieces generated to be added
            for (int x = 0; x < pieces; x++) {
                ExplosionParticles.Add(new Particle(
                    pieceLocation,
                    _texture,
                    _pieceRectangles[rand.Next(0, _pieceRectangles.Count)],
                    RandomDirection(_pieceSpeedScale) + momentum,
                    Vector2.Zero,
                    _explosionMaxSpeed,
                    _durationCount,
                    _initialColor,
                    _finalColor));
            }

            int points = rand.Next(_minPointCount, _maxPointCount + 1);

            // this adds the actual pieces
            for (int x = 0; x < points; x++) {
                ExplosionParticles.Add(new Particle(
                    location,
                    _texture,
                    _pointRectangle,
                    RandomDirection((float)rand.Next(_pointSpeedMin, _pointSpeedMax)) + momentum,
                    Vector2.Zero,
                    _explosionMaxSpeed,
                    _durationCount,
                    _initialColor,
                    Color.RosyBrown));
            }

            // play a sound for the explosion
            SoundManager.PlayExplosion();
        }
    }
}
