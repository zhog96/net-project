using System;
using JetBrains.Annotations;

namespace SmartEl
{
    [Serializable]
    public class DtoPlayer
    {
        public DtoPlayer(int id, double x, double y, double z, double rotation)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.z = z;
            this.rotation = rotation;
        }

        public int id;
        public double x; 
        public double y;
        public double z;
        public double rotation;
    }
}