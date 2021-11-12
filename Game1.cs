using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace NormanTheNeckbeard
{
    // Enum for the state of the game
    public enum GameState
    {
        StartMenu,
        Gameplay,
        Inventory,
        PauseMenu,
        GameOver,
        Win,
        Finish
    }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        // Universal tile size

        public const int tileSize = 64;

        // Drawing Fields

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont arial20;
        private Texture2D normanTexture;
        private Texture2D enemyTexture;
        private Texture2D platformTexture;
        private Texture2D katanaTexture;
        private Texture2D pillowChanTexture;
        private Texture2D collectableText;
        private Texture2D backgroundTexture;
        private Texture2D cursorTexture;
        private Texture2D startButtonTexture;
        private Texture2D menuButtonTexture;
        private Texture2D resumeButtonTexture;
        private Texture2D tryAgainButtonTexture;
        private Texture2D titleTexture;
        private Texture2D nextTexture;
        private Texture2D congratsTexture;

        private float gameScale = .2f;
        private int numRowTiles;
        private int numColTiles;
        private char[,] worldReader;
        private int levelIndex;

        // Logic Fields

        private Player norman;
        private WorldManager worldManager;
        private InventoryManager inventory;
        private KeyboardState kbState;
        private KeyboardState prevKbState;
        private MouseState mouse;
        private MouseState prevMouse;
        private GameState gameState;
        //private Rectangle attackRect;
        private Random actOfGod;
        private List<string> titleList;
        private List<string> levels;
        private Collectables finalPoint;
        private Button startButton;
        private Button menuButton;
        private Button resumeButton;
        private Button tryAgainButton;
        private Button nextButton;


        // Property

        public float GameScale { get { return gameScale; } }



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            actOfGod = new Random();
            titleList = new List<string>()
            {
                "Do it for Pillow-Chan!",
                "Hungry for tendies?",
                "Omae wa, mou shindeiru.",
                "While you were playing video games, Norman was studying the blade.",
                "Pokimane please we need you.",
                "No amount of axe can mask the musk.",
                "Your parents love you, you know.",
                "Welcome to Simp-City",
                "[Inaudible weeaboo screeching]"
            };
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Game should start at the start menu state
            gameState = GameState.StartMenu;

            Window.Title = titleList[actOfGod.Next(0, titleList.Count)];

            graphics.PreferredBackBufferWidth = (int)(((640 * gameScale) / 2) * 15);
            graphics.PreferredBackBufferHeight = (int)(((640 * gameScale) / 2) * 10);
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the textures to be used
            normanTexture = Content.Load<Texture2D>("Norman Spritesheet");
            enemyTexture = Content.Load<Texture2D>("Mushroom");
            platformTexture = Content.Load<Texture2D>("Brick Wall");
            katanaTexture = Content.Load<Texture2D>("Basic Katana");
            collectableText = Content.Load<Texture2D>("Essence_of_the_Goddess");
            arial20 = Content.Load<SpriteFont>("arial20");
            backgroundTexture = Content.Load<Texture2D>("Background_Brick");
            pillowChanTexture = Content.Load<Texture2D>("Pillow-Chan");
            cursorTexture = Content.Load<Texture2D>("Mouse Cursor");
            startButtonTexture = Content.Load<Texture2D>("Begin_Button");
            titleTexture = Content.Load<Texture2D>("Title");
            menuButtonTexture = Content.Load<Texture2D>("Menu");
            tryAgainButtonTexture = Content.Load<Texture2D>("Try_again");
            resumeButtonTexture = Content.Load<Texture2D>("Return");
            nextTexture = Content.Load<Texture2D>("Next_Level");
            congratsTexture = Content.Load<Texture2D>("Congrats");

            // Initialize the Player class
            Rectangle position = new Rectangle(tileSize, tileSize, tileSize, tileSize);
            int hitBoxWidth = (int)(tileSize * .828125);
            int hitBoxHeight = (int)(tileSize * .96875);
            norman = new Player(
                position,
                new Rectangle(position.X + ((position.Width - hitBoxWidth) / 2), position.Y + (position.Height - hitBoxHeight), hitBoxWidth, hitBoxHeight),
                normanTexture,
                inventory);

            // Initialize each manager class
            worldManager = new WorldManager(enemyTexture, Content.Load<Texture2D>("Brick Wall"), norman);
            inventory = new InventoryManager();

            // Make the buttons
            startButton = new Button(startButtonTexture, new Rectangle(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width / 8,
                    (GraphicsDevice.Viewport.Height / 2) + 75, startButtonTexture.Width / 4, startButtonTexture.Height / 4), .25f);

            menuButton = new Button(menuButtonTexture, new Rectangle(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width / 8,
                    (GraphicsDevice.Viewport.Height / 2) + 50, startButtonTexture.Width / 4, startButtonTexture.Height / 4), .25f);

            resumeButton = new Button(resumeButtonTexture, new Rectangle(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width / 8,
                    (GraphicsDevice.Viewport.Height / 2) - 200, startButtonTexture.Width / 4, startButtonTexture.Height / 4), .25f);

            tryAgainButton = new Button(tryAgainButtonTexture, new Rectangle(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width / 8,
                    (GraphicsDevice.Viewport.Height / 2) - 200, startButtonTexture.Width / 4, startButtonTexture.Height / 4), .25f);

            nextButton = new Button(nextTexture, new Rectangle(GraphicsDevice.Viewport.Width / 2 - startButtonTexture.Width / 8,
                    (GraphicsDevice.Viewport.Height / 2) - 200, startButtonTexture.Width / 4, startButtonTexture.Height / 4), .25f);

            // Load in the level
            FileStream inStream = null;
            StreamReader input = null;

            levels = new List<string>();
            levels.Add("Level1.level");
            levels.Add("Level2.level");
            levels.Add("Level3.level");

            try
            {
                if (levelIndex <= levels.Count)
                {
                    inStream = File.OpenRead(levels[levelIndex]);
                    input = new StreamReader(inStream);
                }
                else
                {
                    levelIndex = 0;
                    inStream = File.OpenRead(levels[levelIndex]);
                    input = new StreamReader(inStream);
                }
                int.TryParse(input.ReadLine(), out numRowTiles);
                int.TryParse(input.ReadLine(), out numColTiles);
                input.ReadLine();

                worldReader = new char[numRowTiles, numColTiles];

                string line;
                for (int i = 0; i < numColTiles; i++)
                {
                    for (int j = 0; j < numRowTiles; j++)
                    {
                        line = input.ReadLine();
                        
                        worldReader[j, i] = Char.Parse(line);
                    }
                }
            }
            catch(Exception e)
            {
                // Ask Chris about messageboxes in monogame:
                //DialogResult result = MessageBox.Show("File Loaded Successfully",
                //"File Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Console.WriteLine("Error reading file.  " + e.Message);
            }
            finally
            {
                if (input != null)
                {
                    input.Close();
                }
            }

            //sorts out the various components in the game
            for(int i = 0; i < numRowTiles; i++)
            {
                for(int j = 0; j < numColTiles; j++)
                {
                    switch (worldReader[i, j])
                    {
                        case 'P':
                            {
                                norman.X = graphics.PreferredBackBufferWidth / 2 - tileSize / 2;
                                norman.Y = graphics.PreferredBackBufferHeight /2 - tileSize / 2;
                                break;
                            }
                        case 'E':
                            {
                                Rectangle e = new Rectangle(i * tileSize, j * tileSize, tileSize, tileSize);
                                int eHitBoxWidth = (int)(e.Width * .8125);
                                int eHitBoxHeight = (int)(e.Width * .875);
                                worldManager.Enemies.Add(new Enemy(e, new Rectangle(e.X + ((e.Width - eHitBoxWidth) / 2), e.Y + (e.Height - eHitBoxHeight), eHitBoxWidth, eHitBoxHeight),
                                    enemyTexture, norman));
                                break;
                            }
                        case '=':
                            {
                                worldManager.PlatformList.Add(new Platform(platformTexture, new Rectangle(i * tileSize, j * tileSize, tileSize, tileSize)));
                                break;
                            }
                        case '-':
                            {
                                worldManager.PlatformList.Add(new Platform(platformTexture, new Rectangle(i * tileSize, j * tileSize, tileSize, tileSize)));
                                break;
                            }
                        case 'X':
                            {
                                finalPoint = new Collectables(new Rectangle(i * tileSize, j * tileSize, tileSize, tileSize), new Rectangle(i * tileSize, j * tileSize, tileSize, tileSize), pillowChanTexture);
                                worldManager.FinalPoint = finalPoint;
                                break;
                            }
                        case 'C':
                            {
                                worldManager.Collectables.Add(new Collectables (new Rectangle(i * tileSize, j * tileSize, tileSize, tileSize), new Rectangle(i * tileSize, j * tileSize, tileSize, tileSize), collectableText));
                                break;
                            }
                    }
                    worldManager.Background.Add(new WorldTile(backgroundTexture, new Rectangle(i * tileSize, j * tileSize, tileSize, tileSize)));
                }
            }
            
            //makes the tiles in the background
           
            

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // set prevKbstate to equal kbState before it updates, making it represent the frame before
            // At frame 1 prevKbState is null
            prevKbState = kbState;
            // Update the keyboard state variable to the current keyboard state
            kbState = Keyboard.GetState();

            prevMouse = mouse;
            mouse = Mouse.GetState();
            Rectangle mouseRect = new Rectangle(mouse.X, mouse.Y, cursorTexture.Width, cursorTexture.Height);


            // checks to see if the player died

            // Finite state machine for the game states
            // I used else ifs just to ensure only one gameState change occurs
            // The if statements don't need to be singlePress bools because the gameState changes in a frame anyway
            switch (gameState)
            {
                case GameState.StartMenu:

                    // Switch to gameplay state if the start button is pressed
                    // Check to see if the mouse is colliding with the start button
                    if(mouseRect.Intersects(startButton.HitBox) == true)
                    {
                        // If this is the first time the mouse has been pressed
                        if(mouse.LeftButton == ButtonState.Pressed && mouse != prevMouse)
                        {
                            gameState = GameState.Gameplay;
                        }
                    }

                    break;


                case GameState.Gameplay:

                    // Update worldManager
                    worldManager.Update(gameTime, kbState, prevKbState);

                    // Switch to gameOver state if the player has no health
                    if (norman.HP <= 0)
                    {
                        gameState = GameState.GameOver;
                    }

                    // Switch to pauseMenu state if the P key is pressed
                    else if (SinglePress(Keys.P))
                        gameState = GameState.PauseMenu;

                    // Switch to inventory state if the I key is pressed
                    else if (SinglePress(Keys.I))
                        gameState = GameState.Inventory;

                    // Check if attacking
                    if (norman.Attack)
                    {
                        // Checks intersections and deals damage accordingly
                        for (int i = 0; i < worldManager.Enemies.Count; i++)
                        {
                            if (norman.AttackRect.Intersects(worldManager.Enemies[i].HitBox) && worldManager.Enemies[i].gotHit == false)
                            {
                                worldManager.Enemies[i].gotHit = true;
                                worldManager.Enemies[i].TakeDamage(20);
                                if (norman.Direction == Facing.Right)
                                    worldManager.Enemies[i].VelocityX += 700;
                                else
                                    worldManager.Enemies[i].VelocityX -= 700;
                            }

                        }
                    }

                    for (int i = 0; i < worldManager.Enemies.Count; i++)
                    {
                        if (worldManager.Enemies[i].HitBox.Intersects(norman.HitBox))
                        {
                            norman.TakeDamage(15);
                        }
                    }
                    for (int i = 0; i < worldManager.Collectables.Count; i++ )
                    {
                        if (norman.HitBox.Intersects(worldManager.Collectables[i].HitBox))
                        {
                            norman.HP += 15;
                            worldManager.Collectables.Remove(worldManager.Collectables[i]);
                        }
                    }

                    if (norman.HitBox.Intersects(finalPoint.HitBox))
                    {
                        if(levelIndex == 2)
                        {
                            gameState = GameState.Finish;
                        }
                        else
                        {
                            gameState = GameState.Win;
                        }
                    }
                    
                    break;

               case GameState.Inventory:
                    // Switch to gameplay state if the Esc or I key is pressed
                    if (SinglePress(Keys.I))
                        gameState = GameState.Gameplay;
                    // Switch to pauseMenu state if the P key is pressed
                    else if (SinglePress(Keys.P))
                        gameState = GameState.PauseMenu;
                    break;

                case GameState.PauseMenu:

                    // Switch to gameplay state if the start button is presse
                    // Check to see if the mouse is colliding with the menu button
                    if (mouseRect.Intersects(menuButton.HitBox) == true)
                    {
                        // If this is the first time the mouse has been pressed
                        if (mouse.LeftButton == ButtonState.Pressed && mouse != prevMouse)
                        {
                            gameState = GameState.StartMenu;
                            norman.Respawn();
                            LoadContent();
                        }
                    }

                    // Check to see if the mouse is colliding with the resume button
                    if (mouseRect.Intersects(resumeButton.HitBox) == true)
                    {
                        // If this is the first time the mouse has been pressed
                        if (mouse.LeftButton == ButtonState.Pressed && mouse != prevMouse)
                        {
                            gameState = GameState.Gameplay;
                            
                        }
                    }

                    if (SinglePress(Keys.I))
                        gameState = GameState.Inventory;

                    break;

                case GameState.GameOver:

                    // Check to see if the mouse is colliding with the menu button
                    if (mouseRect.Intersects(menuButton.HitBox) == true)
                    {
                        // If this is the first time the mouse has been pressed
                        if (mouse.LeftButton == ButtonState.Pressed && mouse != prevMouse)
                        {
                            gameState = GameState.StartMenu;
                            norman.Respawn();
                            LoadContent();
                        }
                    }

                    // Check to see if the mouse is colliding with the try again button
                    if (mouseRect.Intersects(tryAgainButton.HitBox) == true)
                    {
                        // If this is the first time the mouse has been pressed
                        if (mouse.LeftButton == ButtonState.Pressed && mouse != prevMouse)
                        {
                            norman.Respawn();
                            gameState = GameState.Gameplay;
                            LoadContent();
                        }
                    }
                    break;

                case GameState.Win:

                    // Check to see if the mouse is colliding with the menu button
                    if (mouseRect.Intersects(menuButton.HitBox) == true)
                    {
                        // If this is the first time the mouse has been pressed
                        if (mouse.LeftButton == ButtonState.Pressed && mouse != prevMouse)
                        {
                            norman.Respawn();
                            LoadContent();
                            gameState = GameState.StartMenu;
                        }
                    }

                    if (mouseRect.Intersects(nextButton.HitBox) == true)
                    {
                        // If this is the first time the mouse has been pressed
                        if (mouse.LeftButton == ButtonState.Pressed && mouse != prevMouse)
                        {
                            int currenthp = norman.HP;
                            levelIndex++;
                            gameState = GameState.Gameplay;
                            LoadContent();
                            norman.HP = currenthp;
                        }
                    }

                    break;

                case GameState.Finish:
                    {
                        // Check to see if the mouse is colliding with the menu button
                        if (mouseRect.Intersects(menuButton.HitBox) == true)
                        {
                            // If this is the first time the mouse has been pressed
                            if (mouse.LeftButton == ButtonState.Pressed && mouse != prevMouse)
                            {
                                gameState = GameState.StartMenu;
                                norman.Respawn();
                                LoadContent();
                            }
                        }

                        break;
                    }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // draws for the gameplay state
            if (gameState == GameState.Gameplay)
            {
                spriteBatch.Begin();
                // Draw Player and manager classes
                
                worldManager.Draw(spriteBatch);
                norman.Draw(spriteBatch);
                spriteBatch.DrawString(arial20, "HP: " + norman.HP, new Vector2(), Color.Red) ;

                // End the spritebatch
                spriteBatch.End();
            }
            if (gameState == GameState.Win)
            {
                spriteBatch.Begin();

                nextButton.Draw(spriteBatch);

                menuButton.Draw(spriteBatch);

                // Draw the Mouse:
                // Record the coordinates of the mouse
                Vector2 mousePosition = new Vector2(mouse.X, mouse.Y);
                spriteBatch.Draw(cursorTexture, mousePosition, Color.White);

                spriteBatch.End();
            }
            //draws for game over state
            if (gameState == GameState.GameOver)
            {
                spriteBatch.Begin();

                tryAgainButton.Draw(spriteBatch);
                menuButton.Draw(spriteBatch);

                // Draw the Mouse:
                // Record the coordinates of the mouse
                Vector2 mousePosition = new Vector2(mouse.X, mouse.Y);
                spriteBatch.Draw(cursorTexture, mousePosition, Color.White);

                spriteBatch.End();
            }
            //draws for the menu
            if (gameState == GameState.StartMenu)
            {
                spriteBatch.Begin();

                // Draw the Start Button
                startButton.Draw(spriteBatch);

                // Draw the Title
                spriteBatch.Draw(
                            titleTexture,                                                                   // Texture
                            new Vector2((GraphicsDevice.Viewport.Width / 2) - (titleTexture.Width / 4), 
                            (GraphicsDevice.Viewport.Height / 2) - (titleTexture.Height / 2)),              // Coordinates
                            new Rectangle(
                                0,
                                0,                                                                          // Portion
                                titleTexture.Width,
                                titleTexture.Height
                                ),
                            Color.White,                                                                    // Color
                            0.0f,                                                                           // Rotation
                            Vector2.Zero,                                                                   // Origin
                            new Vector2(.5f, .5f),                                                          // Scale
                            SpriteEffects.None,                                                             // Effects
                            0                                                                               // Layer depth
                            );

                // Draw the Mouse:
                // Record the coordinates of the mouse
                Vector2 mousePosition = new Vector2(mouse.X, mouse.Y);
                spriteBatch.Draw(cursorTexture, mousePosition, Color.White);

                spriteBatch.End();
            }
            if (gameState == GameState.PauseMenu)
            {
                spriteBatch.Begin();


                menuButton.Draw(spriteBatch);
                resumeButton.Draw(spriteBatch);

                // Draw the Mouse:
                // Record the coordinates of the mouse
                Vector2 mousePosition = new Vector2(mouse.X, mouse.Y);
                spriteBatch.Draw(cursorTexture, mousePosition, Color.White);

                spriteBatch.End();
            }
            if (gameState == GameState.Inventory)
            {
                spriteBatch.Begin();

                // Draw the Mouse:
                // Record the coordinates of the mouse
                Vector2 mousePosition = new Vector2(mouse.X, mouse.Y);
                spriteBatch.Draw(cursorTexture, mousePosition, Color.White);


                spriteBatch.DrawString(arial20, "This is the inventory not done Leave me alone.",
                    new Vector2(GraphicsDevice.Viewport.Width / 2 - arial20.MeasureString("This is the inventory not done Leave me alone. ").X / 2, 0),
                    Color.White);
                spriteBatch.DrawString(arial20, "Press 'P' to go to the pause menu.", 
                    new Vector2(GraphicsDevice.Viewport.Width / 2 - arial20.MeasureString("Press 'P' to go to the pause menu.").X / 2, arial20.MeasureString("E").Y), Color.White);
                spriteBatch.DrawString(arial20, "Press 'I' to go back to game.",
                    new Vector2(GraphicsDevice.Viewport.Width / 2 - arial20.MeasureString("Press 'I' to go back to game.").X / 2, arial20.MeasureString("E").Y * 2),
                    Color.White);
                spriteBatch.End();
            }
            if(gameState == GameState.Finish)
            {
                spriteBatch.Begin();

                spriteBatch.Draw(
                            congratsTexture,                                                                   // Texture
                            new Vector2((GraphicsDevice.Viewport.Width / 2) - (congratsTexture.Width / 4) + 30,
                            (GraphicsDevice.Viewport.Height / 2) - ((congratsTexture.Height / 2) - 50)),              // Coordinates
                            new Rectangle(
                                0,
                                0,                                                                          // Portion
                                congratsTexture.Width,
                                congratsTexture.Height
                                ),
                            Color.White,                                                                    // Color
                            0.0f,                                                                           // Rotation
                            Vector2.Zero,                                                                   // Origin
                            new Vector2(.4f, .4f),                                                          // Scale
                            SpriteEffects.None,                                                             // Effects
                            0                                                                               // Layer depth
                            );

                menuButton.Draw(spriteBatch);

                // Draw the Mouse:
                // Record the coordinates of the mouse
                Vector2 mousePosition = new Vector2(mouse.X, mouse.Y);
                spriteBatch.Draw(cursorTexture, mousePosition, Color.White);

                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        /// <summary>
        /// Returns true if this is the first time a key has been pressed.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool SinglePress(Keys key)
        {
            if (kbState.IsKeyDown(key) && prevKbState.IsKeyUp(key))
                return true;
            else
                return false;
        }
    }
}
