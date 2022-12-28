package org.netproject.message

import com.fasterxml.jackson.annotation.JsonProperty

data class LightDto(
        @JsonProperty
        val isOpen: Boolean,
        @JsonProperty
        val lightID: String
)
