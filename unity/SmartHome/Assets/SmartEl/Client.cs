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
        public GameObject UIControllerObject;
        public UIController UIControllerScript;
        WebSocket ws;
        private string id;

        public void Start()
        {
            Screen.SetResolution(960, 540, false, 60);
            spawnerComponent = Spawner.GetComponent<Spawner>();
            UIControllerScript = UIControllerObject.GetComponent<UIController>();
            Button.onClick.AddListener(Subscribe);
            UIControllerScript.ButtonToRequestHost.onClick.AddListener(_tryRequestHost);
            UIControllerScript.ButtonToRequestGuest.onClick.AddListener(_tryRequestGuest);
        }

        private void Subscribe()
        {
            var clientId = Guid.NewGuid().ToString();
            // ws = new WebSocket("ws://"+ UIControllerScript.IP.text +":8080/gs-guide-websocket");
            ws = new WebSocket("ws://" + "127.0.0.1" + ":8080/gs-guide-websocket");
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
                var body = e.Data.Substring(e.Data.IndexOf("\n\n") + 2);
                if (e.Data.Contains("/topic/id"))
                {
                    var message = JsonUtility.FromJson<Message<string>>(body);
                    if (message.key == clientId)
                    {
                        print(body);
                        if (message.payload.Contains("RoleResponse"))
                        {
                            //todo check response and chose role
                        }
                        else
                        {
                            id = message.payload;
                        }
                    }
                }

                if (e.Data.Contains("/topic/players") && id != null)
                {
                    {
                        var message = JsonUtility.FromJson<Message<List<DtoPlayer>>>(body);
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
        
        private void _tryRequestHost()
        {
            print("Sdafasfdfd");
            var connect = new StompMessage("SEND", JsonUtility.ToJson(new Message<RoleRequest>(id, new RoleRequest(RolesEnum.Host, UIControllerScript.Password.text))));
            connect["destination"] = "/app/register";
            var serializer = new StompMessageSerializer();
            ws.Send(serializer.Serialize(connect));
        }
        
        private void _tryRequestGuest()
        {
            var connect = new StompMessage("SEND", JsonUtility.ToJson(new Message<RoleRequest>(id, new RoleRequest(RolesEnum.Guest, ""))));
            connect["destination"] = "/app/register";
            var serializer = new StompMessageSerializer();
            ws.Send(serializer.Serialize(connect));
        }

        private void NoParamaterOnclick()
        {
            var currentPlayer = spawnerComponent.currentPlayer;
            var position = currentPlayer.transform.position;
            var rotation = currentPlayer.transform.rotation;
            // print(id);
            if (id != null)
            {
                var connect = new StompMessage("SEND", JsonUtility.ToJson(new Message<DtoPlayer>(
                    id,
                    new DtoPlayer(id, Guid.NewGuid().ToString(), position.x, position.y, position.z, rotation.x,
                        rotation.y, rotation.z, rotation.w)))
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