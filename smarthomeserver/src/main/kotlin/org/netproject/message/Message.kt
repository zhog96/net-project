package org.netproject.message

import com.fasterxml.jackson.annotation.JsonProperty

data class Message<T> (
        @JsonProperty
        val key: String,
        @JsonProperty
        val payload: T
)
