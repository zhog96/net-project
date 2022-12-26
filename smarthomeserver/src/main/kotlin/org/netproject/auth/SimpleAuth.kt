package org.netproject.auth

import org.netproject.home.HomeState
import org.netproject.home.HomeState.Companion.update
import org.netproject.home.Roles
import org.netproject.home.Roles.HOST
import org.springframework.stereotype.Service

@Service
class SimpleAuth(private val homeState: HomeState) : Auth {
    private val hostPassword = "Наруто"

    override fun byPassword(sessionId: String, password: String, role: Roles): Boolean {
        val authed = when (role) {
            HOST -> password == hostPassword
            else -> true
        }
        if (authed) {
            homeState.players.update(sessionId) { copy(role = role) }
        }
        return authed
    }
}