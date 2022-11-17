package com.netproject.client.scenemanager

import com.badlogic.gdx.utils.Disposable
import com.netproject.client.scenemanager.messages.SceneMessage
import com.netproject.client.scenemanager.messages.SwitchSceneMessage
import com.netproject.client.scenemanager.scene.MenuScene
import com.netproject.client.scenemanager.scene.Scene
import java.util.concurrent.ConcurrentLinkedQueue

class SceneManager: Disposable {
    private val sceneMessageQueue: ConcurrentLinkedQueue<SceneMessage> = ConcurrentLinkedQueue()
    private var currentScene: Scene = MenuScene(sceneMessageQueue)

    init {
        currentScene.load()
    }

    fun update() {
        while (!sceneMessageQueue.isEmpty()) {
            when (val message = sceneMessageQueue.poll()) {
                is SwitchSceneMessage -> {
                    currentScene.dispose()
                    currentScene = message.nextScene
                    currentScene.load()
                }
            }
        }
    }

    fun render() {
        currentScene.render()
    }

    fun resize(width: Int, height: Int) {
        currentScene.resize(width, height)
    }

    override fun dispose() {
        currentScene.dispose()
    }
}