using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Character;

namespace Game.Milestones
{
    [System.Serializable]
    public class Milestone
    {
        public string name;
        public float averageRequired;
        [TextArea(1, 3)]
        public string announcement;
        public bool reached;
    }

    public class MilestoneManager : MonoBehaviour
    {
        public static MilestoneManager Instance { get; private set; }

        [Header("Hitos")]
        [SerializeField]
        private Milestone[] milestones = new Milestone[]
        {
            new Milestone { name = "Candidato Local", averageRequired = 30f,
                announcement = "La gente empieza a conocerte. Eres candidato local." },
            new Milestone { name = "Lider de Partido", averageRequired = 50f,
                announcement = "Tu partido crece. Ahora lideras el movimiento." },
            new Milestone { name = "Congresista", averageRequired = 70f,
                announcement = "Llegaste al Congreso. Aqui las cosas se ponen serias." },
            new Milestone { name = "Presidente", averageRequired = 90f,
                announcement = "Lo lograste. Eres Presidente. Pero..." }
        };

        [Header("UI Popup")]
        [SerializeField] private GameObject milestonePopup;
        [SerializeField] private TMP_Text milestoneTitle;
        [SerializeField] private TMP_Text milestoneMessage;
        [SerializeField] private Button closeMilestoneButton;

        [Header("HUD")]
        [SerializeField] private TMP_Text currentRankText;

        [Header("Referencia")]
        [SerializeField] private TamaCharacterStats characterStats;

        private int currentMilestoneIndex = -1;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            if (milestonePopup != null)
                milestonePopup.SetActive(false);
            if (closeMilestoneButton != null)
                closeMilestoneButton.onClick.AddListener(ClosePopup);
            if (currentRankText != null)
                currentRankText.text = "Ciudadano";

            if (characterStats != null)
                characterStats.OnStatsChanged += CheckMilestones;
        }

        private void CheckMilestones(TamaStat c, TamaStat k, TamaStat d)
        {
            if (characterStats == null) return;
            float avg = characterStats.AverageStats;

            for (int idx = milestones.Length - 1; idx >= 0; idx--)
            {
                if (!milestones[idx].reached && avg >= milestones[idx].averageRequired)
                {
                    if (idx > currentMilestoneIndex)
                    {
                        milestones[idx].reached = true;
                        currentMilestoneIndex = idx;
                        ShowMilestone(milestones[idx]);
                        break;
                    }
                }
            }
        }

        private void ShowMilestone(Milestone milestone)
        {
            if (currentRankText != null)
                currentRankText.text = milestone.name;

            if (milestonePopup != null && milestoneTitle != null && milestoneMessage != null)
            {
                milestoneTitle.text = milestone.name;
                milestoneMessage.text = milestone.announcement;
                milestonePopup.SetActive(true);
            }

            Debug.Log("HITO ALCANZADO: " + milestone.name);
        }

        private void ClosePopup()
        {
            if (milestonePopup != null)
                milestonePopup.SetActive(false);
        }

        public string GetCurrentRank()
        {
            if (currentMilestoneIndex < 0) return "Ciudadano";
            return milestones[currentMilestoneIndex].name;
        }

        private void OnDestroy()
        {
            if (characterStats != null)
                characterStats.OnStatsChanged -= CheckMilestones;
        }
    }
}