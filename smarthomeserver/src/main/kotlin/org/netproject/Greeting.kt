package org.netproject

import com.fasterxml.jackson.annotation.JsonProperty

class Greeting(
        @JsonProperty("content")
        val content: String,
)