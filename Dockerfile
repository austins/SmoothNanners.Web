###################################################
## Build stage.
###################################################
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine@sha256:871137a2bc06faf9486aac28cf5629dfae5edb5b7126646e873791119ee20d02 AS build

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
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine@sha256:1d9e1eb36eb822e7be487e7a11cd2350529e14e5e91484a08b501c9822867be4

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