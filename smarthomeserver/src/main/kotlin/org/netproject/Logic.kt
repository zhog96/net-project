package org.netproject

import org.netproject.home.HomeState
import org.netproject.home.HomeState.Companion.update
import org.netproject.home.Object
import org.netproject.home.Roles.GUEST
import org.netproject.home.Roles.HOST
import org.netproject.home.Session
import org.springframework.scheduling.annotation.Scheduled
import org.springframework.stereotype.Service
import java.time.Instant.now
import java.util.concurrent.CopyOnWriteArrayList

@Service
class Logic(private val homeState: HomeState) {
    val events = CopyOnWriteArrayList<String>()

    @Scheduled(fixedRate = 20)
    fun update() {
        homeState.doors.forEach { (key, door) ->
            homeState.players.values.filter { hasAccess(it, door) && it.player != null }.any { session ->
                Position.dist(session.player!!, door) < 3
            }.let {
                if (it != door.open) {
                    events.add("[${now()}] ${
                        when (it) {
                            true -> "Opening"
                            else -> "Closing"
                        }
                    } door $key")
                }
                homeState.doors.update(key) {
                    copy(open = it)
                }
            }
        }
        homeState.lights.forEach { (key, light) ->
            homeState.players.values.filter { hasAccess(it, light) && it.player != null }.any { session ->
                Position.dist(session.player!!, light) < 8
            }.let {
                if (it != light.open) {
                    events.add("[${now()}] ${
                        when (it) {
                            true -> "Turning on"
                            else -> "Turning off"
                        }
                    } light $key")
                }
                homeState.lights.update(key) { copy(open = it) }
            }
        }
    }

    private fun hasAccess(session: Session, obj: Object): Boolean =
            session.role == HOST || obj.level == GUEST
}