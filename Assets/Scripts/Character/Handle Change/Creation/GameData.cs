using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    public string PlayerName;
    private const string baseName = "Tico";

    public enum Language
    {
        SPANISH,
        ENGLISH
    }

    public Language GameLanguage;

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

    public Language GetCurrentLanguage() => GameLanguage;
}