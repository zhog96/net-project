package org.netproject

import com.fasterxml.jackson.annotation.JsonProperty

data class Player(
        @JsonProperty("id")
        val id: Int,
        @JsonProperty("x")
        val x: Double,
        @JsonProperty("y")
        val y: Double,
        @JsonProperty("z")
        val z: Double,
        @JsonProperty("rx")
        val rx: Double,
        @JsonProperty("ry")
        val ry: Double,
        @JsonProperty("rz")
        val rz: Double,
        @JsonProperty("w")
        val w: Double,
)