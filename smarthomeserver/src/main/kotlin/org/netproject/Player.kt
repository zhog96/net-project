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
        @JsonProperty("rotation")
        val rotation: Double
)