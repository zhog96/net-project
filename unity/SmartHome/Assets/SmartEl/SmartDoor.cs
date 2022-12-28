using System;
using System.Collections;
using UnityEngine;

namespace SmartEl

{
    public class SmartDoor : MonoBehaviour
    {
        public string Id;
        public Animator openandclose;
        public bool forAll;
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
        }

        public void opening()
        {
            print("opening the door: " + Id);
            openandclose.Play("Opening");
            open = true;
        }

        public void closing()
        {
            print("closing the door: " + Id);
            openandclose.Play("Closing");
            open = false;
        }
    }
}