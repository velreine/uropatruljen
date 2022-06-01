using System;
using System.IO;

namespace CommonData.Model.Action
{
    public class SetColorAction : AbstractAction
    {
        public byte RValue { get; set; }
        public byte GValue { get; set; }
        public byte BValue { get; set; }
        
        
        public override byte[] ToBytes()
        {
            // Int32 ComponentIdentifier, byte r, g and b.
            const int payloadSize = sizeof(int) + (3 * sizeof(byte));
            var buffer = new byte[payloadSize];

            var ms = new MemoryStream(buffer);
            
            var baComponentIdentifier = BitConverter.GetBytes(this.ComponentIdentifier);
            var baR = BitConverter.GetBytes(this.RValue);
            var baG = BitConverter.GetBytes(this.GValue);
            var baB = BitConverter.GetBytes(this.BValue);
            
            ms.Write(baComponentIdentifier, 0, sizeof(int));
            ms.Write(baR, 0, sizeof(byte));
            ms.Write(baG, 0, sizeof(byte));
            ms.Write(baB, 0, sizeof(byte));

            return ms.ToArray();
        }
    }
}