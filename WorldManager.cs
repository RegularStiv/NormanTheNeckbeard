using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace NormanTheNeckbeard
{
    class WorldManager
    {
        // Fields

        private List<Platform> platforms;
        private List<Enemy> enemies;
        private double totalTime;
        private Player norman;
        private CollisionManager colMan;
        private List<Collectables> collectables;
        private Collectables finalPoint;
        private List<WorldTile> background;



        // Properties

        public List<Platform> PlatformList { get { return platforms; } }
        public List<Enemy> Enemies { get { return enemies; } }
        public List<Collectables> Collectables { get { return collectables; } }
        public Collectables FinalPoint { get { return finalPoint; } set { finalPoint = value; } }
        public List<WorldTile> Background { get { return background; } }

        // Constructor

        public WorldManager(Texture2D enemyTexture, Texture2D platformTexture, Player norman)
        {
            this.norman = norman;
            platforms = new List<Platform>();
            enemies = new List<Enemy>();
            collectables = new List<Collectables>();
            background = new List<WorldTile>();
            totalTime = 0;
            colMan = new CollisionManager(norman, this);
        }



        // Methods

        /// <summary>
        /// Updates each enemy in the WorldManager.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime, KeyboardState kbState, KeyboardState prevKbState)
        {
            totalTime += gameTime.ElapsedGameTime.TotalSeconds;

            // Update norman
            norman.Update(gameTime, kbState, prevKbState);
            // Update the enemies
            foreach (Enemy enemy in enemies)
                enemy.Update(gameTime);
            //removes enemies if they die
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].HP <= 0)
                {
                    enemies.Remove(enemies[i]);
                }
            }

            // Update the collision manager
            colMan.Update();

            // Update the enemies
            foreach (Enemy enemy in enemies)
            {
                // Move everything according to norman
                enemy.X -= (int)norman.Movement.X;
                enemy.Y -= (int)norman.Movement.Y;
            }

            // Update the platforms
            foreach (Platform plat in platforms)
            {
                // Move everything according to norman
                plat.X -= norman.Movement.X;
                plat.Y -= norman.Movement.Y;
            }

            // Update Collectibles
            foreach (Collectables collectables in collectables)
            {
                // Move everything according to norman
                collectables.X -= (int)norman.Movement.X;
                collectables.Y -= (int)norman.Movement.Y;
            }

            // Update worldTiles
            foreach (WorldTile worldTile in background)
            {
                // Move everything according to norman
                worldTile.X -= (int)norman.Movement.X;
                worldTile.Y -= (int)norman.Movement.Y;
            }

            // Update finalPoint
            // Move everything according to norman
            finalPoint.X -= (int)norman.Movement.X;
            finalPoint.Y -= (int)norman.Movement.Y;

        }

        // Draws everything inside the WorldManager.
        public void Draw(SpriteBatch sb)
        {
            foreach (WorldTile worldTile in background)
            {
                worldTile.Draw(sb);
            }
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(sb);
            }
            foreach (Collectables collectables in collectables)
            {
                collectables.Draw(sb);
            }
            finalPoint.Draw(sb);
            for (int i = 0; i < platforms.Count; i++)
            {
                platforms[i].Draw(sb, 4);
            }
            
        }

    }
}
