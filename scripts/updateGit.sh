#!bin/bash

cd /home/kirillok/ColorChess/ColorChess/
git reset --hard HEAD
git pull origin main

bash /home/kirillok/script/startGameServer.sh
bash /home/kirillok/script/startSite.sh
bash /home/kirillok/script/startTelegramBot.sh

echo "UpdateGit is done"
