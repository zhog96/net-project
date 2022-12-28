using System;
using System.Collections.Generic;
using System.Linq;
using SmartEl.Dtos;
using UnityEngine;
using WebSocketSharp;

namespace SmartEl
{
    public class Client : MonoBehaviour
    {
        // public Text host;
        public GameObject Spawner;
        public Spawner spawnerComponent;
        public GameObject UIControllerObject;
        public UIController UIControllerScript;
        public GameObject[] SmartDoorsObjects;
        public SmartDoor[] SmartDoors;
        public Dictionary<string, SmartDoor> SmartDoorsMap = new();
        public volatile DtoDoor[] DtoDoors = {};
        public GameObject[] SmartLightsObjects;
        public volatile SmartLight[] SmartLights;
        public Dictionary<string, SmartLight> SmartLightsMap = new();
        public DtoLight[] DtoLights = {};

        WebSocket ws;
        private string id;

        public void Start()
        {
            Screen.SetResolution(960, 540, false, 60);
            spawnerComponent = Spawner.GetComponent<Spawner>();
            UIControllerScript = UIControllerObject.GetComponent<UIController>();
            UIControllerScript.ButtonToSubscribe.onClick.AddListener(Subscribe);
            UIControllerScript.ButtonToRequestHost.onClick.AddListener(_tryRequestHost);
            UIControllerScript.ButtonToRequestGuest.onClick.AddListener(_tryRequestGuest);
            
            SmartDoorsObjects = GameObject.FindGameObjectsWithTag("SmartDoor");
            SmartDoors = new SmartDoor[SmartDoorsObjects.Length];
            for (int i = 0; i < SmartDoorsObjects.Length; i++)
            {
                SmartDoors[i] = SmartDoorsObjects[i].GetComponent<SmartDoor>();
                SmartDoorsMap.Add(SmartDoors[i].Id, SmartDoors[i]);
                print(SmartDoors[i].Id + " " + SmartDoors[i].open);
            }

            SmartLightsObjects = GameObject.FindGameObjectsWithTag("SmartLight");
            SmartLights = new SmartLight[SmartLightsObjects.Length];
            for (int i = 0; i < SmartLightsObjects.Length; i++)
            {
                SmartLights[i] = SmartLightsObjects[i].GetComponent<SmartLight>();
                SmartLightsMap.Add(SmartLights[i].Id, SmartLights[i]);
                print(SmartLights[i].Id + " " + SmartLights[i].enabled);
            }
        }

        private void Subscribe()
        {
            var clientId = Guid.NewGuid().ToString();
            // ws = new WebSocket("ws://"+ UIControllerScript.IP.text +":8080/gs-guide-websocket");
            // ws = new WebSocket("ws://"+ "51.250.9.76" +":8080/gs-guide-websocket");
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
                
                sub["id"] = "sub-" + clientId + "doors";
                sub["destination"] = "/topic/doors";
                ws.Send(serializer.Serialize(sub));

                sub["id"] = "sub-" + clientId + "doors";
                sub["destination"] = "/topic/lights";
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
                
                if (e.Data.Contains("/topic/doors") && id != null)
                {
                    {
                        var message = JsonUtility.FromJson<Message<List<DtoDoor>>>(body);
                        DtoDoors = message.payload.ToArray();
                    }
                }

                if (e.Data.Contains("/topic/lights") && id != null)
                {
                    print(body);
                    {
                        var message = JsonUtility.FromJson<Message<List<DtoLight>>>(body);
                        DtoLights = message.payload.ToArray();
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
            SendDoorUpdates();
            SendLightUpdates();
            UpdateDoors();
            UpdateLights();
        }
        
        public void SendDoorUpdates()
        {
            List<Updates> doorUpdates = new List<Updates>();
            foreach (var door in SmartDoors)
            {
                var pos = door.transform.position;
                doorUpdates.Add(new Updates(door.Id, pos.x, pos.y, pos.z, door.forAll ? RolesEnum.Guest : RolesEnum.Host));
            }
            var connect = new StompMessage("SEND", JsonUtility.ToJson(new Message<List<Updates>>(id, doorUpdates)));
            connect["destination"] = "/app/updateDoors";
            var serializer = new StompMessageSerializer();
            print(serializer.Serialize(connect));
            ws.Send(serializer.Serialize(connect));
        }

        public void SendLightUpdates()
        {
            List<Updates> lightUpdates = new List<Updates>();
            foreach (var smartLight in SmartLights)
            {
                var pos = smartLight.transform.position;
                lightUpdates.Add(new Updates(smartLight.Id, pos.x, pos.y, pos.z, RolesEnum.Guest));
            }
            var connect = new StompMessage("SEND", JsonUtility.ToJson(new Message<List<Updates>>(id, lightUpdates)));
            connect["destination"] = "/app/updateLights";
            var serializer = new StompMessageSerializer();
            ws.Send(serializer.Serialize(connect));
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
            var connect = new StompMessage("SEND", JsonUtility.ToJson(new Message<RoleRequest>(id, new RoleRequest(RolesEnum.Host, UIControllerScript.Password.text))));
            connect["destination"] = "/app/changeRole";
            var serializer = new StompMessageSerializer();
            ws.Send(serializer.Serialize(connect));
        }
        
        private void _tryRequestGuest()
        {
            var connect = new StompMessage("SEND", JsonUtility.ToJson(new Message<RoleRequest>(id, new RoleRequest(RolesEnum.Guest, ""))));
            connect["destination"] = "/app/changeRole";
            var serializer = new StompMessageSerializer();
            ws.Send(serializer.Serialize(connect));
        }

        private void NoParamaterOnclick()
        {
            var currentPlayer = spawnerComponent.currentPlayer;
            var position = currentPlayer.transform.position;
            var rotation = currentPlayer.transform.rotation;
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
        
        private void UpdateDoors()
        {
            for (int i = 0; i < DtoDoors.Length; i++)
            {
                var smartDoorDto = DtoDoors[i];
                var smartDoor = SmartDoorsMap[smartDoorDto.doorID];
                if (smartDoorDto.open.Equals(smartDoor.open))
                {
                    continue;
                }
                if (smartDoorDto.open.Equals(true) && smartDoor.open.Equals(false))
                {
                    smartDoor.opening();
                }
                if (smartDoorDto.open.Equals(false) && smartDoor.open.Equals(true))
                {
                    smartDoor.closing();
                }
            }
        }

        private void UpdateLights()
        {
            for (int i = 0; i < DtoLights.Length; i++)
            {
                var smartLightDto = DtoLights[i];
                var smartLight = SmartLightsMap[smartLightDto.lightID];
                if (smartLightDto.enable.Equals(smartLight.Light.enabled))
                {
                    continue;
                }
                if (smartLightDto.enable.Equals(true) && smartLight.Light.enabled.Equals(false))
                {
                    smartLight.TurnOnLight();
                }
                if (smartLightDto.enable.Equals(false) && smartLight.Light.enabled.Equals(true))
                {
                    smartLight.TurnOffLight();
                }
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