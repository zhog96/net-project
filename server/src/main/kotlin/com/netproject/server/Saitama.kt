package com.netproject.server

import com.fasterxml.jackson.annotation.JsonProperty

data class Saitama(
        @JsonProperty("x")
        val x: Float,
        @JsonProperty("y")
        val y: Float
)