package org.netproject.home

import org.netproject.home.Roles.*
import org.netproject.message.Player

data class Session(
        val token: String,
        val role: Roles = GUEST,
        val player: Player? = null
)