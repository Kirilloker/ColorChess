#!bin/bash
pkill -f -9 'dotnet.*Report.dll'

cd /home/kirillok/ColorChess/ColorChess/ReportBotTelegram/

dotnet build

dotnet /home/kirillok/ColorChess/ColorChess/ReportBotTelegram/bin/Debug/net6.0/Report.dll > /home/kirillok/script/outputTelegramBot.log 2 > &1 &

echo "start Telegramm Bot is done"



