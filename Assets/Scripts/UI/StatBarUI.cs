using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Character;

namespace Game.UI
{
    public class StatBarUI : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private TMP_Text labelText;
        [SerializeField] private TMP_Text valueText;

        [Header("Colores")]
        [SerializeField] private Color healthyColor = new Color(0.2f, 0.8f, 0.2f);
        [SerializeField] private Color warningColor = new Color(0.9f, 0.9f, 0.1f);
        [SerializeField] private Color dangerColor = new Color(0.9f, 0.1f, 0.1f);

        public void UpdateBar(TamaStat stat)
        {
            float normalized = stat.Normalized;

            if (fillImage != null)
            {
                fillImage.fillAmount = normalized;

                if (normalized <= 0.25f)
                    fillImage.color = dangerColor;
                else if (normalized <= 0.5f)
                    fillImage.color = warningColor;
                else
                    fillImage.color = healthyColor;
            }

            if (labelText != null)
                labelText.text = stat.Name;

            if (valueText != null)
                valueText.text = Mathf.RoundToInt(stat.Value).ToString();
        }
    }
}