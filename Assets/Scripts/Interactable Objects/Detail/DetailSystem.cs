using Febucci.UI;
using Game.Managers.Mouse;
using Game.Managers.Timing;
using Game.Systems.Input;
using System.Collections;
using UnityEngine;

namespace Game.Systems.Interaction.Detail
{
    public class DetailSystem : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private DetailCreator Creator;
        
        [Header("References")]
        [SerializeField] private GameObject DetailCanvasUI;
        [SerializeField] private TypewriterByCharacter DetailText;

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
        }

        public void ShowDetail(string detailMs)
        {
            if (!DetailCanvasUI.activeInHierarchy) DetailCanvasUI.SetActive(true);

            m_Occupied = true;

            InterruptionManager.Instance.EnableInterruption(InterruptionType.NOTIFICATION);

            DetailText.ShowText(string.Format(detailMs, GameData.Instance.GetPlayerLabel().ToUpper()));
        }

        private void HideDetail()
        {
            if (!m_Occupied) return;
            
            m_Occupied = true;

            DetailText.ShowText("");

            InterruptionManager.Instance.DisableInteruption();
            MouseManager.Instance.UpdateOcuppiedState(false);

            DetailCanvasUI.SetActive(false);
        }

        public void RequestDetailObjCreation(string objID)
        {
            Creator.CreateDetailObject(objID);
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