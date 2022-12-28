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
        public RolesEnum level;

        public Updates(string id, float x, float y, float z, RolesEnum level)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.z = z;
            this.level = level;
        }
    }
}