using System;

namespace SmartEl.Dtos
{
    [Serializable]
    public class DtoDoor
    {
        public string doorID;
        public bool isOpen;

        public DtoDoor(string doorID, bool isOpen)
        {
            this.doorID = doorID;
            this.isOpen = isOpen;
        }
    }
}