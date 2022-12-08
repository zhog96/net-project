package org.netproject

import com.fasterxml.jackson.annotation.JsonProperty

class HelloMessage(
        @JsonProperty("name")
        val name: String,
)