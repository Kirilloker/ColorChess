@echo off

set "source=bin\Debug\net471\ColorChessModel.dll"

set "destination=..\Demo_2\Assets\Plugins"

copy "%source%" "%destination%"

pause
