###################################################
## Build stage.
###################################################
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine@sha256:1e2d0919b9489be1306a955bc506b74a95fd0c5fb9cb8bd58b96bea82d600152 AS build

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
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine@sha256:4c3901bfc7d61d3e20e68f2034688dc83ac0c79d6c2b0e28eddd4b4ac0b9f5f4

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
