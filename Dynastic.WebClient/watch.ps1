
dotnet build -p:TailwindBuild=false
Start-Process "dotnet" -ArgumentList "watch"
npm run watch