namespace CommonData.Model.Action
{
    public abstract class AbstractAction : IAction
    {
        public int ComponentIdentifier { get; set; }
        
        public abstract byte[] ToPayload();
    }
}