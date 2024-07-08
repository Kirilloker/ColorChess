#!/bin/bash

pkill -f "node server.js"
pkill -f "npm start"

bash /home/kirillok/script/getIp.sh
ip_address=$(cat /home/kirillok/script/ip_address.txt)

cd /home/kirillok/ColorChess/ColorChess/Site/React-App/my-react-app/src/components/Top/
sed -i "s/localhost/$ip_address/g" Top.js

cd /home/kirillok/ColorChess/ColorChess/Site/React-App/my-react-app/

PORT=20202 npm start > /home/kirillok/script/outputReact.log 2>&1 &

cd /home/kirillok/ColorChess/ColorChess/Site/Node-Server/

sed -i "s/localhost/$ip_address/g" server.js
sed -i "s/users/Users/g" server.js
sed -i "s/userstatistics/UserStatistics/gI" server.js

node server.js > /home/kirillok/script/outputNode.log  2>&1 &

echo "Start site is done"