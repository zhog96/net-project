package org.netproject.home

import org.springframework.stereotype.Service
import java.util.concurrent.ConcurrentHashMap

@Service
class HomeState {
    val players = ConcurrentHashMap<String, Session>()
    val doors = ConcurrentHashMap<String, Object>()
    val lights = ConcurrentHashMap<String, Object>()

    fun quit(key: String) {
        try {
            players.remove(key)
        } catch (_: NullPointerException) {}
    }

    companion object {
        fun <T> ConcurrentHashMap<String, T>.update(key: String, body: T.() -> T) {
            this[key] = this[key]?.body() ?: return
        }
    }
}