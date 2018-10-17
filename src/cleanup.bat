@ECHO OFF
PROMPT $

REM ========================================================================
ECHO ### Clean up WDNUtils.Common

IF EXIST ".\WDNUtils.Common\WDNUtils.Common.csproj.user" DEL /A /F /Q ".\WDNUtils.Common\WDNUtils.Common.csproj.user"
IF EXIST ".\WDNUtils.Common\bin" DEL /A /F /S /Q ".\WDNUtils.Common\bin"
IF EXIST ".\WDNUtils.Common\bin" RMDIR /S /Q ".\WDNUtils.Common\bin"
IF EXIST ".\WDNUtils.Common\obj" DEL /A /F /S /Q ".\WDNUtils.Common\obj"
IF EXIST ".\WDNUtils.Common\obj" RMDIR /S /Q ".\WDNUtils.Common\obj"

REM ========================================================================
ECHO ### Clean up WDNUtils.DBOracle

IF EXIST ".\WDNUtils.DBOracle\WDNUtils.DBOracle.csproj.user" DEL /A /F /Q ".\WDNUtils.DBOracle\WDNUtils.DBOracle.csproj.user"
IF EXIST ".\WDNUtils.DBOracle\bin" DEL /A /F /S /Q ".\WDNUtils.DBOracle\bin"
IF EXIST ".\WDNUtils.DBOracle\bin" RMDIR /S /Q ".\WDNUtils.DBOracle\bin"
IF EXIST ".\WDNUtils.DBOracle\obj" DEL /A /F /S /Q ".\WDNUtils.DBOracle\obj"
IF EXIST ".\WDNUtils.DBOracle\obj" RMDIR /S /Q ".\WDNUtils.DBOracle\obj"

REM ========================================================================
ECHO ### Clean up vistual studio generated files

IF EXIST ".\.vs\" DEL /A /F /S /Q ".\.vs"
IF EXIST ".\.vs\" RMDIR /S /Q ".\.vs"
IF EXIST ".\TestResults\" DEL /A /F /S /Q ".\TestResults"
IF EXIST ".\TestResults\" RMDIR /S /Q ".\TestResults"

ECHO ### Finished!
PAUSE
EXIT /B
