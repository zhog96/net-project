﻿using System;
using JetBrains.Annotations;

namespace SmartEl
{
    [Serializable]
    public class DtoPlayer
    {
        public DtoPlayer(string id, string uid, double x, double y, double z, double rx, double ry, double rz, double w)
        {
            this.id = id;
            this.uid = uid;
            this.x = x;
            this.y = y;
            this.z = z;
            this.rx = rx;
            this.ry = ry;
            this.rz = rz;
            this.w = w;
        }

        public string id;
        public string uid;
        public double x; 
        public double y;
        public double z;
        public double rx; 
        public double ry;
        public double rz;
        public double w;
    }
}