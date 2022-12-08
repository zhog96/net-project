package org.netproject

import com.fasterxml.jackson.annotation.JsonProperty

class Players (
        @JsonProperty
        val players: List<Player>
)