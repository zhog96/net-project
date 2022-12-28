package org.netproject

import org.netproject.home.HomeState
import org.netproject.home.Object
import org.netproject.home.Session
import org.springframework.web.bind.annotation.GetMapping
import org.springframework.web.bind.annotation.RequestMapping
import org.springframework.web.bind.annotation.RestController

@RestController
@RequestMapping("/info")
class StatsController(
        private val homeState: HomeState,
        private val logic: Logic
) {
    @GetMapping("/p")
    fun players(): List<Session> = homeState.players.values.toList()

    @GetMapping("/d")
    fun doors(): List<Object> = homeState.doors.values.toList()

    @GetMapping("/l")
    fun lights(): List<Object> = homeState.lights.values.toList()

    @GetMapping("/e")
    fun events(): String = logic.events.subList(
            (logic.events.size - 50).coerceAtLeast(0),
            logic.events.size
    ).joinToString(separator = "\n")
}