using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.U2D;

namespace Game.Utils
{
    public static class SpriteAtlasHandling
    {
        public static Sprite GetSpriteFromAtlas(SpriteAtlas atlas, string spriteName)
        {
            return atlas.GetSprite(spriteName);
        }

        public static Sprite GetLocalizedSprite(SpriteAtlas atlas, string spriteName)
        {
            string suffix = LocalizationSettings.SelectedLocale.Identifier.Code switch
            {
                "es" => "_SPA",
                _ => "_ENG"
            };

            return atlas.GetSprite(spriteName + suffix);
        }
    }
}