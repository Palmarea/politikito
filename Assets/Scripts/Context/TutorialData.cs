using System;
using System.Collections.Generic;
using System.Linq;

public class TutorialData
{
    public Action<int> OnTutorialStepComplete;
    
    public List<int> CompletedTutorialSteps { get; private set; } = new ();
        
    private const int TUTORIAL_STEP_COUNT = 7;

    public const int CHARACTER_STEP_INDEX = 0;
    public const int EXPLORE_ROOM_INDEX = 1;
    public const int WATERING_CAN_STEP_INDEX = 2;
    public const int COOKIE_STEP_INDEX = 3;
    public const int DUMBBELL_STEP_INDEX = 4;
    public const int OBJECT_INTERACT_STEP_INDEX = 5;
    public const int NEWS_INTERACT_STEP_INDEX = 6;

    private readonly int[] _section1Steps =
    {
        CHARACTER_STEP_INDEX,
        EXPLORE_ROOM_INDEX,
        WATERING_CAN_STEP_INDEX,
        COOKIE_STEP_INDEX,
        DUMBBELL_STEP_INDEX
    };

    private readonly int[] _section2Steps =
    {
        OBJECT_INTERACT_STEP_INDEX,
        NEWS_INTERACT_STEP_INDEX
    };

    public int CurrentSectionIndex
    {
        get
        {
            bool section1Complete = _section1Steps.All(IsTutorialStepCompleted);

            return section1Complete ? 1 : 0;
        }
    }

    public void CompleteTutorialStep(int stepIndex)
    {
        if (stepIndex < 0 || stepIndex >= TUTORIAL_STEP_COUNT)
            return;

        if (CompletedTutorialSteps.Contains(stepIndex))
            return;

        if (!CanCompleteStep(stepIndex))
            return;

        CompletedTutorialSteps.Add(stepIndex);
        OnTutorialStepComplete?.Invoke(stepIndex);
    }

    private bool CanCompleteStep(int stepIndex)
    {
        if (CurrentSectionIndex == 0)
        {
            return Array.Exists(_section1Steps, step => step == stepIndex);
        }

        if (CurrentSectionIndex == 1)
        {
            return Array.Exists(_section2Steps, step => step == stepIndex);
        }

        return false;
    }

    public bool IsTutorialComplete() => CompletedTutorialSteps.Count == TUTORIAL_STEP_COUNT;

    public bool IsTutorialStepCompleted(int stepIndex)
    {
        if (stepIndex < 0 || stepIndex >= TUTORIAL_STEP_COUNT)
            return false;

        return CompletedTutorialSteps.Contains(stepIndex);
    }

    public bool IsCurrentSectionComplete()
    {
        if (CurrentSectionIndex == 0)
            return _section1Steps.All(IsTutorialStepCompleted);

        return _section2Steps.All(IsTutorialStepCompleted);
    }
}
