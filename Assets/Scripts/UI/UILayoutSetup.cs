using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game.UI
{
    public class UILayoutSetup : MonoBehaviour
    {
        [Header("Barras")]
        [SerializeField] private RectTransform charismaBar;
        [SerializeField] private RectTransform knowledgeBar;
        [SerializeField] private RectTransform determinationBar;

        [Header("Botones")]
        [SerializeField] private RectTransform waterButton;
        [SerializeField] private RectTransform cleanButton;
        [SerializeField] private RectTransform inspireButton;

        [Header("Cooldowns")]
        [SerializeField] private RectTransform waterCooldown;
        [SerializeField] private RectTransform cleanCooldown;
        [SerializeField] private RectTransform inspireCooldown;

        [Header("Info")]
        [SerializeField] private RectTransform dayText;

        [Header("Config")]
        [SerializeField] private float barWidth = 300f;
        [SerializeField] private float barHeight = 35f;
        [SerializeField] private float barSpacing = 10f;
        [SerializeField] private float buttonWidth = 160f;
        [SerializeField] private float buttonHeight = 55f;
        [SerializeField] private float buttonSpacing = 20f;

        void Start()
        {
            SetupLayout();
        }

        [ContextMenu("Aplicar Layout")]
        public void SetupLayout()
        {
            // === BARRAS: arriba al centro ===
            float barsStartY = -40f;

            SetupRect(charismaBar, 0.5f, 1f, 0.5f, 1f,
                0f, barsStartY, barWidth, barHeight);

            SetupRect(knowledgeBar, 0.5f, 1f, 0.5f, 1f,
                0f, barsStartY - (barHeight + barSpacing), barWidth, barHeight);

            SetupRect(determinationBar, 0.5f, 1f, 0.5f, 1f,
                0f, barsStartY - 2f * (barHeight + barSpacing), barWidth, barHeight);

            // === DIA: arriba a la derecha ===
            SetupRect(dayText, 1f, 1f, 1f, 1f,
                -80f, -40f, 150f, 40f);

            // === BOTONES: abajo al centro ===
            float totalButtonsWidth = 3f * buttonWidth + 2f * buttonSpacing;
            float startX = -totalButtonsWidth / 2f + buttonWidth / 2f;

            SetupRect(waterButton, 0.5f, 0f, 0.5f, 0f,
                startX, 80f, buttonWidth, buttonHeight);

            SetupRect(cleanButton, 0.5f, 0f, 0.5f, 0f,
                0f, 80f, buttonWidth, buttonHeight);

            SetupRect(inspireButton, 0.5f, 0f, 0.5f, 0f,
                -startX, 80f, buttonWidth, buttonHeight);

            // === COOLDOWNS: debajo de cada boton ===
            SetupRect(waterCooldown, 0.5f, 0f, 0.5f, 0f,
                startX, 40f, 60f, 25f);

            SetupRect(cleanCooldown, 0.5f, 0f, 0.5f, 0f,
                0f, 40f, 60f, 25f);

            SetupRect(inspireCooldown, 0.5f, 0f, 0.5f, 0f,
                -startX, 40f, 60f, 25f);

            // Setup fonts for any TMP without font
            FixFonts();
        }

        private void SetupRect(RectTransform rt, float anchorMinX, float anchorMinY,
            float anchorMaxX, float anchorMaxY, float posX, float posY, float width, float height)
        {
            if (rt == null) return;

            rt.anchorMin = new Vector2(anchorMinX, anchorMinY);
            rt.anchorMax = new Vector2(anchorMaxX, anchorMaxY);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = new Vector2(posX, posY);
            rt.sizeDelta = new Vector2(width, height);
        }

        private void FixFonts()
        {
            TMP_Text[] allTexts = GetComponentsInChildren<TMP_Text>(true);
            TMP_FontAsset defaultFont = null;

            // Buscar una fuente que ya este asignada
            foreach (var txt in allTexts)
            {
                if (txt.font != null)
                {
                    defaultFont = txt.font;
                    break;
                }
            }

            if (defaultFont == null)
            {
                defaultFont = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
            }

            if (defaultFont != null)
            {
                foreach (var txt in allTexts)
                {
                    if (txt.font == null)
                    {
                        txt.font = defaultFont;
                    }
                }
            }
        }
    }
}