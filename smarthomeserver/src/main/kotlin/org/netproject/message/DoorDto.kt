package org.netproject.message

import com.fasterxml.jackson.annotation.JsonProperty

data class DoorDto(
        @JsonProperty
        val isOpen: Boolean,
        @JsonProperty
        val doorID: String
)
