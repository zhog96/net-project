package com.netproject.client.scenemanager.scene

import com.badlogic.gdx.Gdx
import com.badlogic.gdx.Input.Keys.*
import com.badlogic.gdx.graphics.GL20.GL_COLOR_BUFFER_BIT
import com.badlogic.gdx.graphics.Texture
import com.badlogic.gdx.graphics.g2d.Batch
import com.badlogic.gdx.graphics.g2d.SpriteBatch
import com.badlogic.gdx.graphics.g2d.TextureRegion
import com.fasterxml.jackson.databind.ObjectMapper
import com.netproject.client.SaitamaPosition
import com.netproject.client.StompSessionHandlerImpl
import com.netproject.client.scenemanager.messages.SceneMessage
import com.netproject.client.scenemanager.messages.SwitchSceneMessage
import org.springframework.messaging.converter.MappingJackson2MessageConverter
import org.springframework.messaging.simp.stomp.StompSession
import org.springframework.messaging.simp.stomp.StompSessionHandler
import org.springframework.web.socket.client.WebSocketClient
import org.springframework.web.socket.client.standard.StandardWebSocketClient
import org.springframework.web.socket.messaging.WebSocketStompClient
import java.util.*
import kotlin.math.abs

class SaitamaScene(private val queue: Queue<SceneMessage>) : Scene {
    private lateinit var batch: Batch
    private lateinit var saitamaPosition: SaitamaPosition
    private lateinit var textureRegion: TextureRegion
    private lateinit var session: StompSession

    override fun load() {
        saitamaPosition = SaitamaPosition(-1000f, -1000f)
        batch = SpriteBatch()
        textureRegion = TextureRegion(Texture("saitama.png"))

        val client: WebSocketClient = StandardWebSocketClient()
        val stompClient = WebSocketStompClient(client)
        val converter = MappingJackson2MessageConverter()
        converter.objectMapper = ObjectMapper()
        stompClient.messageConverter = converter
        val sessionHandler: StompSessionHandler = StompSessionHandlerImpl(saitamaPosition)
        session = stompClient.connect("ws://130.193.48.74:8080/gateway", sessionHandler).get()
    }

    override fun render() {
        Gdx.gl.glClearColor(0f, 0.5f, 0f, 1f)
        Gdx.gl.glClear(GL_COLOR_BUFFER_BIT)

        val deltaTime = Gdx.graphics.deltaTime
        val timeScale = 500f
        var dx = 0f
        var dy = 0f

        if (Gdx.input.isKeyPressed(UP) || Gdx.input.isKeyPressed(W)) {
            dy += deltaTime * timeScale
        }
        if (Gdx.input.isKeyPressed(LEFT) || Gdx.input.isKeyPressed(A)) {
            dx -= deltaTime * timeScale
        }
        if (Gdx.input.isKeyPressed(DOWN) || Gdx.input.isKeyPressed(S)) {
            dy -= deltaTime * timeScale
        }
        if (Gdx.input.isKeyPressed(RIGHT) || Gdx.input.isKeyPressed(D)) {
            dx += deltaTime * timeScale
        }
        if (Gdx.input.isKeyPressed(ESCAPE)) {
            queue.add(SwitchSceneMessage(MenuScene(queue)))
        }

        if (abs(dx) > 1e-5 || abs(dy) > 1e-5) {
            session.send("/app/update", SaitamaPosition(dx, dy))
        }

        batch.begin()
        batch.draw(
                textureRegion,
                saitamaPosition.x,
                saitamaPosition.y,
                textureRegion.regionWidth / 2f,
                textureRegion.regionHeight / 2f,
                textureRegion.regionWidth + 0f,
                textureRegion.regionHeight + 0f,
                0.3f,
                0.3f,
                0f
        )
        batch.end()
    }

    override fun resize(width: Int, height: Int) {
    }

    override fun dispose() {
        session.disconnect()
    }
}