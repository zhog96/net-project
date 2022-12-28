using System;

namespace SmartEl.Dtos
{
    [Serializable]
    public class Updates
    {
        public string id;
        public float x;
        public float y;
        public float z;
        public bool open;

        public Updates(string id, float x, float y, float z, bool open)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.z = z;
            this.open = open;
        }
    }
}