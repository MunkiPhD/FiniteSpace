using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FiniteSpace {
    class Sprite {
        public Texture2D texture;
        protected List<Rectangle> frames = new List<Rectangle>();
        protected int _frameWidth = 0;
        protected int _frameHeight = 0;
        private int _currentFrame = 0;
        protected float _frameTime = 0.1f;
        protected float _timeForCurrentFrame = 0.0f;
        private Color _tintColor = Color.White;
        protected float _rotation = 0.0f;

        public int collisionRadius = 0;
        public int boundingXPadding = 0;
        public int boundingYPadding = 0;

        protected Vector2 _location = Vector2.Zero;
        protected Vector2 _velocity = Vector2.Zero;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="location"></param>
        /// <param name="texture"></param>
        /// <param name="initialFrame"></param>
        /// <param name="velocity"></param>
        public Sprite(Vector2 location, Texture2D texture, Rectangle initialFrame, Vector2 velocity) {
            this._location = location;
            this.texture = texture;
            this._velocity = velocity;
            frames.Add(initialFrame);
            _frameWidth = initialFrame.Width;
            _frameHeight = initialFrame.Height;
        }



        /// <summary>
        /// Updates the current sprite
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime) {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _timeForCurrentFrame += elapsed;

            if (_timeForCurrentFrame >= _frameTime) {
                _currentFrame = (_currentFrame + 1) % (frames.Count);
                _timeForCurrentFrame = 0.0f;
            }

            _location += (Velocity * elapsed);
        }



        /// <summary>
        /// Draw!
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, Center, Source, TintColor, Rotation,
                new Vector2(_frameWidth / 2, _frameHeight / 2),
                1.0f, SpriteEffects.None, 0.0f);
        }



        /// <summary>
        /// Checks whether an object of type rectangle is colliding with this object
        /// </summary>
        /// <param name="otherBox">The bounding box of another item to check against collition</param>
        /// <returns>True if the rectangle intersects this object, false otherwise</returns>
        public bool IsBoxColliding(Rectangle otherBox) {
            return BoundingBoxRect.Intersects(otherBox);
        }



        /// <summary>
        /// Checks whether an object of type circle is intersecting with this object
        /// </summary>
        /// <param name="otherCenter">The center of the object</param>
        /// <param name="otherRadius">The radius of the object</param>
        /// <returns>True if they intersect, false otherwise</returns>
        public bool IsCircleColliding(Vector2 otherCenter, float otherRadius) {
            if (Vector2.Distance(Center, otherCenter) < (collisionRadius + otherRadius))
                return true;
            else
                return false;
        }



        /// <summary>
        /// Adds a frame the the sprite's animation
        /// </summary>
        /// <param name="frameRectangle">The rectangle to add to the frames list</param>
        public void AddFrame(Rectangle frameRectangle) {
            frames.Add(frameRectangle);
        }

        #region Getters and Setters

        public Rectangle BoundingBoxRect {
            get {
                return new Rectangle((int)_location.X + boundingXPadding,
                    (int)_location.Y + boundingYPadding,
                    _frameWidth - (boundingXPadding * 2),
                    _frameHeight - (boundingYPadding * 2));
            }
        }




        public Vector2 Location {
            get { return _location; }
            set { _location = value; }
        }

        public Vector2 Velocity {
            get { return _velocity; }
            set { _velocity = value; }
        }

        public Color TintColor {
            get { return _tintColor; }
            set { _tintColor = value; }
        }

        public float Rotation {
            get { return _rotation; }
            set { _rotation = value % MathHelper.TwoPi; }
        }


        public int Frame {
            get { return _currentFrame; }
            set { _currentFrame = (int)MathHelper.Clamp(value, 0, frames.Count - 1); }
        }

        public float FrameTime {
            get { return _frameTime; }
            set { _frameTime = MathHelper.Max(0, value); }
        }

        public Rectangle Source {
            get { return frames[_currentFrame]; }
        }

        public Rectangle Destination {
            get { return new Rectangle((int)_location.X, (int)_location.Y, _frameWidth, _frameHeight); }
        }

        public Vector2 Center {
            get { return _location + new Vector2(_frameWidth / 2, _frameHeight / 2); }
        }
        #endregion
    } // end class
} // end namespace
