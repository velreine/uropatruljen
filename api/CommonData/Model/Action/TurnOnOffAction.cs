using System;
using System.IO;

namespace CommonData.Model.Action
{
    public class TurnOnOffAction : AbstractAction
    {
        public bool TurnOn { get; set; }
        
        public override byte[] ToBytes()
        {
            // Int32 ComponentIdentifier, bool TurnOn.
            const int payloadSize = sizeof(int) + sizeof(bool);
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