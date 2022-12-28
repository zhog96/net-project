package org.netproject.message

import com.fasterxml.jackson.annotation.JsonProperty

data class Update(
        @JsonProperty
        val id: String,
        @JsonProperty
        val x: Double,
        @JsonProperty
        val y: Double,
        @JsonProperty
        val z: Double
)
