package org.netproject.message

import com.fasterxml.jackson.annotation.JsonProperty

data class Update(
        @JsonProperty
        val doorID: String,
        @JsonProperty
        val x: Double,
        @JsonProperty
        val y: Double,
        @JsonProperty
        val z: Double,
        @JsonProperty
        val open: Boolean
)
