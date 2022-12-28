using System;

namespace SmartEl.Dtos
{
    [Serializable]
    public class DtoLight
    {
        public string lightID;
        public bool enable;

        public DtoLight(string lightID, bool enable)
        {
            this.lightID = lightID;
            this.enable = enable;
        }
    }
}