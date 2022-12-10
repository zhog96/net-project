package org.netproject

import org.springframework.messaging.handler.annotation.Header
import org.springframework.messaging.handler.annotation.MessageMapping
import org.springframework.messaging.handler.annotation.SendTo
import org.springframework.messaging.simp.SimpMessagingTemplate
import org.springframework.scheduling.annotation.Scheduled
import org.springframework.stereotype.Controller
import org.springframework.web.bind.annotation.RequestBody
import org.springframework.web.bind.annotation.RequestParam
import java.util.concurrent.ConcurrentHashMap
import java.util.concurrent.atomic.AtomicInteger

@Controller
class PersonController(
        private val simpMessagingTemplate: SimpMessagingTemplate
) {
    private val players = ConcurrentHashMap<Int, Player>()
    private val counter = AtomicInteger(0)
    @MessageMapping("/register")
    fun register(
            @Header("id")
            sessionId: String,
            @RequestBody message: HelloMessage
    ) {
        println("register ${sessionId}!")
        simpMessagingTemplate.convertAndSendToUser(
                sessionId, "/user/queue/specific-user", counter.incrementAndGet(), mapOf("id" to sessionId)
        )
    }

    @MessageMapping("/quit")
    // @SendTo("/topic/void")
    fun quit(player: Player): Int {
        val id = player.id
        players.remove(id)
        return 0
    }

    @MessageMapping("/updatePlayer")
    // @SendTo("/topic/void")
    fun updatePlayer(player: Player): Int {
        // println("update from $player")
        val id = player.id
        synchronized(players) {
            players.put(id, player)
        }
        return 0
    }

    @MessageMapping("/getPlayers")
    @SendTo("/topic/players")
    fun getPlayers(
            message: HelloMessage
    ): Players {
        var p = emptyList<Player>();
        synchronized(players) {
            println(players.values.toList())
            p = players.values.toList()
        }
        return Players(p)
    }

//    @Scheduled(fixedRate = 10)
//    fun sendMessage() {
//        var p = emptyList<Player>();
//        synchronized(players) {
//            println(players.values.toList())
//            p = players.values.toList()
//        }
//        simpMessagingTemplate.convertAndSend("/topic/players", p)
//    }
}