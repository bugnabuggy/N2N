REM This is steps for TeamCity build for Feature branches

REM 1) Restore packages
dotnet restore N2N.sln /p:Configuration=Release 

REM 2) NuGet restore
REM TeamCity configurable step for NuGet 
REM NuGet version 4.1.0
REM Update via solution file

REM 3) Build solution
REM ! dotnet msbuild N2N.sln /p:Configuration=Release 

REM 4) Backend tests
REM TeamCity configurable step for NUnit
REM NUnit 3 runner
REM path to tools packages\NUnit.ConsoleRunner.3.7.0\tools\nunit3-console.exe
REM additional command line parameter --x86
REM Run tests from:  [list of compiled .dll for test projects ]

REM 5) Restore FrontEnd packages
REM Set working direcory for frontend app
REM ! npm i

REM 6) FrontEnd tests
REM Set working direcory for frontend app
REM ! node node_modules/karma/bin/karma start --single-run

REM 7) Build FrontEnd App
REM Set working direcory for frontend app
REM ! ng build --aot

REM 8) Publish website sources
REM ! rmdir /s /q N2N\pub
REM ! dotnet msbuild N2N.sln /t:Publish /p:Configuration=Release /p:PublishDir=pub

REM 9) Stop site, Copy files to web site, and start again
REM ! %env.windir%\system32\inetsrv\appcmd stop site /site.name:api.nick2nick.com
REM ! %env.windir%\system32\inetsrv\appcmd stop site /site.name:nick2nick.com
REM ! %env.windir%\system32\inetsrv\appcmd stop apppool /apppool.name:nick2nick.com
REM ! timeout 5 /nobreak
REM taskkill /im N2N.Web.exe /F
REM ! rmdir /s /q C:\WebSites\api.nick2nick.com
REM ! rmdir /s /q C:\WebSites\nick2nick.com
REM del /s /q C:\WebSites\lms.ruteco.com\*
REM ! md C:\WebSites\nick2nick.com
REM ! md C:\WebSites\api.nick2nick.com
REM ! xcopy src\N2N.Api\pub C:\WebSites\api.nick2nick.com /e /Y
REM ! xcopy src\N2N.Web\pub C:\WebSites\nick2nick.com /e /Y
REM ! timeout 3 /nobreak
REM ! %env.windir%\system32\inetsrv\appcmd start apppool /apppool.name:nick2nick.com
REM ! %env.windir%\system32\inetsrv\appcmd start site /site.name:api.nick2nick.com
REM ! %env.windir%\system32\inetsrv\appcmd start site /site.name:nick2nick.com