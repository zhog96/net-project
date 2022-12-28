using System;
using System.Collections;
using UnityEngine;

namespace SmartEl
{
    public class SmartLight : MonoBehaviour
    {
        public string Id;
        private Light Light;
        private bool IsLight;
        public Transform Player;
        public GameObject WsClientObject;
        public Client WsClientScript;
        // Start is called before the first frame update
        void Start()
        {
            GameObject[] gos = null;
            while (gos == null || gos.Length == 0)
            {
                gos = GameObject.FindGameObjectsWithTag("Player"); 
            }
            
            Player = gos[0].transform;
            if (TryGetComponent(out UnityEngine.Light light))
            {
                IsLight = true;
                Light = light;
                Light.enabled = false;
            }

            WsClientObject = GameObject.Find("Client");
            WsClientScript = WsClientObject.GetComponent<Client>();
        }

        // Update is called once per frame
        void Update()
        {
            float dist = Vector3.Distance(Player.position, transform.position);
            if (IsLight)
            {
                if (dist < 8)
                {
                    if (Light.enabled == false)
                    {
                        WsClientScript.SendLightEvent(Id, true);
                    }
                }
                else
                {
                    if (Light.enabled == true)
                    {
                        WsClientScript.SendLightEvent(Id, false);
                    }
                }
            }
        }
        
        public IEnumerator TurnOnLight()
        {
            print("you are turn on light");
            Light.enabled = true;
            yield return new WaitForSeconds(.5f);
        }
        
        public IEnumerator TurnOffLight()
        {
            print("you are turn on light");
            Light.enabled = false;
            yield return new WaitForSeconds(.5f);
        }
        
    }
}
