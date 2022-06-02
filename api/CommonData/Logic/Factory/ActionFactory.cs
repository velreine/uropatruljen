using System;
using System.Collections.Generic;
using CommonData.Model.Action;

namespace CommonData.Logic.Factory
{
    /**
     * This class represents an Action factory where it is possible to register ActionCreators.
     * Which is essentially just functions that take a raw byte array, and promises to return a derived type of IAction.
     * This factory serves as a central hub for all those creator functions, and enables generic creation at runtime.
     *
     * The factory throws if multiple creator functions are trying to be registered to the same type.
     * The factory also throws if an action type is attempted to be constructed, for which there exists no creator function.
     */
    public class ActionFactory : IActionFactory
    {
        private readonly Dictionary<Type, Func<byte[], IAction>> _actionCreators = new Dictionary<Type, Func<byte[], IAction>>();
        
        public TAction CreateAction<TAction>(byte[] rawData) where TAction : IAction
        {

            if (_actionCreators.TryGetValue(typeof(TAction), out var creator))
            {
                return (TAction)creator.Invoke(rawData);
            }

            throw new Exception(
                $"The factory could not find a suitable creator for the action of type {typeof(TAction)}");
        }

        public IAction CreateAction(byte[] rawData, Type actionType)
        {
            if (_actionCreators.TryGetValue(actionType, out var creator))
            {
                return creator.Invoke(rawData);
            }
            
            throw new Exception(
                $"The factory could not find a suitable creator for the action of type {actionType}");
        }

        public void RegisterActionCreator(Type typeToCreate, Func<byte[], IAction> creatorFunc)
        {

            if (_actionCreators.ContainsKey(typeToCreate))
            {
                throw new Exception(
                    $"The factory already contains an ActionCreator for the type: {typeToCreate}");
            }
            
            _actionCreators.Add(typeToCreate, creatorFunc);
        }
    }
}