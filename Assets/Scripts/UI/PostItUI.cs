using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostItUI : MonoBehaviour
{
    public List<Toggle> tutorialStepToggles;
    public List<TMP_Text> tutorialStepLabels;

    public void Awake()
    {
        Context.TutorialData.OnTutorialStepComplete += HandleOnTutorialStepCompleted;
    }

    private void HandleOnTutorialStepCompleted(int stepIndex)
    {
        if (Context.TutorialData.IsTutorialComplete())
            return;
        
        // EFECTO DE SONIDO ACAA
        tutorialStepToggles[stepIndex].isOn = true;
        tutorialStepLabels[stepIndex].fontStyle = FontStyles.Strikethrough;
    }
}
