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
 


    // Letters
    
    Texture2D alfabetoTexture;
    List<LetraColetavel> letrasNoMapa = new List<LetraColetavel>();
        
    //Letters falling sky
    // Banner e Texturas
    Texture2D alfabetoCinzaTexture;
    int indiceLetraAtual = 0; // 0 = A, 1 = B...
    string ordemAlfabeto = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    // Letras Caindo
    List<LetraColetavel> letrasCaindo = new List<LetraColetavel>();
    double spawnTimer = 0;
    double spawnInterval = 3; // Segundos entre cada queda
    Dictionary<string, Rectangle> letrasSource; // Mover para global para acessar no Update
    Random random = new Random();    

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
        
        alfabetoTexture = Content.Load<Texture2D>("letters/alfabeto");
        alfabetoCinzaTexture = Content.Load<Texture2D>("letters/alfabeto_cinza");

        //Letra A
        //Rectangle rectA = new Rectangle(0, 0, 150, 150); // Exemplo: letra 'A' na posição (0,0) com tamanho 32x32
        //letrasNoMapa.Add(new LetraColetavel('A', new Vector2(100, 100), rectA));

        // Adicione isso no seu LoadContent ou em uma classe de utilitário
        letrasSource = new Dictionary<string, Rectangle>
        {
            // Linha 1
            { "A", new Rectangle(27, 20, 112, 152) },
            { "B", new Rectangle(150, 20, 104, 150) },
            { "C", new Rectangle(262, 19, 106, 153) },
            { "D", new Rectangle(384, 19, 108, 152) },
            { "E", new Rectangle(514, 19, 86, 152) },
            { "F", new Rectangle(550, 201, 87, 153) },
            { "G", new Rectangle(612, 18, 116, 154) },

            // Linha 2
            { "H", new Rectangle(17, 202, 102, 152) },
            { "I", new Rectangle(142, 201, 35, 153) },
            { "J", new Rectangle(202, 202, 94, 154) },
            { "K", new Rectangle(311, 200, 101, 154) },
            { "L", new Rectangle(645, 202, 81, 153) },
            { "N", new Rectangle(426, 200, 106, 154) },
            { "M", new Rectangle(744, 201, 126, 152) },
            

            // Linha 3
            { "O", new Rectangle(12, 386, 119, 151) },
            { "P", new Rectangle(145, 386, 99, 150) },
            { "Q", new Rectangle(744, 18, 116, 154) },
            //{ "I2", new Rectangle(255, 387, 37, 150) }, // Segundo 'I' na imagem
            { "R", new Rectangle(304, 387, 102, 150) },
            { "S", new Rectangle(412, 384, 96, 154) },
            { "T", new Rectangle(508, 388, 102, 149) },
            { "U", new Rectangle(744, 387, 126, 152) },
            //{ "R2", new Rectangle(622, 386, 102, 152) }, // Segundo 'R' na imagem

            // Linha 4
            { "V", new Rectangle(13, 570, 109, 154) },
            { "W", new Rectangle(132, 570, 156, 154) },
            //{ "X", new Rectangle(290, 572, 108, 151) },
            { "X", new Rectangle(408, 572, 106, 151) }, // Segundo 'X'
            { "Y", new Rectangle(517, 571, 104, 151) },
            { "Z", new Rectangle(620, 571, 101, 152) }
        };

        foreach (var letra in letrasSource)
        {
            char caractere = letra.Key[0];
            //letrasNoMapa.Add(new LetraColetavel(caractere, new Vector2(100 + letrasNoMapa.Count * 60, 200), letra.Value));
            letrasNoMapa.Add(new LetraColetavel(caractere, new Vector2(100, 100), letra.Value));
        }

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
        
        
        // --- Lógica de Coleta de Letras ---

        // --- 1. Lógica de Spawn (Cair 2 letras) ---
        spawnTimer += dt;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0;

            // Sorteia a posição da primeira letra
            if (letrasCaindo.Count == 0) 
            {
                SpawnDuplaDeLetras();
            }
        }
        // --- 2. Atualizar posição das letras caindo e Colisão ---
        Rectangle playerRect = new Rectangle((int)_player.Position.X, (int)_player.Position.Y, (int)(50), (int)(80));

        for (int i = letrasCaindo.Count - 1; i >= 0; i--)
        {
            var letra = letrasCaindo[i];
            letra.Position.Y += 100f * dt; // Velocidade de queda

            Rectangle letraRect = new Rectangle((int)letra.Position.X, (int)letra.Position.Y, 40, 40);

            if (playerRect.Intersects(letraRect))
            {
                // Se pegou a letra certa na ordem
                if (letra.Caractere == ordemAlfabeto[indiceLetraAtual])
                {
                    indiceLetraAtual++; // Avança no alfabeto!
                    if (indiceLetraAtual >= ordemAlfabeto.Length) indiceLetraAtual = 0; // Reinicia se acabar
                }
                
                spawnTimer = 0; 
                SpawnDuplaDeLetras();
                letrasCaindo.Clear();
                break;
            }
            else if (letra.Position.Y > GraphicsDevice.Viewport.Height)
            {
                letrasCaindo.RemoveAt(i); // Remove se sumir da tela
            }
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

        for (int i = 0; i < ordemAlfabeto.Length; i++)
        {
            string charAtual = ordemAlfabeto[i].ToString();
            // Se a letra já foi coletada, desenha Colorido. Se não, desenha Cinza.
            Texture2D texUsar = (i < indiceLetraAtual) ? alfabetoTexture : alfabetoCinzaTexture;

            _spriteBatch.Draw(texUsar, bannerPos, letrasSource[charAtual], Color.White, 0f, Vector2.Zero, escalaBanner, SpriteEffects.None, 0f);
            
            bannerPos.X += (letrasSource[charAtual].Width * escalaBanner) + 5;
            if (bannerPos.X > GraphicsDevice.Viewport.Width - 40) {
                bannerPos.X = 20;
                bannerPos.Y += 30;
            }
        }

        // 3. Desenhar Letras Caindo
        foreach (var letra in letrasCaindo)
        {
            _spriteBatch.Draw(alfabetoTexture, letra.Position, letra.SourceRect, Color.White, 0f, Vector2.Zero, 0.3f, SpriteEffects.None, 0f);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }


    void SpawnDuplaDeLetras()
    {
        // 1. Sorteia as posições X
        int posX1 = random.Next(50, GraphicsDevice.Viewport.Width - 100);
        int posX2;
        do {
            posX2 = random.Next(50, GraphicsDevice.Viewport.Width - 100);
        } while (Math.Abs(posX1 - posX2) < 150);

        // 2. Spawn Letra Certa
        SpawnLetra(ordemAlfabeto[indiceLetraAtual], posX1);

        // 3. Spawn Letra Errada
        char letraErrada;
        do {
            letraErrada = ordemAlfabeto[random.Next(ordemAlfabeto.Length)];
        } while (letraErrada == ordemAlfabeto[indiceLetraAtual]);
        
        SpawnLetra(letraErrada, posX2);
    }

    void SpawnLetra(char c, int posX)
    {
        string chave = c.ToString();
        if (letrasSource.ContainsKey(chave))
        {
            // Usa o posX que foi sorteado lá fora
            Vector2 pos = new Vector2(posX, -150); 
            letrasCaindo.Add(new LetraColetavel(c, pos, letrasSource[chave]));
        }
    }
}
