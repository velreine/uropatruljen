using System;
using CommonData.Model.Action;

namespace CommonData.Logic.Factory
{
    public interface IActionFactory
    {
        TAction CreateAction<TAction>(byte[] rawData) where TAction : IAction;
        IAction CreateAction(byte[] rawData, Type actionType);
        void RegisterActionCreator(Type typeToCreate, Func<byte[], IAction> creatorFunc);
    }
}