package org.netproject.message

import com.fasterxml.jackson.annotation.JsonProperty

data class DoorDto(
        @JsonProperty
        val open: Boolean,
        @JsonProperty
        val doorID: String
)
