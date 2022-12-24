using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SmartEl.Dtos;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using WebSocketSharp;
using Object = System.Object;

namespace SmartEl
{
    public class Client : MonoBehaviour
    {
        public Button Button;
        // public Text host;
        public GameObject Spawner;
        public Spawner spawnerComponent;
        public Text host;
        WebSocket ws;
        private string id;

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
                sub["id"] = "sub-" + clientId + "id";
                sub["destination"] = "/topic/id";
                ws.Send(serializer.Serialize(sub));

                sub["id"] = "sub-" + clientId + "players";
                sub["destination"] = "/topic/players";
                ws.Send(serializer.Serialize(sub));
            };
            ws.OnMessage += (sender, e) =>
            {
                // print("Spring now says: " + e.Data);
                var body = e.Data.Substring(e.Data.IndexOf("\n\n") + 2);
                if (e.Data.Contains("/topic/id"))
                {
                    var message = JsonUtility.FromJson<Message<string>>(body);
                    if (message.key == clientId)
                    {
                        print(body);
                        id = message.payload;
                    }
                }
                if (e.Data.Contains("/topic/players") && id != null)
                {
                    {
                        var message = JsonUtility.FromJson<Message<List<DtoPlayer>>>(body);
                        // spawnerComponent.UpdatePlayers(message.payload.ToArray());
                        print(id);
                        spawnerComponent.UpdatePlayers(message.payload.Where(p => p.id != id).ToArray());
                    }
                }
            };
            ws.OnError += (sender, e) =>
                print("Error: " + e.Exception);
            ws.Connect();
            _auth(clientId);
        }

        public void FixedUpdate()
        {
            if (id != null)
            {
                var playerController = spawnerComponent.currentPlayer.GetComponent<Player>();
                playerController.SetId(id);
                NoParamaterOnclick();
            }
        }

        private void _auth(string clientId)
        {
            var connect = new StompMessage("SEND", JsonUtility.ToJson(new Message<string>(clientId, null)));
            connect["destination"] = "/app/register";
            var serializer = new StompMessageSerializer();
            ws.Send(serializer.Serialize(connect));
        }
        
        private void NoParamaterOnclick()
        {
            var currentPlayer = spawnerComponent.currentPlayer;
            var position = currentPlayer.transform.position;
            var rotation = currentPlayer.transform.rotation;
            var id = currentPlayer.GetComponent<Player>().id;
            // print(id);
            if (id != null)
            {
                var connect = new StompMessage("SEND", JsonUtility.ToJson(new Message<DtoPlayer>(
                    id,
                    new DtoPlayer(id, position.x, position.y, position.z, rotation.x, rotation.y, rotation.z, rotation.w)))
                );
                connect["destination"] = "/app/updatePlayer";
                StompMessageSerializer serializer = new StompMessageSerializer();
                ws.Send(serializer.Serialize(connect));
            }
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