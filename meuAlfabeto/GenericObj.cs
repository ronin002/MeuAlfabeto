
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace meuAlfabeto
{
    public class GenericObj
    {
        // Propriedades (o que o player tem)
        public Vector2 Position;
        private List<Texture2D> _idleSprites, _walkSprites, _jumpSprites;
        private List<Texture2D> _currentAnimation;
        
        private int _currentFrame;
        private double _timer;
        private double _frameTime = 0.04;
        private float _speed = 200f;
        private SpriteEffects _flip = SpriteEffects.None;

        // Física
        private float _verticalVelocity = 0;
        private float _gravity = 1200f;
        private float _jumpForce = -500f;
        private float _groundLevel = 320f;
        public bool IsJumping { get; private set; }

        // Construtor
        public GenericObj(Vector2 startPosition, List<Texture2D> idle, List<Texture2D> walk, List<Texture2D> jump)
        {
            Position = startPosition;
            _idleSprites = idle;
            _walkSprites = walk;
            _jumpSprites = jump;
            _currentAnimation = _idleSprites;
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