using System;

namespace SmartEl.Dtos
{
    [Serializable]
    public class DoorEvent
    {
        public string doorID;
        public string playerId;
        public bool inArea;

        public DoorEvent(string doorID, string playerId, bool inArea)
        {
            this.doorID = doorID;
            this.playerId = playerId;
            this.inArea = inArea;
        }
    }
}