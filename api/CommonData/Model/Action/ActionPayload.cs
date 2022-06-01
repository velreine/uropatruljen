using System;
using System.IO;

namespace CommonData.Model.Action
{
    /**
     * This is a wrapper class that wraps an action into a payload.
     * The class provides convenience functions from creating this type to/from bytes.
     */
    public class ActionPayload
    {
        public int ActionIdentifier { get; set; }
        
        public byte[] ActionData { get; set; }
        
        // Ensures the action payload cannot be instantiated manually.
        private ActionPayload() {}
        
        /// <summary>
        /// Given an instantiated Action object, "wraps" that action by instantiating an ActionPayload from it.
        /// </summary>
        /// <param name="action">The instantiated action to create an ActionPayload from.</param>
        /// <returns>The instantiated ActionPayload</returns>
        public static ActionPayload FromAction(IAction action)
        {
            return new ActionPayload
            {
                ActionIdentifier = ActionMap.ActionTypeToActionIdentifier[action.GetType()],
                ActionData = action.ToBytes()
            };
        }

        /// <summary>
        /// Given a payload (an array of bytes) construct the object representation of ActionPayload from that.
        /// </summary>
        /// <param name="payload">The data from which to construct the object</param>
        /// <returns>An instantiated ActionPayload</returns>
        public static ActionPayload FromPayload(byte[] payload)
        {
            // Read the action identifier from the payload.
            var actionIdentifier = BitConverter.ToInt32(payload, 0);
            
            var output = new ActionPayload
            {
                ActionIdentifier = actionIdentifier
            };

            // The length of the action data should be the length minus the ActionIdentifier.
            var actionDataLength = payload.Length - sizeof(int);

            if (actionDataLength <= 0)
            {
                return output;
            }

            // Fill the action with data from the payload.
            var actionDataBuffer = new byte[actionDataLength];
            for (int i = 4; i < payload.Length; i++)
            {
                actionDataBuffer[i - 4] = payload[i];
            }

            output.ActionData = actionDataBuffer;

            return output;
        }
        
        /// <summary>
        /// Given an instance of ActionPayload converts the entire thing to a byte payload.
        /// </summary>
        /// <returns>A byte array that represents the payload.</returns>
        public byte[] ToPayload()
        {
            var buffer = new byte[sizeof(int) + ActionData.Length];
            var ms = new MemoryStream(buffer);
            
            ms.Write(BitConverter.GetBytes(this.ActionIdentifier), 0, sizeof(int));
            ms.Write(ActionData, 0, ActionData.Length);

            return ms.ToArray();
        }

        
        
        
        
        
    }
}