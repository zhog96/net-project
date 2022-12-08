using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using Object = System.Object;
using Random = UnityEngine.Random;

namespace SmartEl
{
    public class Client : MonoBehaviour
    {
        public Button Button;
        // public Text host;
        public GameObject Spawner;
        public Object spawnerComponentLock = new object();
        public Spawner spawnerComponent;
        public Text host;
        WebSocket ws;
        public int id;

        public void Start()
        {
            Screen.SetResolution(960, 540, false, 60);
            spawnerComponent = Spawner.GetComponent<Spawner>();
            Button.onClick.AddListener(Subscribe);
        }
        
        private void Subscribe()
        {
            var clientId = Guid.NewGuid().ToString();
            // ws = new WebSocket("ws://"+ host.text +":8080/gs-guide-websocket");
            ws = new WebSocket("ws://"+ "127.0.0.1" +":8080/gs-guide-websocket");
            ws.OnOpen += (sender, e) =>
            {
                var serializer = new StompMessageSerializer();
                var connect = new StompMessage("CONNECT");
                connect["accept-version"] = "1.1";
                connect["heart-beat"] = "10000,10000";
                ws.Send(serializer.Serialize(connect));
                
                var sub = new StompMessage("SUBSCRIBE");
                sub["id"] = "sub-" + clientId;
                sub["destination"] = "/topic/id";
                ws.Send(serializer.Serialize(sub));
                 
                sub["destination"] = "/topic/players";
                ws.Send(serializer.Serialize(sub));
                Thread.Sleep(1000);
                _auth();
            };
            ws.OnMessage += (sender, e) =>
            {
                // print("Spring now says: " + e.Data);
                var body = e.Data.Substring(e.Data.IndexOf("\n\n") + 2);
                if (e.Data.Contains("/topic/id"))
                {
                    id = int.Parse(body);
                }
                if (e.Data.Contains("/topic/players"))
                {
                    print(body);
                    var players = JsonUtility.FromJson<DtoPlayers>(body).players;
                    lock (spawnerComponentLock)
                    {
                        foreach(var player in players)
                        {
                            if (player.id != id)
                            {
                                spawnerComponent.UpdatePlayers(player);
                            }
                        }
                    }
                }
            };
            ws.OnError += (sender, e) =>
                print("Error: " + e.Exception);
            ws.Connect();
        }

        public void FixedUpdate()
        {
            // 1. Слушать сервер
            //   1.1. Запрос на добавление другого игрока
            //   1.2. Смотреть передвижения других игроков
            //
            //
            // 1. Отправлять на сервер
            //   1.1. Запрос на исключение себя из списка других игроков
            //   1.2. Отправлять свои координаты
            if (ws != null)
            {
                var playerController = spawnerComponent.currentPlayer.GetComponent<Player>();
                playerController.SetId(id);
                NoParamaterOnclick();
            }
        }

        private void _auth()
        {
            var connect = new StompMessage("SEND", JsonUtility.ToJson(new HelloMessage("Bobby!")));
            connect["destination"] = "/app/register";
            var serializer = new StompMessageSerializer();
            ws.Send(serializer.Serialize(connect));
        }
        
        private void _getPlayers()
        {
            var connect = new StompMessage("SEND", JsonUtility.ToJson(new HelloMessage("Bobby!")));
            connect["destination"] = "/app/getPlayers";
            var serializer = new StompMessageSerializer();
            ws.Send(serializer.Serialize(connect));
        }
        private void NoParamaterOnclick()
        {
            var currentPlayer = spawnerComponent.currentPlayer;
            var position = currentPlayer.transform.position;
            var id = currentPlayer.GetComponent<Player>().id;
            var x = position.x;
            var y = position.y;
            var z = position.z;
            var rotation = currentPlayer.transform.rotation.y;
            if (id > 0)
            {
                var connect = new StompMessage("SEND", JsonUtility.ToJson(new DtoPlayer(id, x, y, z, rotation)));
                connect["destination"] = "/app/updatePlayer";
                StompMessageSerializer serializer = new StompMessageSerializer();
                ws.Send(serializer.Serialize(connect));
            }
            _getPlayers();
        }
        
        // void OnApplicationQuit()
        // {
        //     var SpawnerController = Spawner.GetComponent<Spawner>();
        //     GameObject currentPlayer = SpawnerController.myCurrentPlayer;
        //     print("Quit");
        //     var position = currentPlayer.transform.position;
        //     int id = currentPlayer.GetComponent<Player>().id;
        //     Double x = position.x;
        //     Double y = position.y;
        //     Double z = position.z;
        //     Double rotation = currentPlayer.transform.rotation.y;
        //     
        //     var connect = new StompMessage("SEND", "{\"id\":\"" + id +"\"," +
        //                                            "\"x\":\"" + x.ToString().Replace(",", ".") +"\"," + 
        //                                            "\"y\":\"" + y.ToString().Replace(",", ".") +"\"," +
        //                                            "\"z\":\"" + z.ToString().Replace(",", ".") +"\"," +
        //                                            "\"rotation\":\"" + rotation.ToString().Replace(",", ".") +"\"}");
        //     connect["destination"] = "/app/quit";
        //     StompMessageSerializer serializer = new StompMessageSerializer();
        //     ws.Send(serializer.Serialize(connect));
        // }
    }
}