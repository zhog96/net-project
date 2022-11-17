package com.netproject.client

import com.badlogic.gdx.ApplicationListener
import com.badlogic.gdx.Gdx
import com.badlogic.gdx.Input
import com.badlogic.gdx.Input.*
import com.badlogic.gdx.Input.Keys.*
import com.badlogic.gdx.graphics.GL20.GL_COLOR_BUFFER_BIT
import com.badlogic.gdx.graphics.Texture
import com.badlogic.gdx.graphics.g2d.Batch
import com.badlogic.gdx.graphics.g2d.SpriteBatch
import com.badlogic.gdx.graphics.g2d.TextureRegion
import com.fasterxml.jackson.databind.ObjectMapper
import com.netproject.client.scenemanager.SceneManager
import org.springframework.messaging.converter.MappingJackson2MessageConverter
import org.springframework.messaging.simp.stomp.StompSession
import org.springframework.messaging.simp.stomp.StompSessionHandler
import org.springframework.web.socket.client.WebSocketClient
import org.springframework.web.socket.client.standard.StandardWebSocketClient
import org.springframework.web.socket.messaging.WebSocketStompClient


class Application : ApplicationListener {
    private lateinit var sceneManager: SceneManager

    override fun create() {
        sceneManager = SceneManager()
    }

    override fun resize(width: Int, height: Int) {
        sceneManager.resize(width, height)
    }

    override fun render() {
        sceneManager.update()
        sceneManager.render()
    }

    override fun pause() {
    }

    override fun resume() {
    }

    override fun dispose() {
    }
}