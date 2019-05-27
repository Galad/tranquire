git clean -xdf
Set-Location src\ToDoList\ClientApp
npm ci
npm run-script build
Set-Location ..\..\..
dotnet build Tranquire.sln --configuration Release
dotnet test Tranquire.sln --configuration Release
dotnet pack --configuration Release 