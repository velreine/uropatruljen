using System;
using System.IO;

namespace CommonData.Model.Action
{
    public class SetColorAction : AbstractAction
    {
        public ushort RValue { get; set; }
        public ushort GValue { get; set; }
        public ushort BValue { get; set; }
        
        
        public override byte[] ToPayload()
        {
            // Int32 ComponentIdentifier, UInt16 r, g and b.
            var payloadSize = sizeof(int) + (3 * sizeof(UInt16));
            var buffer = new byte[payloadSize];

            var ms = new MemoryStream(buffer);

            var baComponentIdentifier = BitConverter.GetBytes(this.ComponentIdentifier);
            var baR = BitConverter.GetBytes(this.RValue);
            var baG = BitConverter.GetBytes(this.GValue);
            var baB = BitConverter.GetBytes(this.BValue);
            
            ms.Write(baComponentIdentifier, 0, sizeof(int));
            ms.Write(baR, 0, sizeof(ushort));
            ms.Write(baG, 0, sizeof(ushort));
            ms.Write(baB, 0, sizeof(ushort));

            return ms.ToArray();
        }
    }
}