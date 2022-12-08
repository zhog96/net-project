using System;
using UnityEngine.Serialization;

namespace SmartEl
{
    [Serializable]
    public class HelloMessage
    {
        public HelloMessage(string name)
        {
            this.name = name;
        }
        
        public string name;
    }
}