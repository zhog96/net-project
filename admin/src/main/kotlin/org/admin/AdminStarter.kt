package org.admin

import de.codecentric.boot.admin.server.config.EnableAdminServer
import org.springframework.boot.autoconfigure.SpringBootApplication
import org.springframework.boot.runApplication

@SpringBootApplication
@EnableAdminServer
class AdminStarter

fun main(args: Array<String>) {
    runApplication<AdminStarter>(*args)
}