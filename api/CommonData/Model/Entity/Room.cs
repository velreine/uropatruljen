using System.Collections.Generic;
using CommonData.Model.Entity.Contracts;

namespace CommonData.Model.Entity {

   
    public class Room : AbstractEntity
    {
        public string Name { get; set; }

        public Home Home { get; set; }

        public ICollection<Device> Devices { get; set; }
       
    }

}