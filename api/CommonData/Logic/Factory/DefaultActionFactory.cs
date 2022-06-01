using System;
using CommonData.Model.Action;

namespace CommonData.Logic.Factory
{
    /**
     * This factory registers default ActionCreator functions for Actions that we as the developers created ourselves.
     */
    public class DefaultActionFactory : ActionFactory
    {
        // Create default creators.
        public DefaultActionFactory()
        {
            
            // Register TurnOnOffAction
            this.RegisterActionCreator(typeof(TurnOnOffAction), rawData => new TurnOnOffAction()
            {
                // byte layout
                // 0,1,2,3 => COMPONENT_IDENTIFIER
                // 4 => ON/OFF
                ComponentIdentifier = BitConverter.ToInt32(rawData, 0),
                TurnOn = BitConverter.ToBoolean(rawData, 4),
            });
            
            // Register SetColorAction
            this.RegisterActionCreator(typeof(SetColorAction), rawData => new SetColorAction()
            {
                // byte layout
                // 0,1,2,3 => COMPONENT_IDENTIFIER
                // 4 => R
                // 5 => G
                // 6 => B
                ComponentIdentifier = BitConverter.ToInt32(rawData, 0),
                RValue = rawData[4],
                GValue = rawData[5],
                BValue = rawData[6],
            });
            
        }
    }
}