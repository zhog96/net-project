package org.netproject

class Player {
    var id: Int? = null
    var x: Double? = null
    var y: Double? = null
    var z: Double? = null
    var rotation: Double? = null

    fun updatePlayer(player: Player) {
        x = player.x
        y = player.y
        z = player.z
        rotation = player.rotation
    }
}