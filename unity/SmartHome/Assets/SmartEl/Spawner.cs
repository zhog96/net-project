using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SmartEl;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject currentPlayer;
    public Dictionary<int, GameObject> anotherPlayers = new();
    public ConcurrentDictionary<int, DtoPlayer> DtoPlayers = new();
    public ConcurrentQueue<int> ids = new();
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
        foreach (var id in ids)
        {
            if (!anotherPlayers.ContainsKey(id))
            {
                GameObject clone = Instantiate(currentPlayer.gameObject,
                    currentPlayer.transform.position,
                    currentPlayer.transform.rotation);
                clone.tag = "AnotherPlayer";
                clone.GetComponent<Player>().id = id;
                anotherPlayers.Add(id, clone);
            }
            else
            {
                DtoPlayer dtoPlayer = DtoPlayers[id];
                anotherPlayers[id].transform.position = new Vector3((float) dtoPlayer.x, (float) dtoPlayer.y, (float) dtoPlayer.z);
                anotherPlayers[id].transform.rotation = new Quaternion(0, (float) dtoPlayer.rotation, 0, anotherPlayers[id].transform.rotation.w);
            }
        }
    }

    public void AddPlayer(int id)
    {
        ids.Enqueue(id);
    }

    public void UpdatePlayers(DtoPlayer dtoPlayer)
    {
        if (DtoPlayers.ContainsKey(dtoPlayer.id))
        {
            DtoPlayers.TryUpdate(dtoPlayer.id, dtoPlayer, DtoPlayers[dtoPlayer.id]);
        }
        else
        {
            DtoPlayers.TryAdd(dtoPlayer.id, dtoPlayer);
            AddPlayer(dtoPlayer.id);
        }
    }
}