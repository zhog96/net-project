package org.netproject.message

import com.fasterxml.jackson.annotation.JsonProperty

data class Player(
        @JsonProperty
        val id: String,
        @JsonProperty
        val uid: String,
        @JsonProperty
        val x: Double,
        @JsonProperty
        val y: Double,
        @JsonProperty
        val z: Double,
        @JsonProperty
        val rx: Double,
        @JsonProperty
        val ry: Double,
        @JsonProperty
        val rz: Double,
        @JsonProperty
        val w: Double,
)