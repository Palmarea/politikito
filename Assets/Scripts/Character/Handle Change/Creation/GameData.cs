using Gaskellgames;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    public string PlayerName;
    private const string baseName = "Tico";

    public enum Language
    {
        ENGLISH,
        SPANISH
    }

    public Language GameLanguage = Language.ENGLISH;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        ChangeLanguage(GameLanguage);
        DontDestroyOnLoad(gameObject);
    }

    public string GetPlayerName()
    {
        return PlayerName != "" ? PlayerName.ToUpper() : baseName.ToUpper();
    }

    public Language GetCurrentLanguage() => GameLanguage;

    public void ChangeLanguage(Language newLanguage)
    {
        GameLanguage = newLanguage;
        
        StartCoroutine(SetLocale(GameLanguage.ToInt()));
    }

    public void Change(int index)
    {
        StartCoroutine(SetLocale(index));

    }

    private IEnumerator SetLocale(int _localeID)
    {
        yield return LocalizationSettings.InitializationOperation;

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
    }
}