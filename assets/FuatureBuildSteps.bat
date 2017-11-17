REM This is steps for TeamCity build for Feature branches

REM 1) Restore packages
REM ! dotnet restore N2N.sln /p:Configuration=Release /p:Platform="Any CPU"

REM 2) NuGet restore
REM TeamCity configurable step for NuGet 
REM NuGet version 4.1.0
REM Update via solution file

REM 3) Build solution
REM ! dotnet msbuild N2N.sln /p:Configuration=Release /p:Platform="Any CPU"

REM 4) Backend tests
REM TeamCity configurable step for NUnit
REM NUnit 3 runner
REM path to tools packages\NUnit.ConsoleRunner.3.7.0\tools\nunit3-console.exe
REM additional command line parameter --x86
REM Run tests from:  [list of compiled .dll for test projects ]

REM 5) Restore FrontEnd packages
REM Set working direcory for frontend app
REM ! npm i -g typescript
REM ! npm i -g @angular/cli
REM ! npm i -g webpack
REM ! npm i

REM 6) FrontEnd tests
REM Set working direcory for frontend app
REM ! node node_modules/karma/bin/karma start --single-run