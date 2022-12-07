using System.Collections;
using UnityEngine;

namespace SojaExiles

{
    public class opencloseDoor : MonoBehaviour
    {
        public Animator openandclose;
        public bool open;
        public Transform Player;

        void Start()
        {
            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("Player");
            Player = gos[0].transform;
            open = false;
        }

        void Update()
        {
            // float dist = Vector3.Distance(Player.position, transform.position);
            // if (dist < 5)
            // {
            //     if (open == false)
            //     {
            //         StartCoroutine(opening());
            //     }
            // }
            // else
            // {
            //     if (open)
            //     {
            //         StartCoroutine(closing());
            //     }
            // }
        }

        IEnumerator opening()
        {
            print("you are opening the door");
            openandclose.Play("Opening");
            open = true;
            yield return new WaitForSeconds(.5f);
        }

        IEnumerator closing()
        {
            print("you are closing the door");
            openandclose.Play("Closing");
            open = false;
            yield return new WaitForSeconds(.5f);
        }
    }
}