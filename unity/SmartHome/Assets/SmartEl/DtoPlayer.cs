using System;
using JetBrains.Annotations;

namespace SmartEl
{
    public class DtoPlayer
    {
        public int id { get; set; }
        [CanBeNull]
        public Double x { get; set; }
        [CanBeNull]
        public Double y { get; set; }
        [CanBeNull]
        public Double z { get; set; }
        [CanBeNull]
        public Double rotation { get; set; }
    }
}