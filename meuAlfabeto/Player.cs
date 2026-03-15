
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace meuAlfabeto
{
    public class Player
    {
        // Propriedades (o que o player tem)
        public Vector2 Position;
        private List<Texture2D> _idleSprites, _walkSprites, _jumpSprites;        
        public List<Texture2D> _currentAnimation;
        
        public int _currentFrame;
        public double _timer;
        public double _frameTime = 0.04;
        public float _speed = 200f;
        public SpriteEffects _flip = SpriteEffects.None;

        // Física
        public float _verticalVelocity = 0;
        public float _gravity = 1200f;
        public float _jumpForce = -500f;
        public float _groundLevel = 320f;
        public bool IsJumping { get; private set; }

        // Construtor
        public Player()
        {
            
        }
        public Player(Vector2 startPosition, List<Texture2D> idle, List<Texture2D> walk, List<Texture2D> jump)
        {
            Position = startPosition;
            _idleSprites = idle;
            _walkSprites = walk;
            _jumpSprites = jump;
            _currentAnimation = _idleSprites;
        }

        
        public void LoadPlayer(ContentManager content, (string, int) maskIdle, (string, int) maskWalk, (string, int) maskJump)
        {
            var playerPosition = new Vector2(300, 320);
            List<Texture2D> idleSprites = LoadPlayerSprites(content, "player/idle/Idle_{0}", 16);
            List<Texture2D> walkSprites = LoadPlayerSprites(content, "player/walk/Walk_{0}", 19);
            List<Texture2D> jumpSprites = LoadPlayerSprites(content, "player/jump/Jump_{0}", 30);
            
            _idleSprites = idleSprites;
            _walkSprites = walkSprites;
            _jumpSprites = jumpSprites;
        }

        public static List<Texture2D> LoadPlayerSprites(ContentManager content,  string maskPath, int endSprite)
        {
            List<Texture2D> sprites = new List<Texture2D>();
          
            for (int i = 1; i <= endSprite; i++) // Ajuste o '2' para o total de fotos que você tiver
            {                              
                string indice = i < 10 ? $"0{i}" : i.ToString();
                string fileName = string.Format(maskPath, indice);
                sprites.Add(content.Load<Texture2D>(fileName));
            }
            
            return sprites;
        }

        public void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var oldAnimation = _currentAnimation;

            // Gravidade e Pulo
            if (Position.Y < _groundLevel)
            {
                _verticalVelocity += _gravity * dt;
                IsJumping = true;
            }
            else
            {
                _verticalVelocity = 0;
                Position.Y = _groundLevel;
                IsJumping = false;
            }

            if (kstate.IsKeyDown(Keys.Space) && !IsJumping)
            {
                _verticalVelocity = _jumpForce;
                IsJumping = true;
            }

            Position.Y += _verticalVelocity * dt;

            // Movimento Horizontal
            bool moving = false;
            if (kstate.IsKeyDown(Keys.Left))
            {
                Position.X -= _speed * dt;
                _flip = SpriteEffects.FlipHorizontally;
                moving = true;
            }
            if (kstate.IsKeyDown(Keys.Right))
            {
                Position.X += _speed * dt;
                _flip = SpriteEffects.None;
                moving = true;
            }

            // Escolha da Animação
            if (IsJumping) _currentAnimation = _jumpSprites;
            else if (moving) _currentAnimation = _walkSprites;
            else _currentAnimation = _idleSprites;

            if (oldAnimation != _currentAnimation) _currentFrame = 0;

            // Update da Animação
            _timer += gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer >= _frameTime)
            {
                _timer = 0;
                _currentFrame = (_currentFrame + 1) % _currentAnimation.Count;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _currentAnimation[_currentFrame],
                Position,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                0.2f, // Escala
                _flip,
                0f
            );
        }

        // Retângulo para colisão (usado no Game1)
        public Rectangle GetBounds()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, 50, 80);
        }
    }
}