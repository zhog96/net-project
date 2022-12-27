using System;
using System.Collections;
using UnityEngine;

namespace SmartEl

{
    public class SmartDoor : MonoBehaviour
    {
        public string Id;
        public Animator openandclose;
        public bool open;
        public Transform Player;
        public GameObject WsClientObject;
        public Client WsClientScript;
        void Start()
        {
            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("Player");
            WsClientObject = GameObject.Find("Client");
            WsClientScript = WsClientObject.GetComponent<Client>();
            Player = gos[0].transform;
            open = false;
            if (TryGetComponent(out UnityEngine.Animator openandclose))
            {
                this.openandclose = openandclose;
            }
        }

        void FixedUpdate()
        {
            float dist = Vector3.Distance(Player.position, transform.position);
            if (dist < 3)
            {
                if (open == false)
                {
                    WsClientScript.SendDoorEvent(Id, true);
                }
            }
            else
            {
                if (open)
                {
                    WsClientScript.SendDoorEvent(Id, false);
                }
            }
        }

        public IEnumerator opening()
        {
            print("opening the door: " + Id);
            openandclose.Play("Opening");
            open = true;
            yield return new WaitForSeconds(.5f);
        }

        public IEnumerator closing()
        {
            print("closing the door: " + Id);
            openandclose.Play("Closing");
            open = false;
            yield return new WaitForSeconds(.5f);
        }
    }
}