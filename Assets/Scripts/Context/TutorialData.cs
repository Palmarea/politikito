public class TutorialData
{
    public int CurrentTutorialStep { get; private set; } = 0;
    
    private const int TUTORIAL_STEP_COUNT = 4;
    
    public void CompleteTutorialStep()
    {
        if (CurrentTutorialStep >= TUTORIAL_STEP_COUNT)
            return;
        
        CurrentTutorialStep++;
    }
    
    public bool IsTutorialComplete() => CurrentTutorialStep == TUTORIAL_STEP_COUNT;
}
