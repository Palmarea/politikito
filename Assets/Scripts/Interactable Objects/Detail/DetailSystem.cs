using Febucci.UI;
using Game.Managers.Mouse;
using Game.Managers.Timing;
using Game.Systems.Input;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Systems.Interaction.Detail
{
    public class DetailSystem : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private DetailCreator Creator;
        
        [Header("References")]
        [SerializeField] private GameObject DetailCanvasUI;

        [Header("Achievement References")]
        [SerializeField] private GameObject TextBox;
        [SerializeField] private TypewriterByCharacter DetailText;

        [Header("Milestone References")]
        [SerializeField] private Image NewspaperImage;
        [SerializeField] private TextMeshProUGUI NewspaperText;


        private bool m_Occupied = false;
        private bool suscribed = false;

        public static DetailSystem Instance;

        #region Singleton
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(Instance);
            }
            else
            {
                Instance = this;
            }

        }
        #endregion

        private void Start()
        {
            if (!suscribed)
            {
                MouseManager.Instance.OnSimpleClickPerformed += HideDetail;
                suscribed = true;
            }

            TextBox.SetActive(false);
            NewspaperImage.gameObject.SetActive(false);
            NewspaperText.text = "";
        }

        public void ShowDetail(DetailObjData data)
        {
            if (m_Occupied) return;
            
            if (!DetailCanvasUI.activeInHierarchy) DetailCanvasUI.SetActive(true);

            m_Occupied = true;

            InterruptionManager.Instance.EnableInterruption(InterruptionType.DETAIL);

            if (data.type == DetailType.ACHIEVEMENT)
            {
                TextBox.SetActive(true);
                DetailText.ShowText(string.Format(data.description, GameData.Instance.GetPlayerLabel().ToUpper()));
            }
            else
            {
                NewspaperImage.gameObject.SetActive(true);
                NewspaperText.SetText(string.Format(data.description, GameData.Instance.GetPlayerLabel()));
            }

        }

        private void HideDetail()
        {
            if (!m_Occupied) return;
            
            m_Occupied = false;

            TextBox.SetActive(false);
            DetailText.ShowText("");

            NewspaperImage.gameObject.SetActive(false);
            NewspaperText.text = "";

            InterruptionManager.Instance.DisableInteruption();
            MouseManager.Instance.UpdateOcuppiedState(false);

            DetailCanvasUI.SetActive(false);
        }

        public void RequestDetailObjCreation(string objID, Vector3 spawnPosition, bool needFocus = true)
        {
            Creator.CreateDetailObject(objID, spawnPosition, needFocus);
        }

        private void OnEnable()
        {
            if (InputManager.Instance == null) return;

            MouseManager.Instance.OnSimpleClickPerformed += HideDetail;
            suscribed = true;
        }

        private void OnDisable()
        {
            MouseManager.Instance.OnSimpleClickPerformed -= HideDetail;
        }
    }
}