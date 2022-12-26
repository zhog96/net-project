package org.netproject.home

enum class Roles {
    GUEST,
    HOST,
    ALL;

    companion object {
        fun byId(idx: Int): Roles = Roles.values()[idx]
    }
}