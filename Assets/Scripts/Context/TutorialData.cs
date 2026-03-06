using System;
using System.Collections.Generic;

public class TutorialData
{
    public Action<int> OnTutorialStepComplete;
    
    public List<int> CompletedTutorialSteps { get; private set; } = new ();
    
    private const int TUTORIAL_STEP_COUNT = 4;

    public const int CHARACTER_STEP_INDEX = 0;
    public const int WATERING_CAN_STEP_INDEX = 1;
    public const int COOKIE_STEP_INDEX = 2;
    public const int DUMBBELL_STEP_INDEX = 3;
    
    public void CompleteTutorialStep(int stepIndex)
    {
        if (stepIndex >= TUTORIAL_STEP_COUNT)
            return;

        if (CompletedTutorialSteps.Contains(stepIndex))
            return;
        
        OnTutorialStepComplete?.Invoke(stepIndex);
        CompletedTutorialSteps.Add(stepIndex);
    }
    
    public bool IsTutorialComplete() => CompletedTutorialSteps.Count == TUTORIAL_STEP_COUNT;
}
