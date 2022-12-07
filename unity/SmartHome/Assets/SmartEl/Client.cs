using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

using WebSocketSharp;
using Random = UnityEngine.Random;

namespace SmartEl
{
    public class Client : MonoBehaviour
    {
        public Button Button;
        // public Text host;
        public GameObject Spawner;
        public Text host;
        WebSocket ws;
        public int id;

        public void Start()
        {
            Button.onClick.AddListener(Subscribe);
        }
        
        private void Subscribe()
        {
            Regex regexId = new Regex(@"\n\d+");


            var SpawnerController = Spawner.GetComponent<Spawner>();
            var PlayerController = SpawnerController.myCurrentPlayer.GetComponent<Player>();
            // 1. Установить связь с GUI
            // 2. ? Установить связь с игроком

            // 3. Установить связь с сервером
            ws = new WebSocket("ws://"+ host.text +":8080/gs-guide-websocket");
            int clientId = Random.Range(0, 100);
            print("enter using");
            ws.OnOpen += (sender, e) =>
            {
                print("Spring says: open");
                StompMessageSerializer serializer = new StompMessageSerializer();
                
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
                auth();
		          
            };
	           
            ws.OnError += (sender, e) =>
                print("Error: " + e.Message);
            ws.OnMessage += (sender, e) =>
            {
                print("Spring now says: " + e.Data);
                if (e.Data.Contains("/topic/id"))
                {
                    MatchCollection match = regexId.Matches(e.Data);
                    int lId = int.Parse(match[0].Value);
                    print(lId);
                    this.id = lId;
                    // Task.Run(() => forceSetPlayerId(lId));
                }
                if (e.Data.Contains("/topic/players"))
                {
                    print("begin parse json");
                    Task.Run(() => parseJson(e.Data, SpawnerController));
                }
            };
            

            print("enter connect");
            ws.Connect();
            
            print(ws.IsAlive);
        }

        public void FixedUpdate()
        {
            // 1. Слушать сервер
            //   1.1. Запрос на добавление другого игрока
            //   1.2. Смотреть передвижения других игроков


            // 1. Отправлять на сервер
            //   1.1. Запрос на исключение себя из списка других игроков
            //   1.2. Отправлять свои координаты
            var SpawnerController = Spawner.GetComponent<Spawner>();
            var PlayerController = SpawnerController.myCurrentPlayer.GetComponent<Player>();
            PlayerController.SetId(this.id);
            NoParamaterOnclick();
        }
        
        void parseJson(string raw, Spawner spawnerController)
        {
            int start = raw.IndexOf('[');
            int end = raw.IndexOf(']');
            var jsonArray = raw.Substring(start, end - start + 1);
            var deptList = JsonConvert.DeserializeObject<List<DtoPlayer>>(jsonArray);
            print("JsonConvert.DeserializeObject size of " + deptList.Count);
            foreach(var dept in deptList)
            {
                print("Try process another player" + dept.id);
                if (dept.id != this.id)
                {
                    print("Internal try process another player");
                    spawnerController.UpdatePlayers(dept);
                }
            }
        }
        
        private void auth()
        {
            var connect = new StompMessage("SEND", "{\"name\":\"meh\"}");
            connect["destination"] = "/app/register";
            StompMessageSerializer serializer = new StompMessageSerializer();
            ws.Send(serializer.Serialize(connect));
        }
        
        private void getPlayers()
        {
            var connect = new StompMessage("SEND", "{\"name\":\"meh\"}");
            connect["destination"] = "/app/getPlayers";
            StompMessageSerializer serializer = new StompMessageSerializer();
            ws.Send(serializer.Serialize(connect));
        }
        private void NoParamaterOnclick()
        {
            var SpawnerController = Spawner.GetComponent<Spawner>();
            GameObject currentPlayer = SpawnerController.myCurrentPlayer;
            print("Send my coordinates");
            var position = currentPlayer.transform.position;
            int id = currentPlayer.GetComponent<Player>().id;
            Double x = position.x;
            Double y = position.y;
            Double z = position.z;
            Double rotation = currentPlayer.transform.rotation.y;
            
            var connect = new StompMessage("SEND", "{\"id\":\"" + id +"\"," +
                                                   "\"x\":\"" + x.ToString().Replace(",", ".") +"\"," + 
                                                   "\"y\":\"" + y.ToString().Replace(",", ".") +"\"," +
                                                   "\"z\":\"" + z.ToString().Replace(",", ".") +"\"," +
                                                   "\"rotation\":\"" + rotation.ToString().Replace(",", ".") +"\"}");
            connect["destination"] = "/app/updatePlayer";
            StompMessageSerializer serializer = new StompMessageSerializer();
            ws.Send(serializer.Serialize(connect));
            getPlayers();
        }
        
        void OnApplicationQuit()
        {
            var SpawnerController = Spawner.GetComponent<Spawner>();
            GameObject currentPlayer = SpawnerController.myCurrentPlayer;
            print("Quit");
            var position = currentPlayer.transform.position;
            int id = currentPlayer.GetComponent<Player>().id;
            Double x = position.x;
            Double y = position.y;
            Double z = position.z;
            Double rotation = currentPlayer.transform.rotation.y;
            
            var connect = new StompMessage("SEND", "{\"id\":\"" + id +"\"," +
                                                   "\"x\":\"" + x.ToString().Replace(",", ".") +"\"," + 
                                                   "\"y\":\"" + y.ToString().Replace(",", ".") +"\"," +
                                                   "\"z\":\"" + z.ToString().Replace(",", ".") +"\"," +
                                                   "\"rotation\":\"" + rotation.ToString().Replace(",", ".") +"\"}");
            connect["destination"] = "/app/quit";
            StompMessageSerializer serializer = new StompMessageSerializer();
            ws.Send(serializer.Serialize(connect));
        }
    }
}