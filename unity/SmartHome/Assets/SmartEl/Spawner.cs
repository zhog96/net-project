using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using StarterAssets;
using UnityEngine;

namespace SmartEl
{

    public class Spawner : MonoBehaviour
    {
        private class Entity
        {
            public GameObject player;
            public bool moving;
            public string uid;

            public Entity(GameObject player, bool moving, string uid)
            {
                this.player = player;
                this.moving = moving;
                this.uid = uid;
            }
        }

        public GameObject currentPlayer;
        private readonly Dictionary<string, Entity> _anotherPlayers = new();
        private volatile DtoPlayer[] _players = {};
        private int _animIDSpeed = Animator.StringToHash("Speed");

        // Start is called before the first frame update
        void Start()
        {
        }

        private void FixedUpdate()
        {
            var toDelete = _anotherPlayers.Keys.Where(playerKey =>
            {
                var flag = false;
                foreach (var player in _players)
                {
                    if (player.id.Equals(playerKey))
                    {
                        flag = true;
                    }
                }
                return !flag;
            });
            foreach (var playerKey in toDelete)
            {
                var playerObject = _anotherPlayers[playerKey].player;
                _anotherPlayers.Remove(playerKey);
                Destroy(playerObject);
            }
            
            foreach (var player in _players)
            {
                if (!_anotherPlayers.ContainsKey(player.id))
                {
                    var clone = Instantiate(currentPlayer.gameObject,
                        currentPlayer.transform.position,
                        currentPlayer.transform.rotation);
                    clone.tag = "AnotherPlayer";
                    currentPlayer.gameObject.GetComponent<ThirdPersonController>();
                    _anotherPlayers.Add(player.id, new Entity(clone, false, Guid.NewGuid().ToString()));
                }
                else
                {
                    var pos = new Vector3((float) player.x, (float) player.y, (float) player.z);
                    var anotherPlayer = _anotherPlayers[player.id];
                    if (player.uid != anotherPlayer.uid)
                    {
                        anotherPlayer.moving = (pos - anotherPlayer.player.transform.position).sqrMagnitude > 1e-3;
                    }
                    anotherPlayer.player.transform.position = pos;
                    anotherPlayer.player.transform.rotation = new Quaternion(
                        (float) player.rx,
                        (float) player.ry,
                        (float) player.rz,
                        (float) player.w);
                    anotherPlayer.uid = player.uid;
                }
            }
            foreach (var entity in _anotherPlayers.Values)
            {
                print(entity.moving);
                if (entity.moving)
                {
                    entity.player.GetComponent<Animator>().SetFloat(_animIDSpeed, 3f);
                }
                else
                {
                    entity.player.GetComponent<Animator>().SetFloat(_animIDSpeed, 0f);
                }
            }
        }

        public void UpdatePlayers(DtoPlayer[] serverPlayers)
        {
            _players = serverPlayers;
        }
    }
}