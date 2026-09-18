@echo off
dotnet publish "%~dp0app\Writefix.csproj" -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -o "%~dp0dist"
if errorlevel 1 exit /b 1
echo.
echo Built: %~dp0dist\Writefix.exe
