package org.example;

public class Player {
    Integer id;
    Double x, y, z;
    Double rotation;

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public Double getX() {
        return x;
    }

    public void setX(Double x) {
        this.x = x;
    }

    public Double getY() {
        return y;
    }

    public void setY(Double y) {
        this.y = y;
    }

    public Double getZ() {
        return z;
    }

    public void setZ(Double z) {
        this.z = z;
    }

    public Double getRotation() {
        return rotation;
    }

    public void setRotation(Double rotation) {
        this.rotation = rotation;
    }

    public void updatePlayer(Player player) {
        this.x = player.x;
        this.y = player.y;
        this.z = player.z;
        this.rotation = player.rotation;
    }
}
