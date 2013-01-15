using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace FiniteSpace {
    class AsteroidManager {
        private int _screenWidth = 800;
        private int _screenHeight = 600;
        private int _screenPadding = 10;
        private Rectangle initialFrame;
        private int _asteroidFrames;
        private Texture2D _texture;

        public List<Sprite> Asteroids = new List<Sprite>();
        private int _minSpeed = 60;
        private int _maxSpeed = 120;
        private Random rand = new Random();



        public AsteroidManager(int asteroidCount, Texture2D texture, Rectangle initialFrame, int asteroidFrames, int screenWidth, int screenHeight) {
            _texture = texture;
            this.initialFrame = initialFrame;
            this._asteroidFrames = asteroidFrames;
            this._screenWidth = screenWidth;
            this._screenHeight = screenHeight;

            for(int x = 0; x < asteroidCount; x++) {
                AddAsteroid();
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime) {
            foreach(Sprite asteroid in Asteroids) {
                asteroid.Update(gameTime);
                if(!IsOnScreen(asteroid)) {
                    asteroid.Location = RandomLocation();
                    asteroid.Velocity = RandomVelocity();
                }
            }

            for(int x = 0; x < Asteroids.Count; x++) {
                for(int y = x + 1; y < Asteroids.Count; y++) {
                    if(Asteroids[x].IsCircleColliding(Asteroids[y].Center, Asteroids[y].collisionRadius)){
                        BounceAsteroids(Asteroids[x], Asteroids[y]);
                    }
                }
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            foreach(Sprite asteroid in Asteroids) {
                asteroid.Draw(spriteBatch);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="asteroidOne"></param>
        /// <param name="asteroidTwo"></param>
        private void BounceAsteroids(Sprite asteroidOne, Sprite asteroidTwo) {
            Vector2 centerOfMass = (asteroidOne.Velocity + asteroidTwo.Velocity) / 2;
            Vector2 normal1 = asteroidTwo.Center - asteroidOne.Center;
            normal1.Normalize();

            Vector2 normal2 = asteroidOne.Center - asteroidTwo.Center;
            normal2.Normalize();

            asteroidOne.Velocity -= centerOfMass;
            asteroidOne.Velocity = Vector2.Reflect(asteroidOne.Velocity, normal1);

            asteroidOne.Velocity += centerOfMass;
            asteroidTwo.Velocity -= centerOfMass;

            asteroidTwo.Velocity = Vector2.Reflect(asteroidTwo.Velocity, normal2);
            asteroidTwo.Velocity += centerOfMass;
        }



        /// <summary>
        /// Generates a random location for an asteroid
        /// </summary>
        /// <returns></returns>
        private Vector2 RandomLocation() {
            Vector2 location = Vector2.Zero;
            bool locationOkay = true;
            int tryCount = 0;

            do {
                locationOkay = true;
                switch(rand.Next(0, 3)) {
                    case 0:
                        location.X = -initialFrame.Width;
                        location.Y = rand.Next(0, _screenHeight);
                        break;
                    case 1:
                        location.X = _screenWidth;
                        location.Y = rand.Next(0, _screenHeight);
                        break;
                    case 2:
                        location.X = rand.Next(0, _screenWidth);
                        location.Y = -initialFrame.Height;
                        break;
                }

                foreach(Sprite asteroid in Asteroids) {
                    if(asteroid.IsBoxColliding(new Rectangle((int)location.X, (int)location.Y, initialFrame.Width, initialFrame.Height))) {
                        locationOkay = false;
                    }
                }

                if((tryCount > 5) && (locationOkay == false)) {
                    location = new Vector2(-500, -500);
                    locationOkay = true;
                }
            } while(locationOkay == false);

            return location;

        }



        /// <summary>
        /// Generates a random velocity for an asteroid
        /// </summary>
        /// <returns></returns>
        private Vector2 RandomVelocity() {
            Vector2 velocity = new Vector2(rand.Next(0, 101) - 50, rand.Next(0, 101) - 50);
            velocity.Normalize();
            velocity *= rand.Next(_minSpeed, _maxSpeed);

            return velocity;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="asteroid"></param>
        /// <returns></returns>
        private bool IsOnScreen(Sprite asteroid) {
            if(asteroid.Destination.Intersects(new Rectangle(-_screenPadding, -_screenPadding, _screenWidth + _screenPadding, _screenHeight + _screenPadding))) {
                return true;
            } else {
                return false;
            }

        }



        /// <summary>
        /// Adds an asteroid! duh!
        /// </summary>
        public void AddAsteroid() {
            Sprite newAsteroid = new Sprite(new Vector2(-500, -500), _texture, initialFrame, Vector2.Zero);

            for(int x = 1; x < _asteroidFrames; x++) {
                newAsteroid.AddFrame(new Rectangle(initialFrame.X + (initialFrame.Width * x), initialFrame.Y, initialFrame.Width, initialFrame.Height));
            }

            newAsteroid.Rotation = MathHelper.ToRadians((float)rand.Next(0, 360));
            newAsteroid.collisionRadius = 15;
            Asteroids.Add(newAsteroid);
        }



        /// <summary>
        /// Clears all the asteroids from the field
        /// </summary>
        public void Clear() {
            Asteroids.Clear();
        }

    }
}
