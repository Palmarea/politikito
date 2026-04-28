using UnityEngine;
using UnityEngine.U2D;

namespace Game.Utils
{
    public static class SpriteAtlasHandling
    {
        public static Sprite GetSpriteFromAtlas(SpriteAtlas atlas, string spriteName)
        {
            return atlas.GetSprite(spriteName);
        }
    }
}
