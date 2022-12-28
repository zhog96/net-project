using System;

namespace SmartEl.Dtos
{
    [Serializable]
    public class DoorUpdates
    {
        public string doorID;
        public float x;
        public float y;
        public float z;
        public bool open;

        public DoorUpdates(string doorID, float x, float y, float z, bool open)
        {
            this.doorID = doorID;
            this.x = x;
            this.y = y;
            this.z = z;
            this.open = open;
        }
    }
}