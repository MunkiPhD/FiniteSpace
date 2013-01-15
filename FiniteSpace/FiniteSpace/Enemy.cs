using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FiniteSpace {
    class Enemy {
        public Sprite EnemySprite;
        public Vector2 GunOffset = new Vector2(25, 25);
        public bool Destroyed = false;

        private Queue<Vector2> _waypoints = new Queue<Vector2>();
        private Vector2 _currentWaypoint;
        private float _speed = 120f;
        private int _enemyRadius = 15;
        private Vector2 _previousLocation = Vector2.Zero;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="texture">The spritesheet</param>
        /// <param name="location">Initial location of the enemy</param>
        /// <param name="initialFrame">The initial frame in the spritesheet</param>
        /// <param name="frameCount">Number of frames in the animation</param>
        public Enemy(Texture2D texture, Vector2 location, Rectangle initialFrame, int frameCount) {
            EnemySprite = new Sprite(location, texture, initialFrame, Vector2.Zero);

            for(int x = 1; x < frameCount; x++) {
                EnemySprite.AddFrame(new Rectangle(
                    initialFrame.X - (initialFrame.Width * x),
                    initialFrame.Y,
                    initialFrame.Width,
                    initialFrame.Height));
            }

            _previousLocation = location;
            _currentWaypoint = location;
            EnemySprite.collisionRadius = _enemyRadius;
        }



        /// <summary>
        /// Update!
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime) {
            if(IsActive()) {
                Vector2 heading = _currentWaypoint - EnemySprite.Location;

                if(heading != Vector2.Zero) {
                    heading.Normalize();
                }

                heading *= _speed;
                EnemySprite.Velocity = heading;
                _previousLocation = EnemySprite.Location;
                EnemySprite.Update(gameTime);
                EnemySprite.Rotation = (float)Math.Atan2(EnemySprite.Location.Y - _previousLocation.Y, EnemySprite.Location.X - _previousLocation.X);

                if(WaypointReached()) {
                    if(_waypoints.Count > 0)
                        _currentWaypoint = _waypoints.Dequeue();
                }
            }
        } // end update



        /// <summary>
        /// Draw!
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            if(IsActive())
                EnemySprite.Draw(spriteBatch);
        }



        /// <summary>
        /// Adds a waypoint to this enemy's queue
        /// </summary>
        /// <param name="waypoint"></param>
        public void AddWaypoint(Vector2 waypoint) {
            _waypoints.Enqueue(waypoint);
        }



        /// <summary>
        /// Determines whether a waypoint has been reached
        /// </summary>
        /// <returns></returns>
        public bool WaypointReached() {
            if(Vector2.Distance(EnemySprite.Location, _currentWaypoint) < (float)EnemySprite.Source.Width / 2)
                return true;
            else
                return false;
        }



        /// <summary>
        /// Determines if the enemy is still active
        /// </summary>
        /// <returns></returns>
        public bool IsActive() {
            if(Destroyed)
                return false;

            if(_waypoints.Count > 0)
                return true;

            if(WaypointReached())
                return false;

            return true;
        }
    } // end class
} // end namespace
