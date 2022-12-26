package org.netproject

import org.netproject.auth.Auth
import org.netproject.auth.SimpleAuth
import org.netproject.home.HomeState
import org.netproject.home.HomeState.Companion.update
import org.netproject.home.Roles.*
import org.netproject.home.Session
import org.netproject.message.ChangeRoleRequest
import org.netproject.message.Message
import org.netproject.message.Player
import org.springframework.messaging.handler.annotation.Headers
import org.springframework.messaging.handler.annotation.MessageMapping
import org.springframework.messaging.handler.annotation.Payload
import org.springframework.messaging.handler.annotation.SendTo
import org.springframework.messaging.simp.SimpMessagingTemplate
import org.springframework.messaging.support.MessageHeaderAccessor
import org.springframework.scheduling.annotation.Scheduled
import org.springframework.stereotype.Controller
import java.util.*
import java.util.concurrent.ConcurrentHashMap
import javax.management.relation.Role

@Controller
class PersonController(
        private val simpMessagingTemplate: SimpMessagingTemplate,
        private val homeState: HomeState,
        private val auth: Auth
) {

    @MessageMapping("/register")
    @SendTo("/topic/id")
    fun register(
            @Headers headers: MessageHeaderAccessor,
            @Payload message: Message<String>
    ): Message<String> {
        val token = "player ${UUID.randomUUID()}"
        homeState.players[headers.sessionId] = Session(token)
        return Message(message.key, token)
    }

    @MessageMapping("/changeRole")
    @SendTo("/topic/id")
    fun changeRole(
            @Headers headers: MessageHeaderAccessor,
            @Payload message: Message<ChangeRoleRequest>
    ): Message<String> = message.payload.let {
        val a = Message(message.key, auth.byPassword(headers.sessionId, it.password, Companion.byId(it.roleType)).toString())
        println(homeState.players.values.toList())
        a
    }

    @MessageMapping("/quit")
    // @SendTo("/topic/void")
    fun quit(player: Player): Int {
        val id = player.id
        // players.remove(id)
        return 0
    }

    @MessageMapping("/updatePlayer")
    fun updatePlayer(
            @Headers headers: MessageHeaderAccessor,
            @Payload message: Message<Player>) {
        homeState.players.update(headers.sessionId) { copy(player = message.payload) }
    }

    @Scheduled(fixedRate = 20)
    fun getPlayers() {
        simpMessagingTemplate.convertAndSend(
                "/topic/players",
                Message(ALL.ordinal.toString(), homeState.players.values.mapNotNull { it.player })
        )
    }

    private val MessageHeaderAccessor.sessionId: String
        get() = getHeader("simpSessionId") as String
}