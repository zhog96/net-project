using System.Collections;
using UnityEngine;

namespace SmartEl

{
    public class SmartDoor : MonoBehaviour
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
            if (TryGetComponent(out UnityEngine.Animator openandclose))
            {
                this.openandclose = openandclose;
            }
        }

        void Update()
        {
            float dist = Vector3.Distance(Player.position, transform.position);
            if (dist < 3)
            {
                if (open == false)
                {
                    StartCoroutine(opening());
                }
            }
            else
            {
                if (open)
                {
                    StartCoroutine(closing());
                }
            }
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