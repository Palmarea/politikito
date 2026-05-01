using Game.UI;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Managers.Mouse
{
    public enum CursorStateType
    {
        DEFAULT,
        INTEREST,
        GRABABLE,
        HOLD
    }

    [System.Serializable]
    public class CursorObject
    {
        public CursorStateType Type;
        public Sprite Sprite;
    }
    
    public class CursorManager : MonoBehaviour
    {
        public static CursorManager Instance;

        [Header("Dependencies")]
        [SerializeField] private CursorUI CursorUI;

        [Header("Cursor Configuration")]
        [SerializeField] private Image CursorTexture;
        [SerializeField] private CursorObject[] CursorReferences = new CursorObject[4];

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
                DontDestroyOnLoad(this.gameObject);
            }
        }
        #endregion
    
        void Start()
        {
            Cursor.visible = false;
            SceneManager.sceneLoaded += OnSceneChanged;
        }

        public void SetCursorState(CursorStateType TYPE)
        {
            CursorTexture.sprite = GetCursorOfType(TYPE).Sprite;
        }

        public void SetCursorVisibility(bool visible)
        {
            CursorUI.UpdateCursorVisibility(visible);
        }

        public void SetCursorConstrainedAxis(bool constrained)
        {
            CursorUI.UpdateCursorMoveAxis(constrained);
        }

        private CursorObject GetCursorOfType(CursorStateType TYPE) => CursorReferences.First(a => a.Type == TYPE);

        private void OnSceneChanged(Scene scene, LoadSceneMode mode)
        {
            SetCursorState(CursorStateType.DEFAULT);
        }
    }
}
