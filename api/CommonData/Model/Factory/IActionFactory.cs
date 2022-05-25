using System;
using CommonData.Model.Action;

namespace CommonData.Model.Factory
{
    public interface IActionFactory
    {
        TAction CreateAction<TAction>(byte[] rawData) where TAction : IAction;
        void RegisterActionCreator(Type typeToCreate, Func<byte[], IAction> creatorFunc);
    }
}