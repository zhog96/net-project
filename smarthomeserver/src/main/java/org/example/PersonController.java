package org.example;

import org.springframework.messaging.handler.annotation.MessageMapping;
import org.springframework.messaging.handler.annotation.SendTo;
import org.springframework.stereotype.Controller;
import org.springframework.web.util.HtmlUtils;

import java.util.ArrayList;
import java.util.Collection;
import java.util.List;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.CopyOnWriteArrayList;
import java.util.concurrent.atomic.AtomicInteger;

@Controller
public class PersonController {

    private Map<Integer, Player> players = new ConcurrentHashMap<>();
    private List<Integer> ids = new CopyOnWriteArrayList<>();
    private AtomicInteger counter = new AtomicInteger(0);

    @MessageMapping("/register")
    @SendTo("/topic/id")
    public Integer register(HelloMessage message) throws Exception {
        System.out.println("Request for id fromClient");
        return getNewId();
    }

    @MessageMapping("/quit")
    @SendTo("/topic/void")
    public Integer quit(Player player) throws Exception {
        System.out.println("Quit for id " + player.id);
        ids.remove(player.id);
        players.remove(player.id);
        return 0;
    }

    @MessageMapping("/updatePlayer")
    @SendTo("/topic/void")
    public Integer updatePlayer(Player player) throws Exception {
        System.out.println("Request for update id " + player.id + " fromClient");
        players.get(player.id).updatePlayer(player);
        return 0;
    }

    @MessageMapping("/getPlayers")
    @SendTo("/topic/players")
    public Collection<Player> getPlayers(HelloMessage message) throws Exception {
        System.out.println("Request for get all Players");
        return players.values();
    }

    synchronized
    private Integer getNewId(){
        Player player = new Player();
        Integer id = counter.incrementAndGet();
        player.id = id;
        ids.add(id);
        players.put(id, player);
        return id;
    }
}