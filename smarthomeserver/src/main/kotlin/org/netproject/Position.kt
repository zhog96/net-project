package org.netproject

import kotlin.math.pow
import kotlin.math.sqrt

interface Position {
    val x: Double
    val y: Double
    val z: Double;

    companion object {
        fun dist(a: Position, b: Position): Double = sqrt((b.x - a.x).pow(2) + (b.y - a.y).pow(2) + (b.z - a.z).pow(2))
    }
}
