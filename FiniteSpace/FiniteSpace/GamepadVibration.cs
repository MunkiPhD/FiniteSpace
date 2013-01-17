using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FiniteSpace {
    public static class GamepadVibration {
        private static float _secondsToVibrate;

        /// <summary>
        /// Vibrate the controller for the specified player
        /// </summary>
        /// <param name="player">The player to vibrate the controller for</param>
        /// <param name="leftMotor">The force of the left motor, 0 to 1f</param>
        /// <param name="rightMotor">The force of the right motor, 0 to 1f</param>
        /// <param name="secondsToVibrate">How long to vibrate the controller for</param>
        public static void VibrateController(PlayerIndex player, float leftMotor, float rightMotor, float secondsToVibrate) {
            _secondsToVibrate = secondsToVibrate;
            GamePad.SetVibration(player, leftMotor, rightMotor);
        }


        /// <summary>
        /// Updates and determines whether to stop vibrating
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Update(GameTime gameTime) {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_secondsToVibrate > 0)
                _secondsToVibrate -= elapsed;

            if (_secondsToVibrate <= 0) {
                GamePad.SetVibration(PlayerIndex.One, 0, 0);
                _secondsToVibrate = 0.0f;
            }
        }
    } // end class
} // end namespace
