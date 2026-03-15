using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace meuAlfabeto
{
    public class StageLevel
    {
            
        public Texture2D alfabetoTexture;
        public List<LetraColetavel> letrasNoMapa = new List<LetraColetavel>();
            
        //Letters falling sky
        // Banner e Texturas
        public Texture2D alfabetoCinzaTexture;
        public int indiceLetraAtual = 0; // 0 = A, 1 = B...
        public string ordemAlfabeto = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public string[] stages = new string[]
        {
            "AEIOU", "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        }; 

        // Letras Caindo
        public List<LetraColetavel> _letrasCaindo = new List<LetraColetavel>();
        double spawnTimer = 0;
        double spawnInterval = 3; // Segundos entre cada queda
        public Dictionary<string, Rectangle> _letrasSource; // Mover para global para acessar no Update
        Random random = new Random();    
        public StageLevel(int stageIndex, ContentManager content)
        {
            
            if (stageIndex >= 0 && stageIndex < stages.Length)
            {
                ordemAlfabeto = stages[stageIndex];
                LoadLevel(content);
            }
            
        }

        public void LoadLevel(ContentManager content)
        {

            alfabetoTexture = content.Load<Texture2D>("letters/alfabeto");
            alfabetoCinzaTexture = content.Load<Texture2D>("letters/alfabeto_cinza");

            //Letra A
            //Rectangle rectA = new Rectangle(0, 0, 150, 150); // Exemplo: letra 'A' na posição (0,0) com tamanho 32x32
            //letrasNoMapa.Add(new LetraColetavel('A', new Vector2(100, 100), rectA));

            // Adicione isso no seu LoadContent ou em uma classe de utilitário
            _letrasSource = new Dictionary<string, Rectangle>
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

            foreach (var letra in _letrasSource)
            {
                char caractere = letra.Key[0];
                //letrasNoMapa.Add(new LetraColetavel(caractere, new Vector2(100 + letrasNoMapa.Count * 60, 200), letra.Value));
                letrasNoMapa.Add(new LetraColetavel(caractere, new Vector2(100, 100), letra.Value));
            }

            /*    
            alfabetoTexture = content.Load<Texture2D>("alfabeto/alfabeto_colorido");
            alfabetoCinzaTexture = content.Load<Texture2D>("alfabeto/alfabeto_cinza");
            _letrasSource = new Dictionary<string, Rectangle>();
            for (int i = 0; i < 26; i++)
            {
                _letrasSource.Add(((char)('A' + i)).ToString(), new Rectangle(i * 32, 0, 32, 32));
            }

            // Carregar as letras do mapa
            letrasNoMapa.Clear();
            for (int i = 0; i < ordemAlfabeto.Length; i++)
            {
                char letra = ordemAlfabeto[i];
                Vector2 posicao = new Vector2(100 + i * 50, 300); // Exemplo de posição
                letrasNoMapa.Add(new LetraColetavel(letra, posicao, _letrasSource[letra.ToString()]));
            }
            */
        }

        public void Update(GameTime gameTime, GraphicsDevice graphics, Vector2 playerPos)
        {
  
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // --- 1. Lógica de Spawn (Cair 2 letras) ---
            spawnTimer += dt;
            if (spawnTimer >= spawnInterval)
            {
                spawnTimer = 0;

                // Sorteia a posição da primeira letra
                if (_letrasCaindo.Count == 0) 
                {
                    SpawnDuplaDeLetras(graphics);
                }
            }
            // --- 2. Atualizar posição das letras caindo e Colisão ---
            Rectangle playerRect = new Rectangle((int)playerPos.X, (int)playerPos.Y, (int)(50), (int)(80));

            for (int i = _letrasCaindo.Count - 1; i >= 0; i--)
            {
                var letra = _letrasCaindo[i];
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
                    SpawnDuplaDeLetras(graphics);
                    _letrasCaindo.Clear();
                    break;
                }
                else if (letra.Position.Y > graphics.Viewport.Height)
                {
                    _letrasCaindo.RemoveAt(i); // Remove se sumir da tela
                }
            }

        }



        public void SpawnDuplaDeLetras(GraphicsDevice graphics)
        {
            // 1. Sorteia as posições X
            int posX1 = random.Next(50, graphics.Viewport.Width - 100);
            int posX2;
            do {
                posX2 = random.Next(50, graphics.Viewport.Width - 100);
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

        public void SpawnLetra(char c, int posX)
        {
            string chave = c.ToString();
            if (_letrasSource.ContainsKey(chave))
            {
                // Usa o posX que foi sorteado lá fora
                Vector2 pos = new Vector2(posX, -150); 
                _letrasCaindo.Add(new LetraColetavel(c, pos, _letrasSource[chave]));
            }
        }
        
    }
}