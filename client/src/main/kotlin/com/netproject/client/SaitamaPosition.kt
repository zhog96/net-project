package com.netproject.client

import com.fasterxml.jackson.annotation.JsonProperty

data class SaitamaPosition(
        @Volatile
        @JsonProperty("x")
        var x: Float,
        @Volatile
        @JsonProperty("y")
        var y: Float
)