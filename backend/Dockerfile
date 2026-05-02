# 1. IMAGEN DE COMPILACIÓN (SDK)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# 2. COPIAR LA SOLUCIÓN Y EL PROYECTO
COPY *.sln ./
COPY YourCurlyCareApi/*.csproj ./YourCurlyCareApi/

# 3. RESTAURAR DEPENDENCIAS
RUN dotnet restore

# 4. COPIAR TODO EL CÓDIGO Y PUBLICAR
COPY . .
WORKDIR "/app/YourCurlyCareApi"
RUN dotnet publish -c Release -o /app/out

# 5. IMAGEN DE EJECUCIÓN 
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# 6. ARRANCAR LA APP
EXPOSE 8080
ENTRYPOINT ["dotnet", "YourCurlyCareApi.dll"]