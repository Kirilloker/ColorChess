#!bin/bash
pkill -f -9 'dotnet.*GameServer.dll'
cd /home/kirillok/ColorChess/ColorChess/GameServer/

dotnet build

cd bin/Debug/net6.0
dotnet GameServer.dll > /home/kirillok/script/outputGameServer.log 2>&1 &

echo "start game server is done"