using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace CommonData.Model.Action
{
    public class TurnOnOffAction : AbstractAction
    {
        public bool TurnOn { get; set; }
        
        public override byte[] ToPayload()
        {
            // Int32 ComponentIdentifier, bool TurnOn.
            var payloadSize = sizeof(int) + sizeof(bool);
            var buffer = new byte[payloadSize];

            var ms = new MemoryStream(buffer);

            var baComponentIdentifier = BitConverter.GetBytes(this.ComponentIdentifier);
            var baTurnOn = BitConverter.GetBytes(this.TurnOn);
            
            ms.Write(baComponentIdentifier, 0, sizeof(int));
            ms.Write(baTurnOn, 0, sizeof(bool));

            return ms.ToArray();
        }
    }
}