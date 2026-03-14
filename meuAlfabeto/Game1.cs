using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;

namespace meuAlfabeto;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    


    // Enviromment
    Texture2D backgroundTexture;

    // Player Variables

    // Animations
    List<Texture2D> idleSprites;
    List<Texture2D> walkSprites;
    List<Texture2D> jumpSprites;

    Vector2 playerPosition;
    int currentFrame = 0;
    double timer = 0;
    double frameTime = 0.04;


    List<Texture2D> currentAnimation;
    float playerSpeed = 200f;        
    SpriteEffects flip = SpriteEffects.None;


    //GRAVITY
    bool isJumping = false;
    float verticalVelocity = 0;
    float gravity = 1200f; // Força que puxa para baixo
    float jumpForce = -500f; // Força do pulo (negativo sobe)
    float groundLevel = 300f; // Posição Y do seu "chão"

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


        //player
        playerPosition = new Vector2(300, 320);

        idleSprites = new List<Texture2D>();
        for (int i = 1; i <= 16; i++) // Ajuste o '2' para o total de fotos que você tiver
        {
            string fileName = $"player/idle/Idle_{i}";
            idleSprites.Add(Content.Load<Texture2D>(fileName));
        }

        walkSprites = new List<Texture2D>();
        for (int i = 1; i <= 19; i++)
        {
            string indice = i < 10 ? $"0{i}" : i.ToString();
            string fileName = $"player/walk/Walk_{indice}";
            walkSprites.Add(Content.Load<Texture2D>(fileName));
        }

        jumpSprites = new List<Texture2D>();
        for (int i = 1; i <= 30; i++)
        {
            string indice = i < 10 ? $"0{i}" : i.ToString();
            string fileName = $"player/jump/Jump_{indice}";
            jumpSprites.Add(Content.Load<Texture2D>(fileName));
        }

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        var kstate = Keyboard.GetState();
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        
        
        List<Texture2D> oldAnimation = currentAnimation;

        if (playerPosition.Y < groundLevel) // Se estiver acima do chão
        {
            verticalVelocity += gravity * dt; // Gravidade puxa pra baixo
        }
        else // Se tocou no chão
        {
            verticalVelocity = 0;
            playerPosition.Y = groundLevel;
            isJumping = false;
        }

        // 2. Comando de Pulo
        if (kstate.IsKeyDown(Keys.Space) && !isJumping)
        {
            verticalVelocity = jumpForce;
            isJumping = true;
        }

        // 3. Aplica a velocidade vertical na posição
        playerPosition.Y += verticalVelocity * dt;

        //4. Lógica de Animação (Estados)
        if (kstate.IsKeyDown(Keys.Left))
        {
            playerPosition.X -= playerSpeed * dt;
            flip = SpriteEffects.FlipHorizontally;
        }
        if (kstate.IsKeyDown(Keys.Right))
        {
            playerPosition.X += playerSpeed * dt;
            flip = SpriteEffects.None;
        }

        // 2. Definição da Animação (Aqui decidimos qual sprite mostrar)
        if (isJumping)
        {
            currentAnimation = jumpSprites;
        }
        else if (kstate.IsKeyDown(Keys.Left) || kstate.IsKeyDown(Keys.Right))
        {
            currentAnimation = walkSprites;
        }
        else
        {
            currentAnimation = idleSprites;
        }

        // Reset de frame se trocar animação
        if (oldAnimation != currentAnimation)
        {
            currentFrame = 0;
            timer = 0;
        }

        // --- Lógica de Animação ---
        timer += gameTime.ElapsedGameTime.TotalSeconds;

        if (timer >= frameTime)
        {
            currentFrame++;
            timer = 0;
            
            // Reinicia a animação se chegar ao fim da lista
            if (currentFrame >= currentAnimation.Count) 
                currentFrame = 0;
        }

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
       
        int safeFrame = currentFrame % currentAnimation.Count;

        _spriteBatch.Draw(
            currentAnimation[safeFrame], 
            playerPosition, 
            null, 
            Color.White, 
            0f, 
            Vector2.Zero, 
            scale, //0.3f, // Sua escala
            flip, // Aplica o efeito de espelhar
            0f
        );

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
