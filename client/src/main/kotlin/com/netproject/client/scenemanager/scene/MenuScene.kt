package com.netproject.client.scenemanager.scene

import com.badlogic.gdx.Gdx
import com.badlogic.gdx.graphics.GL20
import com.badlogic.gdx.graphics.Texture
import com.badlogic.gdx.graphics.g2d.TextureRegion
import com.badlogic.gdx.scenes.scene2d.Actor
import com.badlogic.gdx.scenes.scene2d.Event
import com.badlogic.gdx.scenes.scene2d.InputEvent
import com.badlogic.gdx.scenes.scene2d.Stage
import com.badlogic.gdx.scenes.scene2d.ui.Button
import com.badlogic.gdx.scenes.scene2d.ui.Table
import com.badlogic.gdx.scenes.scene2d.ui.TextButton.TextButtonStyle
import com.badlogic.gdx.scenes.scene2d.utils.ClickListener
import com.badlogic.gdx.scenes.scene2d.utils.TextureRegionDrawable
import com.netproject.client.scenemanager.messages.SceneMessage
import com.netproject.client.scenemanager.messages.SwitchSceneMessage
import java.util.*


class MenuScene(private val queue: Queue<SceneMessage>) : Scene {
    private var stage: Stage? = null

    override fun load() {
        stage = Stage()
        Gdx.input.inputProcessor = stage
        val table = Table()
        table.setFillParent(true)
        stage?.addActor(table)

        val buttonUpRegion = TextureRegion(Texture("egg.png"))
        val buttonDownRegion = TextureRegion(Texture("saitama.png"))
        val style = TextButtonStyle()
        style.up = TextureRegionDrawable(buttonUpRegion)
        style.down = TextureRegionDrawable(buttonDownRegion)
        val button = Button(style)
        button.addListener(
                object : ClickListener() {
                    override fun clicked(event: InputEvent?, x: Float, y: Float) {
                        queue.add(SwitchSceneMessage(SaitamaScene(queue)))
                    }
                }
        )
        table.add(button)
    }

    override fun render() {
        Gdx.gl.glClearColor(0f, 0.5f, 0f, 1f)
        Gdx.gl.glClear(GL20.GL_COLOR_BUFFER_BIT)
        stage?.act(Gdx.graphics.deltaTime)
        stage?.draw()
    }

    override fun resize(width: Int, height: Int) {
        stage?.viewport?.update(width, height, true);
    }

    override fun dispose() {
        stage?.dispose()
    }
}