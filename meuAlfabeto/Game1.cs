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
    Texture2D backgroundTexture;

    // Player Variables


    Player _player;
 

    // StageLevel

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
        backgroundTexture = Content.Load<Texture2D>("background/florest1");

        _player = new Player();
        (string, int) maskIdle = ("player/idle/Idle_{0}", 16); //path and size of the idle sprites
        (string, int) maskWalk = ("player/walk/Walk_{0}", 19);
        (string, int) maskJump = ("player/jump/Jump_{0}", 30);
        _player.LoadPlayer(Content, maskIdle, maskWalk, maskJump);

      

        // Letters
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
        _player.Update(gameTime);
        
        _stageLevel.Update(gameTime, GraphicsDevice, _player.Position);

 
        base.Update(gameTime);
    }

    

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);


        // TODO: Add your drawing code here
        _spriteBatch.Begin();

        Rectangle screenBounds = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
    
        _spriteBatch.Draw(backgroundTexture, screenBounds, Color.White);
    
        float scale = 0.2f;
       
        int safeFrame =  _player._currentFrame %  _player._currentAnimation.Count;

        _spriteBatch.Draw(
            _player._currentAnimation[safeFrame], 
            _player.Position, 
            null, 
            Color.White, 
            0f, 
            Vector2.Zero, 
            scale, //0.3f, // Sua escala
            _player._flip, // Aplica o efeito de espelhar
            0f
        );

        // 2. Banner do Alfabeto (Topo da tela)
        Vector2 bannerPos = new Vector2(20, 20);
        float escalaBanner = 0.22f; 
        
        for (int i = 0; i <  _stageLevel.ordemAlfabeto.Length; i++)
        {
            string charAtual = _stageLevel.ordemAlfabeto[i].ToString();
            // Se a letra já foi coletada, desenha Colorido. Se não, desenha Cinza.
            Texture2D texUsar = (i < _stageLevel.indiceLetraAtual) ? _stageLevel.alfabetoTexture : _stageLevel.alfabetoCinzaTexture;

            _spriteBatch.Draw(texUsar, bannerPos, _stageLevel._letrasSource[charAtual], Color.White, 0f, Vector2.Zero, escalaBanner, SpriteEffects.None, 0f);
            
            bannerPos.X += (_stageLevel._letrasSource[charAtual].Width * escalaBanner) + 5;
            if (bannerPos.X > GraphicsDevice.Viewport.Width - 40) {
                bannerPos.X = 20;
                bannerPos.Y += 30;
            }
        }

        // 3. Desenhar Letras Caindo
        foreach (var letra in _stageLevel._letrasCaindo)
        {
            _spriteBatch.Draw(_stageLevel.alfabetoTexture, letra.Position, letra.SourceRect, Color.White, 0f, Vector2.Zero, 0.3f, SpriteEffects.None, 0f);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }


}
