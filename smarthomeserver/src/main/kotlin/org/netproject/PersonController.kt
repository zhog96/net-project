package org.netproject

import org.netproject.message.Message
import org.netproject.message.Player
import org.springframework.messaging.handler.annotation.MessageMapping
import org.springframework.messaging.handler.annotation.SendTo
import org.springframework.messaging.simp.SimpMessagingTemplate
import org.springframework.scheduling.annotation.Scheduled
import org.springframework.stereotype.Controller
import org.springframework.web.bind.annotation.RequestBody
import java.util.concurrent.ConcurrentHashMap
import java.util.concurrent.atomic.AtomicInteger

@Controller
class PersonController(
        private val simpMessagingTemplate: SimpMessagingTemplate
) {
    private val players = ConcurrentHashMap<String, Player>()
    private val counter = AtomicInteger(0)

    @MessageMapping("/register")
    @SendTo("/topic/id")
    fun register(
            @RequestBody message: Message<String>
    ): Message<String> {
        val playerId = "player ${counter.incrementAndGet()}"
        println("register ${message.key} as ${playerId}!")
        return Message(message.key, playerId)
    }

    @MessageMapping("/quit")
    // @SendTo("/topic/void")
    fun quit(player: Player): Int {
        val id = player.id
        // players.remove(id)
        return 0
    }

    @MessageMapping("/updatePlayer")
    fun updatePlayer(message: Message<Player>) {
        players[message.key] = message.payload
    }

    @Scheduled(fixedRate = 20)
    fun getPlayers() {
        simpMessagingTemplate.convertAndSend(
                "/topic/players",
                Message("ALL", players.values.toList())
        )
    }
}