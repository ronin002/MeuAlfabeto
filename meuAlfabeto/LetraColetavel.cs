using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace meuAlfabeto
{
    public class LetraColetavel : GenericStaticObj
    {
        public char Caractere;
        public bool Coletada = false;

        public LetraColetavel(char letra, Vector2 pos, Rectangle source)
                                                        : base(pos, source)
        {
            Caractere = letra;
            Position = pos;
            SourceRect = source;
        }
    }

}