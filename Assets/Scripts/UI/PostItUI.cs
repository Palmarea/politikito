using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PostItUI : MonoBehaviour
{
    [SerializeField] private int stepStartIndex;

    public List<Toggle> tutorialStepToggles;
    public List<TMP_Text> tutorialStepLabels;

    public UnityEvent OnSectionCompleted;

    public bool hideOnAwake = false;

    private void Awake()
    {
        Context.TutorialData.OnTutorialStepComplete += HandleOnTutorialStepCompleted;

        if (hideOnAwake)
            gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        Context.TutorialData.OnTutorialStepComplete -= HandleOnTutorialStepCompleted;
    }

    private void HandleOnTutorialStepCompleted(int globalStepIndex)
    {
        int localIndex = globalStepIndex - stepStartIndex;

        if (localIndex < 0 || localIndex >= tutorialStepToggles.Count)
            return;

        // EFECTO DE SONIDO ACA

        tutorialStepToggles[localIndex].isOn = true;
        tutorialStepLabels[localIndex].fontStyle = FontStyles.Strikethrough;

        CheckSectionCompleted();
    }

    private void CheckSectionCompleted()
    {
        for (int i = 0; i < tutorialStepToggles.Count; i++)
        {
            if (!tutorialStepToggles[i].isOn)
                return;
        }

        OnSectionCompleted?.Invoke();
    }
}