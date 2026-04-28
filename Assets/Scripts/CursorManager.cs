using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
    void Start()
    {
        Vector2 hotspot = new Vector2(15, 8);
        Cursor.SetCursor(cursorTexture, hotspot, CursorMode.ForceSoftware);
    }
}
