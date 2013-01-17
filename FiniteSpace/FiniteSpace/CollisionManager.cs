using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FiniteSpace {
    class CollisionManager {
        private AsteroidManager _asteroidManager;
        private PlayerManager _playerManager;
        private EnemyManager _enemyManager;
        private ExplosionManager _explosionManager;
        private Vector2 _offScreen = new Vector2(-500, -500);
        private Vector2 _shotToAsteroidImpact = new Vector2(0, -20);
        private int _enemyPointValue = 100;

        public CollisionManager(AsteroidManager asteroidManager, PlayerManager playerManager, EnemyManager enemyManager, ExplosionManager explosionManager) {
            _asteroidManager = asteroidManager;
            _playerManager = playerManager;
            _enemyManager = enemyManager;
            _explosionManager = explosionManager;
        }



        /// <summary>
        /// Checks all the collisions that could occur in the game
        /// </summary>
        public void CheckCollisions() {
            CheckShotToEnemyCollisions();
            CheckShotToAsteroidCollisions();

            // if the player is still alive, we can run checks against him
            if (!_playerManager.Destroyed) {
                CheckShotToPlayerCollisions();
                CheckEnemyToPlayerCollisions();
                CheckAsteroidToPlayerCollisions();
            }
        }



        /// <summary>
        /// Checks whether shots to an enemy collided
        /// </summary>
        private void CheckShotToEnemyCollisions() {
            foreach (Sprite shot in _playerManager.PlayerShotManager.Shots) {
                foreach (Enemy enemy in _enemyManager.Enemies) {
                    if (shot.IsCircleColliding(enemy.EnemySprite.Center, enemy.EnemySprite.collisionRadius)) {
                        shot.Location = _offScreen;
                        enemy.Destroyed = true;
                        _playerManager.PlayerScore += _enemyPointValue;
                        _explosionManager.AddExplosion(enemy.EnemySprite.Center, enemy.EnemySprite.Velocity / 10);
                    }
                }
            }
        } // end CheckShotToEnemy



        /// <summary>
        /// Checks whether a player shot hit an asteroid
        /// </summary>
        private void CheckShotToAsteroidCollisions() {
            foreach (Sprite shot in _playerManager.PlayerShotManager.Shots) {
                foreach (Sprite asteroid in _asteroidManager.Asteroids) {
                    if(shot.IsCircleColliding(asteroid.Center, asteroid.collisionRadius)){
                        shot.Location = _offScreen;
                        asteroid.Velocity += _shotToAsteroidImpact;
                    }
                }
            }
        }



        /// <summary>
        /// Checks shots fired at the player
        /// </summary>
        private void CheckShotToPlayerCollisions() {
            foreach (Sprite shot in _enemyManager.EnemyShotManager.Shots) {
                if (shot.IsCircleColliding(_playerManager.PlayerSprite.Center, _playerManager.PlayerSprite.collisionRadius)) {
                    shot.Location = _offScreen;
                    _playerManager.Destroyed = true;
                    _explosionManager.AddExplosion(_playerManager.PlayerSprite.Center, Vector2.Zero);
                    GamepadVibration.VibrateController(PlayerIndex.One, 0.5f, 0.5f, 1);
                }
            }
        }



        /// <summary>
        /// Checks if an enemy has collided with a player
        /// </summary>
        private void CheckEnemyToPlayerCollisions() {
            foreach (Enemy enemy in _enemyManager.Enemies) {
                if (enemy.EnemySprite.IsCircleColliding(_playerManager.PlayerSprite.Center, _playerManager.PlayerSprite.collisionRadius)) {
                    enemy.Destroyed = true;
                    _explosionManager.AddExplosion(enemy.EnemySprite.Center, enemy.EnemySprite.Velocity / 10);
                    _playerManager.Destroyed = true;
                    _explosionManager.AddExplosion(_playerManager.PlayerSprite.Center, Vector2.Zero);
                    GamepadVibration.VibrateController(PlayerIndex.One, 0.5f, 0.5f, 1);
                }
            }
        }



        /// <summary>
        /// Checks if a player has hit the rock of a thousand suns
        /// </summary>
        private void CheckAsteroidToPlayerCollisions() {
            foreach (Sprite asteroid in _asteroidManager.Asteroids) {
                if (asteroid.IsCircleColliding(_playerManager.PlayerSprite.Center, _playerManager.PlayerSprite.collisionRadius)) {
                    _playerManager.Destroyed = true;
                    _explosionManager.AddExplosion(_playerManager.PlayerSprite.Center, Vector2.Zero);
                    _explosionManager.AddExplosion(asteroid.Center, asteroid.Velocity / 10);
                    asteroid.Location = _offScreen;
                    GamepadVibration.VibrateController(PlayerIndex.One, 1, 1, 1);
                }
            }
        }


    } // end CollisionManager
} // end namespace
