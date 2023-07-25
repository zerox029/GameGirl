using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameGirlDisplay;

public class LCD : Game
{
  private GraphicsDeviceManager _graphics;
  private SpriteBatch _spriteBatch;

  public LCD()
  {
    _graphics = new GraphicsDeviceManager(this);
    _graphics.PreferredBackBufferWidth = 256;
    _graphics.PreferredBackBufferHeight = 256;
    Content.RootDirectory = "Content";
    IsMouseVisible = true;
  }

  protected override void Initialize()
  {
    // TODO: Add your initialization logic here

    base.Initialize();
  }

  protected override void LoadContent()
  {
    _spriteBatch = new SpriteBatch(GraphicsDevice);

    // TODO: use this.Content to load your game content here
  }

  protected override void Update(GameTime gameTime)
  {
    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
      Exit();

    // TODO: Add your update logic here

    base.Update(gameTime);
  }

  protected override void Draw(GameTime gameTime)
  {
    GraphicsDevice.Clear(Color.CornflowerBlue);

    // TODO: Add your drawing code here

    base.Draw(gameTime);
  }
}
