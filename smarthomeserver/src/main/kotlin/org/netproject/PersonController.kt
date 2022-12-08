package org.netproject

import org.springframework.messaging.handler.annotation.MessageMapping
import org.springframework.messaging.handler.annotation.SendTo
import org.springframework.stereotype.Controller
import org.springframework.web.bind.annotation.RequestBody
import java.util.concurrent.ConcurrentHashMap
import java.util.concurrent.atomic.AtomicInteger

@Controller
class PersonController {
    private val players = ConcurrentHashMap<Int, Player>()
    private val counter = AtomicInteger(0)
    @MessageMapping("/register")
    @SendTo("/topic/id")
    fun register(@RequestBody message: HelloMessage): Int {
        return counter.incrementAndGet()
    }

    @MessageMapping("/quit")
    @SendTo("/topic/void")
    fun quit(player: Player): Int {
        val id = player.id
        players.remove(id)
        return 0
    }

    @MessageMapping("/updatePlayer")
    @SendTo("/topic/void")
    fun updatePlayer(player: Player): Int {
        println("update from $player")
        val id = player.id
        players[id] = player
        return 0
    }

    @MessageMapping("/getPlayers")
    @SendTo("/topic/players")
    fun getPlayers(message: HelloMessage): Players {
        println(players.size)
        return Players(players.values.toList())
    }
}