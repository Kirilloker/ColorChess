$user = "kirillok"
$images = @("colorchess-site", "colorchess-reportbottelegram", "colorchess-gameserver")

docker login

docker-compose build

foreach ($image in $images) {
	docker tag $image $user/"$image":latest
	docker push $user/"$image":latest

	# docker rmi $image
	docker rmi $user/"$image":latest
}
