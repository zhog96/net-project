package org.netproject
import org.springframework.boot.autoconfigure.SpringBootApplication
import org.springframework.boot.runApplication
import org.springframework.scheduling.annotation.EnableScheduling

@SpringBootApplication
@EnableScheduling
class AppStarter

fun main(args: Array<String>) {
    runApplication<AppStarter>(*args)
}