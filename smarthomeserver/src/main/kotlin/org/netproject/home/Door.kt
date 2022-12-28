package org.netproject.home

import org.netproject.Position

data class Door(
        val open: Boolean,
        override val x: Double,
        override val y: Double,
        override val z: Double
): Position