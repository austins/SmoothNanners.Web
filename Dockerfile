###################################################
## Build stage.
###################################################
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine@sha256:cdc618c61fb14b297986a06ea895efe6eb49e30dfe3b2b6c8b4793e600a5f298 AS build

# Publish app.
WORKDIR /app
COPY . .
RUN dotnet publish ./src/SmoothNanners.Web -c Release -o ./out

# Remove .br and .gz files from the published wwwroot directory
# because the app isn't using MapStaticAssets and compression is handled by the reverse proxy.
RUN find ./out/wwwroot -type f \( -name "*.br" -o -name "*.gz" \) -delete

###################################################
## Runtime image creation stage.
###################################################
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine@sha256:c9ff08a26e41b5e271cd7d1942f9f807be9d0eefdd100844f75b70bdb0c2fafd

# Copy published build.
WORKDIR /app
COPY --from=build /app/out .

# Enable globalization.
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8
RUN apk add --no-cache icu-data-full icu-libs

# Configure running the app.
ENTRYPOINT ["dotnet", "SmoothNanners.Web.dll"]
