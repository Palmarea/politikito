using System.Linq;
using UnityEngine;
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

        [Header("Cursor Configuration")]
        [SerializeField] private Image CursorTexture;
        [SerializeField] private CursorObject[] CursorReferences = new CursorObject[4];

        private readonly Vector2 m_Hotspt = new(15, 8);

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
            SetCursorState(CursorStateType.DEFAULT);
        }

        public void SetCursorState(CursorStateType TYPE)
        {
            CursorTexture.sprite = GetCursorOfType(TYPE).Sprite;
        }

        private CursorObject GetCursorOfType(CursorStateType TYPE) => CursorReferences.First(a => a.Type == TYPE);
    }
}
