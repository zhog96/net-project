package org.netproject.message

import com.fasterxml.jackson.annotation.JsonProperty

data class LightDto(
        @JsonProperty
        val enable: Boolean,
        @JsonProperty
        val lightID: String
)
