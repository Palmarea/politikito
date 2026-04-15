using Febucci.UI;
using Game.Managers.Mouse;
using Game.Managers.Timing;
using Game.Systems.Input;
using Game.Utils;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
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

        [Header("Data")]
        [SerializeField] private SpriteAtlas NewsSpriteAtlas;

        public event Action OnObjectCreated;
        private bool m_Occupied = false;
        private bool suscribed = false;

        public static DetailSystem Instance;

        private DetailObjData lastDetailObj;

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

            lastDetailObj = data;

            InterruptionManager.Instance.EnableInterruption(InterruptionType.DETAIL);

            if (data.type == DetailType.ACHIEVEMENT)
            {
                TextBox.SetActive(true);
                DetailText.ShowText(string.Format(data.description, GameData.Instance.GetPlayerLabel().ToUpper()));
            }
            else
            {
                NewspaperImage.sprite = SpriteAtlasHandling.GetSpriteFromAtlas(NewsSpriteAtlas, data.spriteAtlasMilestoneID);
                NewspaperImage.gameObject.SetActive(true);
                NewspaperText.SetText(string.Format(data.description, GameData.Instance.GetPlayerLabel().ToUpper()));
            }

        }

        private void HideDetail()
        {
            if (!m_Occupied) return;
            
            m_Occupied = false;

            TextBox.SetActive(false);

            NewspaperImage.gameObject.SetActive(false);
            NewspaperText.text = "";

            InterruptionManager.Instance.DisableInteruption();
            
            if (lastDetailObj.type == DetailType.MILESTONE)
            {
                InterruptionManager.Instance.EnableInterruption(InterruptionType.OUT);
            }

            MouseManager.Instance.UpdateOcuppiedState(false);

            DetailCanvasUI.SetActive(false);
        }

        public void RequestDetailObjCreation(string objID, Vector3 spawnPosition, Vector3 spawnScale, Vector3 spawnRotation, bool needFocus = true)
        {
            Creator.OnObjectCreated -= NotifyCreation;
            Creator.OnObjectCreated += NotifyCreation;
            
            Creator.CreateDetailObject(objID, spawnPosition, spawnScale, spawnRotation, needFocus);
        }

        private void NotifyCreation()
        {
            Creator.OnObjectCreated -= NotifyCreation;
            OnObjectCreated?.Invoke();
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