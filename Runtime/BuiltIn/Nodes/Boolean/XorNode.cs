namespace Physalia.Flexi
{
    [NodeCategory("Built-in/Logical")]
    public class XorNode : ValueNode
    {
        public Inport<bool> a;
        public Inport<bool> b;
        public Outport<bool> result;

        protected override void EvaluateSelf()
        {
            result.SetValue(a.GetValue() ^ b.GetValue());
        }
    }
}
