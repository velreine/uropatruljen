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
            this.RegisterActionCreator(typeof(TurnOnOffAction), rawData =>
            {
                return new TurnOnOffAction()
                {
                    ComponentIdentifier = BitConverter.ToInt32(rawData, 0),
                    TurnOn = BitConverter.ToBoolean(rawData, 4),
                };
            });
            
            // Register SetColorAction
            this.RegisterActionCreator(typeof(SetColorAction), rawData =>
            {
                // Byte layout.
                //   0    1    2    3    4    5    6    7    8    9
                // i32, i32, i32, i32, ui16, ui16, ui16, ui16, ui16, ui16
                return new SetColorAction()
                {
                    ComponentIdentifier = BitConverter.ToInt32(rawData, 0),
                    RValue = BitConverter.ToUInt16(rawData, 4),
                    GValue = BitConverter.ToUInt16(rawData, 6),
                    BValue = BitConverter.ToUInt16(rawData, 8),
                };
            });
            
        }
    }
}