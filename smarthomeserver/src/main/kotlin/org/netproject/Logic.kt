package org.netproject

import org.netproject.home.HomeState
import org.netproject.home.HomeState.Companion.update
import org.netproject.home.Object
import org.netproject.home.Roles
import org.netproject.home.Roles.GUEST
import org.netproject.home.Roles.HOST
import org.netproject.home.Session
import org.netproject.message.Player
import org.springframework.scheduling.annotation.Scheduled
import org.springframework.stereotype.Service

@Service
class Logic(private val homeState: HomeState) {
    @Scheduled(fixedRate = 20)
    fun update() {
        homeState.doors.forEach { (key, door) ->
            homeState.players.values.filter { hasAccess(it, door) && it.player != null }.any { session ->
                Position.dist(session.player!!, door) < 3
            }.let { homeState.doors.update(key) { copy(open = it) } }
        }
        homeState.lights.forEach { (key, light) ->
            homeState.players.values.filter { hasAccess(it, light) && it.player != null }.any { session ->
                Position.dist(session.player!!, light) < 8
            }.let { homeState.lights.update(key) { copy(open = it) } }
        }
    }

    private fun hasAccess(session: Session, obj: Object): Boolean =
            session.role == HOST || obj.level == GUEST
}