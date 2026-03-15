using MeuAlfabeto;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;


namespace meuAlfabeto;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    


    // Enviromment
    

    Player _player;
 

    StageLevel _stageLevel;
    


    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
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

        //Environment
        //backgroundTexture = Content.Load<Texture2D>("background/florest1");

        (string, int) maskIdle = ("player/idle/Idle_{0}", 16); //path and size of the idle sprites
        (string, int) maskWalk = ("player/walk/Walk_{0}", 19);
        (string, int) maskJump = ("player/jump/Jump_{0}", 30);

        _player = new Player(new Vector2(300, 320), Content, maskIdle, maskWalk, maskJump);

        // Stage
        _stageLevel = new StageLevel(0, Content); // Carrega o stage 0 (AEIOU)
        

    }

    

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        var kstate = Keyboard.GetState();
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // PLAYER MOVEMENT + GRAVITY + JUMPING
        _player.Update(gameTime, GraphicsDevice);
        
        _stageLevel.Update(gameTime, GraphicsDevice, _player.Position);

 
        base.Update(gameTime);
    }

    

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);


        // TODO: Add your drawing code here
        _spriteBatch.Begin();

        _stageLevel.Draw(_spriteBatch, GraphicsDevice);

        _player.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }


}
