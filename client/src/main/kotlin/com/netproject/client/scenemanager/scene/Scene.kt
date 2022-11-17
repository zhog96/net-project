package com.netproject.client.scenemanager.scene

import com.badlogic.gdx.utils.Disposable

interface Scene: Disposable {
    fun load()
    fun render()
    fun resize(width: Int, height: Int)
}