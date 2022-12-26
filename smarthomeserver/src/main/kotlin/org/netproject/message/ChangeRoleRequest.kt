package org.netproject.message

import com.fasterxml.jackson.annotation.JsonProperty

data class ChangeRoleRequest (
        @JsonProperty
        val roleType: Int,
        @JsonProperty
        val password: String
)