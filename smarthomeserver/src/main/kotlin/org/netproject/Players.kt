package org.netproject

import com.fasterxml.jackson.annotation.JsonProperty

data class Players (
        @JsonProperty
        val players: List<Player>
)