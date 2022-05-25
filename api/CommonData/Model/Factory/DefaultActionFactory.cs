using System;
using CommonData.Model.Action;

namespace CommonData.Model.Factory
{
    /**
     * This factory registers default implementors for Actions that we as the developers created ourselves.
     */
    public class DefaultActionFactory : ActionFactory
    {
        // Create default creators.
        public DefaultActionFactory()
        {
            
            // Register TurnOnOffAction
            this.RegisterActionCreator(typeof(TurnOnOffAction), rawData =>
            {
                var output = new TurnOnOffAction();
            
                return new TurnOnOffAction()
                {
                    ComponentIdentifier = BitConverter.ToInt32(rawData, 0),
                    TurnOn = BitConverter.ToBoolean(rawData, 4),
                };
            });
            
        }
    }
}