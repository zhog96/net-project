package org.netproject.home

import org.springframework.stereotype.Service
import java.util.concurrent.ConcurrentHashMap

@Service
class HomeState {
    val players = ConcurrentHashMap<String, Session>()

    fun quit(key: String) {
        try {
            players.remove(key)
        } catch (_: NullPointerException) {}
    }

    companion object {
        fun ConcurrentHashMap<String, Session>.update(key: String, body: Session.() -> Session) {
            this[key] = this[key]?.body() ?: return
        }
    }
}