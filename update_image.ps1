$envFile = ".env"
Get-Content $envFile | ForEach-Object {
    if ($_ -match "^\s*([^=]+)\s*=\s*(.+?)\s*$") {
        [System.Environment]::SetEnvironmentVariable($matches[1], $matches[2])
    }
}

$user = $env:USER

$images = @("colorchess-site", "colorchess-reportbottelegram", "colorchess-gameserver")

docker login
docker-compose build

foreach ($image in $images) {
    docker tag $image "$user/$image:latest"
    docker push "$user/$image:latest"
    docker rmi "$user/$image:latest"
}
