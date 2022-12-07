package org.netproject

import org.springframework.messaging.handler.annotation.MessageMapping
import org.springframework.messaging.handler.annotation.SendTo
import org.springframework.stereotype.Controller
import java.util.concurrent.ConcurrentHashMap
import java.util.concurrent.CopyOnWriteArrayList
import java.util.concurrent.atomic.AtomicInteger

@Controller
class PersonController {
    private val players: MutableMap<Int?, Player> = ConcurrentHashMap()
    private val ids: MutableList<Int?> = CopyOnWriteArrayList()
    private val counter = AtomicInteger(0)
    @MessageMapping("/register")
    @SendTo("/topic/id")
    @Throws(Exception::class)
    fun register(message: HelloMessage?): Int {
        println("Request for id fromClient")
        return newId
    }

    @MessageMapping("/quit")
    @SendTo("/topic/void")
    @Throws(Exception::class)
    fun quit(player: Player): Int {
        println("Quit for id " + player.id)
        ids.remove(player.id)
        players.remove(player.id)
        return 0
    }

    @MessageMapping("/updatePlayer")
    @SendTo("/topic/void")
    @Throws(Exception::class)
    fun updatePlayer(player: Player): Int {
        println("Request for update id " + player.id + " fromClient")
        players[player.id]!!.updatePlayer(player)
        return 0
    }

    @MessageMapping("/getPlayers")
    @SendTo("/topic/players")
    @Throws(Exception::class)
    fun getPlayers(message: HelloMessage?): Collection<Player> {
        println("Request for get all Players")
        return players.values
    }

    @get:Synchronized
    private val newId: Int
        private get() {
            val player = Player()
            val id = counter.incrementAndGet()
            player.id = id
            ids.add(id)
            players[id] = player
            return id
        }
}