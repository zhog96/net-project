package org.netproject

import org.netproject.home.HomeState
import org.netproject.home.HomeState.Companion.update
import org.springframework.scheduling.annotation.Scheduled
import org.springframework.stereotype.Service

@Service
class Logic(private val homeState: HomeState) {
    @Scheduled(fixedRate = 20)
    fun update() {
        homeState.doors.forEach { (key, door) ->
            homeState.players.values.mapNotNull { it.player }.any { player ->
                Position.dist(player, door) < 3
            }.let { homeState.doors.update(key) { copy(open = it) } }
        }
        homeState.lights.forEach { (key, light) ->
            homeState.players.values.mapNotNull { it.player }.any { player ->
                Position.dist(player, light) < 8
            }.let { homeState.lights.update(key) { copy(open = it) } }
        }
    }
}