package org.netproject

import org.netproject.auth.Auth
import org.netproject.home.Door
import org.netproject.home.HomeState
import org.netproject.home.HomeState.Companion.update
import org.netproject.home.Roles.*
import org.netproject.home.Session
import org.netproject.message.*
import org.springframework.messaging.handler.annotation.Headers
import org.springframework.messaging.handler.annotation.MessageMapping
import org.springframework.messaging.handler.annotation.Payload
import org.springframework.messaging.handler.annotation.SendTo
import org.springframework.messaging.simp.SimpMessagingTemplate
import org.springframework.messaging.support.MessageHeaderAccessor
import org.springframework.scheduling.annotation.Scheduled
import org.springframework.stereotype.Controller
import java.util.*

@Controller
class Controller(
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

    @MessageMapping("/updateDoors")
    fun updateDoors(
            @Headers headers: MessageHeaderAccessor,
            @Payload message: Message<List<Update>>
    ) {
        if (homeState.players[headers.sessionId]?.role == HOST) {
            message.payload.forEach {
                if (!homeState.doors.contains(it.doorID)) {
                    homeState.doors[it.doorID] = Door(it.open, it.x, it.y, it.z)
                } else {
                    homeState.doors.update(it.doorID) { copy(open = it.open, x = it.x, y = it.y, z = it.z) }
                }
            }
        }
    }

    @MessageMapping("/updateLights")
    fun updateLights(
            @Headers headers: MessageHeaderAccessor,
            @Payload message: Message<List<Update>>
    ) {
        if (homeState.players[headers.sessionId]?.role == HOST) {
            message.payload.forEach {
                if (!homeState.lights.contains(it.doorID)) {
                    homeState.lights[it.doorID] = Door(it.open, it.x, it.y, it.z)
                } else {
                    homeState.lights.update(it.doorID) { copy(open = it.open, x = it.x, y = it.y, z = it.z) }
                }
            }
        }
    }

    @MessageMapping("/changeRole")
    @SendTo("/topic/id")
    fun changeRole(
            @Headers headers: MessageHeaderAccessor,
            @Payload message: Message<ChangeRoleRequest>
    ): Message<String> = message.payload.let {
        Message(message.key, auth.byPassword(headers.sessionId, it.password, Companion.byId(it.roleType)).toString())
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

    @Scheduled(fixedRate = 20)
    fun getDoors() {
        simpMessagingTemplate.convertAndSend(
                "/topic/doors",
                Message(ALL.ordinal.toString(), homeState.doors.map { (doorID, door) ->
                    DoorDto(
                            doorID = doorID,
                            isOpen = door.open
                    )
                })
        )
    }

    @Scheduled(fixedRate = 20)
    fun getLights() {
        simpMessagingTemplate.convertAndSend(
                "/topic/lights",
                Message(ALL.ordinal.toString(), homeState.lights.map { (lightID, door) ->
                    LightDto(
                            lightID = lightID,
                            isOpen = door.open
                    )
                })
        )
    }


    private val MessageHeaderAccessor.sessionId: String
        get() = getHeader("simpSessionId") as String
}