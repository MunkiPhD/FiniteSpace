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
        private void CheckShotToAsteroid() {
            foreach (Sprite shot in _playerManager.PlayerShotManager.Shots) {
                foreach (Sprite asteroid in _asteroidManager.Asteroids) {
                    if(shot.IsCircleColliding(asteroid.Center, asteroid.collisionRadius)){
                        shot.Location = _offScreen;
                        asteroid.Velocity += _shotToAsteroidImpact;
                    }
                }
            }
        }

    } // end CollisionManager
} // end namespace
