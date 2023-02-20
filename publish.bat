ECHO OFF

ECHO -- Pulling newest version from github:

FOR /F "tokens=* USEBACKQ" %%F IN (`git pull`) DO (
SET var=%%F
)

ECHO --Pull result: %var%
IF "%var%" == "Already up to date." (
 ECHO -- no further action required
)
ECHO -- Changing directory
cd Gryzilla-App

ECHO -- Publishing Gryzilla to local files
dotnet publish --output ./../Published --runtime win-x64 -p:PublishSingleFile=true --self-contained true --configuration Release


PAUSE