
namespace Game.Managers.Timing
{
    public interface IInterruptible
    {
        void HandleInterruptionStart(InterruptionType type);
        
        void HandleInterruptionEnd();
    }
}