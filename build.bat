cd src
dotnet pack -c Release
cd ..

mkdir dist
copy src\dist\*.nupkg dist\