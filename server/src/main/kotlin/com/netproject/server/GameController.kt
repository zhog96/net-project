package com.netproject.server

import org.springframework.http.ResponseEntity
import org.springframework.messaging.handler.annotation.MessageMapping
import org.springframework.stereotype.Controller
import org.springframework.web.bind.annotation.RequestBody


@Controller
class GameController {
    @MessageMapping("/update")
    fun update(@RequestBody delta: Saitama): ResponseEntity<Long> {
        SaitamaStore.x += delta.x
        SaitamaStore.y += delta.y
        return ResponseEntity.ok(1)
    }
}