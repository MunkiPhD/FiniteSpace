using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace FiniteSpace {
    public static class SoundManager {
        private static List<SoundEffect> _explosions = new List<SoundEffect>();
        private static int _explosionCount = 4;
        private static SoundEffect _playerShot;
        private static SoundEffect _enemyShot;
        private static Random rand = new Random();



        /// <summary>
        /// Initialzies the sound manager class by loading all the sounds
        /// </summary>
        /// <param name="content"></param>
        public static void Initialize(ContentManager content) {
            try {
                _playerShot = content.Load<SoundEffect>(@"Sounds\Shot1");
                _enemyShot = content.Load<SoundEffect>(@"Sounds\Shot2");

                for (int x = 1; x <= _explosionCount; x++) {
                    _explosions.Add(content.Load<SoundEffect>(@"Sounds\Explosion" + x.ToString()));
                }
            } catch (Exception e) {
                Debug.Write("SoundManager initialization failed: " + e.Message);
            }
        }



        /// <summary>
        /// Plays a random explosion sound
        /// </summary>
        public static void PlayExplosion() {
            try {
           //     _explosions[rand.Next(0, _explosionCount)].Play();
            } catch (Exception e) {
                Debug.Write("PlayExplosion() Failed: " + e.Message);
            }
        }


        /// <summary>
        /// Plays the player shot sound effect
        /// </summary>
        public static void PlayPlayerShot() {
            try {
            //    _playerShot.Play();
            } catch (Exception e) {
                Debug.Write("PlayPlayerShot() Failed: " + e.Message);
            }
        }



        /// <summary>
        /// Plays the player shot sound effect
        /// </summary>
        public static void PlayEnemyShot() {
            try {
            //    _enemyShot.Play();
            } catch (Exception e) {
                Debug.Write("PlayEnemyShot() Failed: " + e.Message);
            }
        }
    } // end class
} // end namespace
