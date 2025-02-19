###################################################
## Build stage.
###################################################
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine@sha256:c439a8d5fef241d04858145e777a460ec0e6c8fec8e518b41cdee4e37321f306 AS build

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
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine@sha256:36b581c9812089fa977395f6951cd08064b10da98b45653ea43e75b8e247e8fa

# Copy published build.
WORKDIR /app
COPY --from=build /app/out .

# Enable globalization.
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8
RUN apk add --no-cache icu-data-full icu-libs

# Expose port.
EXPOSE 8080

# Configure healthcheck.
RUN apk add --no-cache curl
HEALTHCHECK --interval=2m CMD curl --fail http://localhost:8080/_health || exit 1

# Configure running the app.
ENTRYPOINT ["dotnet", "SmoothNanners.Web.dll"]
