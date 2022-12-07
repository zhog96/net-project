using System;
using System.Collections;
using UnityEngine;

namespace SmartEl
{
    public class SmartLight : MonoBehaviour
    {
        private Light Light;
        private MeshRenderer MeshRenderer;
        private bool IsLight;
        public Transform Player;
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
                Light.enabled = true;
            }

            if (TryGetComponent(out UnityEngine.MeshRenderer meshRenderer))
            {
                IsLight = false;
                MeshRenderer = meshRenderer;
                MeshRenderer.enabled = true;
            }
            
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
                        StartCoroutine(TurnOnLight());
                    }
                }
                else
                {
                    if (Light.enabled == true)
                    {
                        StartCoroutine(TurnOffLight());
                    }
                }
            }
            else
            {
                if (dist < 10)
                {
                    if (MeshRenderer.enabled == false)
                    {
                        StartCoroutine(TurnOnMesh());
                    }
                }
                else
                {
                    if (MeshRenderer.enabled == true)
                    {
                        StartCoroutine(TurnOffMesh());
                    }
                }
            }
        }
        
        IEnumerator TurnOnLight()
        {
            print("you are turn on light");
            Light.enabled = true;
            yield return new WaitForSeconds(.5f);
        }
        
        IEnumerator TurnOffLight()
        {
            print("you are turn on light");
            Light.enabled = false;
            yield return new WaitForSeconds(.5f);
        }
        
        IEnumerator TurnOnMesh()
        {
            MeshRenderer.enabled = true;
            yield return new WaitForSeconds(.5f);
        }
        
        IEnumerator TurnOffMesh()
        {
            MeshRenderer.enabled = false;
            yield return new WaitForSeconds(.5f);
        }
    }
}
