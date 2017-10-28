
@echo off

set H=R:\KSP_1.3.1_dev
set GAMEDIR=000_FilterExtensions
set GAMEDIR2="000_FilterExtensions_Configs"

echo %H%

copy /Y "%1%2" "GameData\%GAMEDIR%\Plugins"

mkdir "%H%\GameData\%GAMEDIR%"
xcopy  /E /y /I GameData\%GAMEDIR% "%H%\GameData\%GAMEDIR%"

xcopy  /E /y /I  GameData\%GAMEDIR2% "%H%\GameData\%GAMEDIR2%"

