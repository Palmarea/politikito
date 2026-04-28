using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    public string PlayerName;
    private const string baseName = "Tico";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public string GetPlayerName()
    {
        return PlayerName != "" ? PlayerName.ToUpper() : baseName.ToUpper();
    }
}