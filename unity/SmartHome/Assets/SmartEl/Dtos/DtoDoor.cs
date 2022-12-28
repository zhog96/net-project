using System;

namespace SmartEl.Dtos
{
    [Serializable]
    public class DtoDoor
    {
        public string doorID;
        public bool open;

        public DtoDoor(string doorID, bool open)
        {
            this.doorID = doorID;
            this.open = open;
        }
    }
}