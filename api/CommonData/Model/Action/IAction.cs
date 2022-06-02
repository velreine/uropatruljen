namespace CommonData.Model.Action
{
    /**
     * An action is an intent or a request for a change.
     */
    public interface IAction
    {
        byte[] ToBytes();
    }
}