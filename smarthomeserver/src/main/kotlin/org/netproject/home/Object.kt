package org.netproject.home

import org.netproject.Position

data class Object(
        val open: Boolean,
        val level: Roles,
        override val x: Double,
        override val y: Double,
        override val z: Double
): Position