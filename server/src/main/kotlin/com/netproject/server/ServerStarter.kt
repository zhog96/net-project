package com.netproject.server
import org.springframework.boot.autoconfigure.SpringBootApplication
import org.springframework.boot.runApplication
import org.springframework.scheduling.annotation.EnableScheduling

@SpringBootApplication
@EnableScheduling
class Server

fun main(args: Array<String>) {
    runApplication<Server>(*args)
}