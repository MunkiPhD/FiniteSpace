using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace FiniteSpace {
    class EnemyManager {
        private Texture2D _texture;
        private Rectangle _initialFrame;
        private int _frameCount;
        
        private PlayerManager _playerManager;
        public List<Enemy> Enemies = new List<Enemy>();
        public ShotManager EnemyShotManager;

        public int MinShipsPerWave = 5;
        public int MaxShipsPerWave = 8;
        private float _nextWaveTimer = 0.0f;
        private float _nextWaveMinTimer = 8.0f;
        private float _shipSpawnTimer = 0.0f;
        private float _shipSpawnWaitTime = 0.5f;
        private float _shipShotChance = 0.2f;
        private List<List<Vector2>> _pathWaypoints = new List<List<Vector2>>();
        private Dictionary<int, int> _waveSpawns = new Dictionary<int,int>();
        public bool Active = true;
        private Random rand = new Random();


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="texture">The spritesheet</param>
        /// <param name="initialFrame">Initial frame for the enemies</param>
        /// <param name="frameCount">Frame count for enemy animation</param>
        /// <param name="playerManager">Copy of the playerManager instance</param>
        /// <param name="screenBounds">The bounds of the screen</param>
        public EnemyManager(Texture2D texture, Rectangle initialFrame, int frameCount, PlayerManager playerManager, Rectangle screenBounds) {
            _texture = texture;
            _initialFrame = initialFrame;
            _frameCount = frameCount;
            _playerManager = playerManager;
            EnemyShotManager = new ShotManager(texture, new Rectangle(0, 300, 5, 5), 4, 2, 150f, screenBounds);
            
            SetupWaypoints();
        }



        /// <summary>
        /// Update!
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime) {
            EnemyShotManager.Update(gameTime);
            for(int x = Enemies.Count - 1; x >= 0; x--) {
                Enemies[x].Update(gameTime);
                if(Enemies[x].IsActive() == false) {
                    Enemies.RemoveAt(x);
                }else{
                    if((float)rand.Next(0, 1000) / 10 <= _shipShotChance) {
                        Vector2 fireLoc = Enemies[x].EnemySprite.Location;
                        fireLoc += Enemies[x].GunOffset;
                        Vector2 shotDirection = _playerManager.PlayerSprite.Center - fireLoc;
                        shotDirection.Normalize();
                        EnemyShotManager.FireShot(fireLoc, shotDirection, false);
                    }
                }
            }

            if(Active)
                UpdateWaveSpawns(gameTime);
        }



        /// <summary>
        /// Draw!
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            EnemyShotManager.Draw(spriteBatch);
            foreach(Enemy enemy in Enemies) {
                enemy.Draw(spriteBatch);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="waveType"></param>
        public void SpawnWave(int waveType) {
            _waveSpawns[waveType] += rand.Next(MinShipsPerWave, MaxShipsPerWave + 1);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateWaveSpawns(GameTime gameTime) {
            _shipSpawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(_shipSpawnTimer > _shipSpawnWaitTime) {
                for(int x = _waveSpawns.Count - 1; x >= 0; x--) {
                    if(_waveSpawns[x] > 0) {
                        _waveSpawns[x]--;
                        SpawnEnemy(x);
                    }
                }
                _shipSpawnTimer = 0f;
            }

            _nextWaveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(_nextWaveTimer > _nextWaveMinTimer){
                SpawnWave(rand.Next(0, _pathWaypoints.Count));
                _nextWaveTimer = 0f;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public void SpawnEnemy(int path) {
            Enemy thisEnemy = new Enemy(_texture, _pathWaypoints[path][0], _initialFrame, _frameCount);

            for(int x = 0; x < _pathWaypoints[path].Count(); x++) {
                thisEnemy.AddWaypoint(_pathWaypoints[path][x]);
            }

            Enemies.Add(thisEnemy);
        }



        /// <summary>
        /// Sets up waypoint lists
        /// </summary>
        private void SetupWaypoints() {
            List<Vector2> path0 = new List<Vector2>();
            path0.Add(new Vector2(850, 300));
            path0.Add(new Vector2(-100, 300));
            _pathWaypoints.Add(path0);
            _waveSpawns[0] = 2;

            List<Vector2> path1 = new List<Vector2>();
            path1.Add(new Vector2(-50, 225));
            path1.Add(new Vector2(850, 225));
            _pathWaypoints.Add(path1);
            _waveSpawns[1] = 3;

            List<Vector2> path2 = new List<Vector2>();
            path2.Add(new Vector2(-100, 50));
            path2.Add(new Vector2(150, 50));
            path2.Add(new Vector2(200, 75));
            path2.Add(new Vector2(200, 125));
            path2.Add(new Vector2(150,150));
            path2.Add(new Vector2(150,175));
            path2.Add(new Vector2(200,200));
            path2.Add(new Vector2(600,200));
            path2.Add(new Vector2(850,600));
            _pathWaypoints.Add(path2);
            _waveSpawns[2] = 5;


            List<Vector2> path3 = new List<Vector2>();
            path3.Add(new Vector2(600, -10));
            path3.Add(new Vector2(650, 250));
            path3.Add(new Vector2(580, 275));
            path3.Add(new Vector2(500, 250));
            path3.Add(new Vector2(500, 200));
            path3.Add(new Vector2(450, 175));
            path3.Add(new Vector2(400, 150));
            path3.Add(new Vector2(-100, 150));
            _pathWaypoints.Add(path3);
            _waveSpawns[3] = 4;
        } // end setupWaypoints
    }
}
