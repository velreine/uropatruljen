namespace CommonData.Model.Action
{
    public interface IAction<out T> where T : IAction<T>
    {
        byte[] ToPayload();
        T FromPayload(byte[] rawData);
    }
}