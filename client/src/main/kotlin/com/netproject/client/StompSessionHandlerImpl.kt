package com.netproject.client

import org.springframework.messaging.simp.stomp.StompCommand
import org.springframework.messaging.simp.stomp.StompHeaders
import org.springframework.messaging.simp.stomp.StompSession
import org.springframework.messaging.simp.stomp.StompSessionHandler
import java.lang.reflect.Type

class StompSessionHandlerImpl(private val saitamaPosition: SaitamaPosition) : StompSessionHandler {
    override fun getPayloadType(headers: StompHeaders): Type = SaitamaPosition::class.java

    override fun handleFrame(headers: StompHeaders, payload: Any?) {
        val msg = payload as SaitamaPosition
        saitamaPosition.x = msg.x
        saitamaPosition.y = msg.y
    }

    override fun afterConnected(session: StompSession, connectedHeaders: StompHeaders) {
        session.subscribe("/topic/saitama/", this)
    }

    override fun handleException(session: StompSession, command: StompCommand?, headers: StompHeaders, payload: ByteArray, exception: Throwable) {
        println(exception.message)
    }

    override fun handleTransportError(session: StompSession, exception: Throwable) {
        println(exception.message)
    }
}