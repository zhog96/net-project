package com.netproject.client.scenemanager.messages

import com.netproject.client.scenemanager.scene.Scene

data class SwitchSceneMessage (
        val nextScene: Scene
) : SceneMessage
