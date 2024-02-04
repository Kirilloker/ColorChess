@echo off

mkdir Test

set "source=bin\Debug\net6.0\ColorChessModel.dll"

set "destination=..\Demo_2\Assets\Plugins"

copy "%source%" "%destination%"

pause
