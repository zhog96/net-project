using System;

namespace SmartEl.Dtos
{
    [Serializable]
    public class Message <T>
    {
        public string key;
        public T payload;

        public Message(string key, T payload)
        {
            this.key = key;
            this.payload = payload;
        }
    }
}