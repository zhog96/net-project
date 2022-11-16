package com.netproject.server

import org.springframework.messaging.simp.SimpMessagingTemplate
import org.springframework.scheduling.annotation.Scheduled
import org.springframework.stereotype.Service

@Service
class ScheduledPushMessages(
        private val simpMessagingTemplate: SimpMessagingTemplate
) {
    @Scheduled(fixedRate = 10)
    fun sendMessage() {
        simpMessagingTemplate.convertAndSend("/topic/saitama/", Saitama(SaitamaStore.x, SaitamaStore.y))
    }
}