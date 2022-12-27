using System;

namespace SmartEl.Dtos
{
    [Serializable]
    public class DoorEvent
    {
        public string doorID;
        public string playerId;
        public bool needOpen;

        public DoorEvent(string doorID, string playerId, bool needOpen)
        {
            this.doorID = doorID;
            this.playerId = playerId;
            this.needOpen = needOpen;
        }
    }
}