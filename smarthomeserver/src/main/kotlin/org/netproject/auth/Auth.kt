package org.netproject.auth

import org.netproject.home.Roles

interface Auth {
    fun byPassword(sessionId: String, password: String, role: Roles): Boolean
}