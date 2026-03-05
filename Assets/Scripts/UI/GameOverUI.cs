using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Character;

namespace Game.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private GameObject gameOverPanel;

        [Header("Contenido")]
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text messageText;
        [SerializeField] private TMP_Text daysText;
        [SerializeField] private Image backgroundImage;

        [Header("Boton")]
        [SerializeField] private Button retryButton;

        [Header("Mensajes por Stat")]
        [TextArea(2, 4)]
        [SerializeField]
        private string charismaMessage =
            "Tu carisma se apago. La gente dejo de escucharte y el pueblo te olvido.";
        [TextArea(2, 4)]
        [SerializeField]
        private string knowledgeMessage =
            "Dejaste de prepararte. Sin conocimiento, tus decisiones destruyeron todo.";
        [TextArea(2, 4)]
        [SerializeField]
        private string determinationMessage =
            "El sistema te desgasto. No te corrompieron, simplemente te rendiste.";

        [Header("Colores por Stat")]
        [SerializeField] private Color charismaDeathColor = new Color(0.3f, 0.1f, 0f);
        [SerializeField] private Color knowledgeDeathColor = new Color(0.2f, 0.2f, 0.2f);
        [SerializeField] private Color determinationDeathColor = new Color(0.1f, 0f, 0.2f);

        [Header("Referencia")]
        [SerializeField] private TamaCharacterStats characterStats;

        private void Start()
        {
            if (gameOverPanel != null)
                gameOverPanel.SetActive(false);

            if (retryButton != null)
                retryButton.onClick.AddListener(OnRetry);

            if (characterStats != null)
                characterStats.OnStatDepleted += ShowGameOver;
        }

        public void ShowGameOver(string depletedStat)
        {
            if (gameOverPanel != null)
                gameOverPanel.SetActive(true);

            if (titleText != null)
                titleText.text = "Tu politico cayo...";

            switch (depletedStat)
            {
                case "Charisma":
                    if (messageText != null) messageText.text = charismaMessage;
                    if (backgroundImage != null) backgroundImage.color = charismaDeathColor;
                    break;
                case "Knowledge":
                    if (messageText != null) messageText.text = knowledgeMessage;
                    if (backgroundImage != null) backgroundImage.color = knowledgeDeathColor;
                    break;
                case "Determination":
                    if (messageText != null) messageText.text = determinationMessage;
                    if (backgroundImage != null) backgroundImage.color = determinationDeathColor;
                    break;
            }

            if (daysText != null)
                daysText.text = "Sobreviviste " + GetDayCount() + " dias";
        }

        private int GetDayCount()
        {
            if (Game.Managers.Timing.TimeManager.Instance != null)
            {
                // Asumiendo 60 segundos por dia, ajusta segun tu DayCycleConfig
                return Mathf.FloorToInt(Game.Managers.Timing.TimeManager.Instance.CurrentTime / 60f) + 1;
            }
            return 1;
        }

        private void OnRetry()
        {
            if (gameOverPanel != null)
                gameOverPanel.SetActive(false);

            if (characterStats != null)
                characterStats.ResetStats();
        }

        private void OnDestroy()
        {
            if (characterStats != null)
                characterStats.OnStatDepleted -= ShowGameOver;
        }
    }
}