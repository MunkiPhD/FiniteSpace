using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FiniteSpace {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameStates gameState = GameStates.TitleScreen;
        Texture2D titleScreen;
        Texture2D spriteSheet;
        Texture2D gameBackground;
        StarField starField;
        AsteroidManager asteroidManager;
        PlayerManager playerManager;
        EnemyManager enemyManager;
        ExplosionManager explosionManager;
        CollisionManager collisionManager;

        SpriteFont pericles14;
        private float _playerDeathDelayTime = 5f;
        private float _playerDeathTimer = 0f;
        private float _titleScreenTimer = 0f;
        private float _titleScreenDelayTime = 1f;
        private int _playerStartingLives = 3;
        private Vector2 _playerStartLocation = new Vector2(390, 550);
        private Vector2 _scoreLocation = new Vector2(20, 10);
        private Vector2 _livesLocation = new Vector2(20, 25);

        enum GameStates {
            TitleScreen,
            Playing,
            PlayerDead,
            GameOver
        }



        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }



        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here

            base.Initialize();
        }



        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            titleScreen = Content.Load<Texture2D>(@"Textures\TitleScreen");
            spriteSheet = Content.Load<Texture2D>(@"Textures\spriteSheet");
            gameBackground = Content.Load<Texture2D>(@"Backgrounds\Milkyway");
            pericles14 = Content.Load<SpriteFont>(@"Fonts\Pericles14");

            // initializations
            starField = new StarField(this.Window.ClientBounds.Width, this.Window.ClientBounds.Height, 200, new Vector2(0, 60f), spriteSheet, new Rectangle(0, 450, 2, 2));
            asteroidManager = new AsteroidManager(10, spriteSheet, new Rectangle(0, 0, 50, 50), 20, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
            playerManager = new PlayerManager(spriteSheet, new Rectangle(0, 150, 50, 50), 3, new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height));
            enemyManager = new EnemyManager(spriteSheet, new Rectangle(0, 200, 50, 50), 6, playerManager, new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height));
            explosionManager = new ExplosionManager(spriteSheet, new Rectangle(0, 100, 50, 50), 3, new Rectangle(0, 450, 2, 2));
            collisionManager = new CollisionManager(asteroidManager, playerManager, enemyManager, explosionManager);

            SoundManager.Initialize(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }



        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            switch (this.gameState) {
                case GameStates.TitleScreen:
                    _titleScreenTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (_titleScreenTimer >= _titleScreenDelayTime) {
                        if ((Keyboard.GetState().IsKeyDown(Keys.Space)) || (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)) {
                            playerManager.LivesRemaining = _playerStartingLives;
                            playerManager.PlayerScore = 0;
                            ResetGame();
                            gameState = GameStates.Playing;
                        }
                    }
                    break;

                case GameStates.Playing:
                    starField.Update(gameTime);
                    asteroidManager.Update(gameTime);
                    playerManager.Update(gameTime);
                    enemyManager.Update(gameTime);
                    explosionManager.Update(gameTime);
                    collisionManager.CheckCollisions(); // check collisions after everything else has been updated

                    if (playerManager.Destroyed) {
                        _playerDeathTimer = 0f;
                        enemyManager.Active = false;
                        playerManager.LivesRemaining--;
                        if (playerManager.LivesRemaining < 0) {
                            gameState = GameStates.GameOver;
                        } else {
                            gameState = GameStates.PlayerDead;
                        }

                    } else {
                        enemyManager.Active = true;
                    }
                    break;

                case GameStates.PlayerDead:
                    _playerDeathTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    starField.Update(gameTime);
                    asteroidManager.Update(gameTime);
                    enemyManager.Update(gameTime);
                    playerManager.Update(gameTime);
                    playerManager.PlayerShotManager.Update(gameTime);
                    explosionManager.Update(gameTime);
                    if (_playerDeathTimer > _playerDeathDelayTime) {
                        ResetGame();
                        gameState = GameStates.Playing;
                    }
                    break;

                case GameStates.GameOver:
                    _playerDeathTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    starField.Update(gameTime);
                    asteroidManager.Update(gameTime);
                    enemyManager.Update(gameTime);
                    playerManager.PlayerShotManager.Update(gameTime);
                    explosionManager.Update(gameTime);
                    if (_playerDeathTimer >= _playerDeathDelayTime)
                        gameState = GameStates.TitleScreen;
                    break;
            }

            GamepadVibration.Update(gameTime);

            // call the base update - should be at the end
            base.Update(gameTime);
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Purple);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            if (gameState == GameStates.TitleScreen) {
                spriteBatch.Draw(titleScreen, new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White);
            }

            if ((gameState == GameStates.Playing) || (gameState == GameStates.PlayerDead) || (gameState == GameStates.GameOver)) {
                // this draws the background
                spriteBatch.Draw(gameBackground, new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White);

                starField.Draw(spriteBatch);
                asteroidManager.Draw(spriteBatch);
                playerManager.Draw(spriteBatch);
                enemyManager.Draw(spriteBatch);
                explosionManager.Draw(spriteBatch);

                // draw the score and the lives remaining
                spriteBatch.DrawString(pericles14, "Score: " + playerManager.PlayerScore.ToString(), _scoreLocation, Color.White);
                if (playerManager.LivesRemaining >= 0)
                    spriteBatch.DrawString(pericles14, "Lives: " + playerManager.LivesRemaining.ToString(), _livesLocation, Color.White);
            }

            if (gameState == GameStates.GameOver) {
                string textToDisplay = "G A M E   O V E R";
                spriteBatch.DrawString(pericles14, textToDisplay, new Vector2(this.Window.ClientBounds.Width / 2 - pericles14.MeasureString(textToDisplay).X / 2, 50), Color.White);
            }

            spriteBatch.End();

            // this should be at the end of the logic
            base.Draw(gameTime);
        }


        private void ResetGame() {
            playerManager.PlayerSprite.Location = _playerStartLocation;
            foreach (Sprite asteroid in asteroidManager.Asteroids) {
                asteroid.Location = new Vector2(-500, 500);
            }
            enemyManager.Enemies.Clear();
            enemyManager.Active = false;
            playerManager.PlayerShotManager.Shots.Clear();
            enemyManager.EnemyShotManager.Shots.Clear();
            playerManager.Destroyed = false;
        } // end resetGame()
    }
}
