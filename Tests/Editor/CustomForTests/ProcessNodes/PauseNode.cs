namespace Physalia.Flexi.Tests
{
    [NodeCategory("Built-in/[Test Custom]")]
    public class PauseNode : ProcessNode
    {
        protected override AbilityState DoLogic()
        {
            return AbilityState.PAUSE;
        }

        public override bool CheckNodeContext(IResumeContext resumeContext)
        {
            return true;
        }

        protected override AbilityState ResumeLogic(IResumeContext resumeContext)
        {
            return AbilityState.RUNNING;
        }
    }
}
