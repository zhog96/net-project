using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SmartEl
{

    public class Spawner : MonoBehaviour
    {
        public GameObject currentPlayer;
        private readonly Dictionary<int, GameObject> _anotherPlayers = new();
        private volatile DtoPlayer[] _players = {};
        private int _animIDSpeed = Animator.StringToHash("Speed");

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            // anotherPlayers[0].GetComponent<Animator>().SetFloat(_animIDSpeed, 3f);
        }

        private void FixedUpdate()
        {
            foreach (var player in _players)
            {
                if (!_anotherPlayers.ContainsKey(player.id))
                {
                    var clone = Instantiate(currentPlayer.gameObject,
                        currentPlayer.transform.position,
                        currentPlayer.transform.rotation);
                    clone.tag = "AnotherPlayer";
                    clone.GetComponent<Player>().id = player.id;
                    _anotherPlayers.Add(player.id, clone);
                }
                else
                {
                    _anotherPlayers[player.id].transform.position =
                        new Vector3((float) player.x, (float) player.y, (float) player.z);
                    _anotherPlayers[player.id].transform.rotation = new Quaternion(
                        (float) player.rx,
                        (float) player.ry,
                        (float) player.rz,
                        (float) player.w);
                }
            }
        }

        public void UpdatePlayers(DtoPlayer[] serverPlayers)
        {
            print(serverPlayers.ToList());
            _players = serverPlayers;
        }
    }
}