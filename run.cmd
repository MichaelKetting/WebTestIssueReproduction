@echo off
pushd %~dp0
set msbuild="C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"
set log-dir=build\BuildOutput\log
set nuget-bin=build\BuildOutput\temp\nuget-bin
set nuget=%nuget-bin%\nuget.exe
set nunit="packages\NUnit.Runners.2.6.3\tools\nunit-console-x86.exe"
set nuget-download=powershell.exe -NoProfile -Command "& {(New-Object System.Net.WebClient).DownloadFile('https://www.nuget.org/nuget.exe','%nuget%')}"
mkdir %nuget-bin%
%nuget-download%
%nuget% restore WebTestIssueReproduction.sln -NonInteractive
%msbuild% WebTestIssueReproduction.sln /v:quiet
if not %ERRORLEVEL%==0 goto build_failed
%nunit% Tests\bin\Debug\Tests.dll /framework:net-4.5 /nologo /nodots
goto build_succeeded

:build_failed
echo.
echo Building has failed.
pause
popd
exit /b 1

:build_succeeded
echo.
pause
popd
exit /b 0
