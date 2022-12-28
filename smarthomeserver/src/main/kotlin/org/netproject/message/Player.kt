package org.netproject.message

import com.fasterxml.jackson.annotation.JsonProperty
import org.netproject.Position

data class Player(
        @JsonProperty
        val id: String,
        @JsonProperty
        val uid: String,
        @JsonProperty
        override val x: Double,
        @JsonProperty
        override val y: Double,
        @JsonProperty
        override val z: Double,
        @JsonProperty
        val rx: Double,
        @JsonProperty
        val ry: Double,
        @JsonProperty
        val rz: Double,
        @JsonProperty
        val w: Double,
): Position