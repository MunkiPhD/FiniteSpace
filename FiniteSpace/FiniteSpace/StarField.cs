using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FiniteSpace {
    class StarField {
        private List<Sprite> _stars = new List<Sprite>();
        private int _screenWidth = 800;
        private int _screenHeight = 600;
        private Random rand = new Random();
        private Color[] colors = { Color.White, Color.Yellow, Color.Wheat, Color.WhiteSmoke, Color.SlateGray };

        public StarField(int screenWidth, int screenHeight, int starCount, Vector2 starVelocity, Texture2D texture, Rectangle frameRectangle) {
            this._screenHeight = screenHeight;
            this._screenWidth = screenWidth;

            // by having the speed be random, we can create a parallaxing effect
            int min = (int)starVelocity.Y / 2;
            int max = (int)starVelocity.Y * 2;

            for(int x = 0; x < starCount; x++) {
                _stars.Add(new Sprite(new Vector2(rand.Next(0, screenWidth), rand.Next(0, screenHeight)), texture, frameRectangle, new Vector2(0, rand.Next(min, max))));
                Color starColor = colors[rand.Next(0, colors.Count())];
                starColor *= (float)(rand.Next(30, 80) / 100f);
                _stars[_stars.Count() - 1].TintColor = starColor;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTIme"></param>
        public void Update(GameTime gameTIme) {
            foreach(Sprite star in _stars) {
                star.Update(gameTIme);
                if(star.Location.Y > _screenHeight) {
                    star.Location = new Vector2(rand.Next(0, _screenWidth), 0);
                }
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            foreach(Sprite star in _stars) {
                star.Draw(spriteBatch);
            }
        }

    }
}
