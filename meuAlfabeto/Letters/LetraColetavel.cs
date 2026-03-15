using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace meuAlfabeto
{
    public class LetraColetavel
    {
        public char Caractere;
        public Vector2 Posicao;
        public Rectangle SourceRect; // Onde a letra está na imagem original
        public bool Coletada = false;

        public LetraColetavel(char letra, Vector2 pos, Rectangle source)
        {
            Caractere = letra;
            Posicao = pos;
            SourceRect = source;
        }
    }

}