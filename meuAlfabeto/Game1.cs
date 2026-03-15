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
