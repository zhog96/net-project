using System;
using UnityEngine;

namespace SmartEl
{
    public class MyCursorController : MonoBehaviour
    {
        public static MyCursorController Instance;
        // Start is called before the first frame update

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
