###################################################
## Build stage.
###################################################
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine@sha256:c439a8d5fef241d04858145e777a460ec0e6c8fec8e518b41cdee4e37321f306 AS build

# Copy source files.
# The build should be run with the context at the root of the solution in order to copy necessary files.
WORKDIR /app
COPY .editorconfig Directory.Build.props ./
COPY ./src/SmoothNanners.Web ./src/SmoothNanners.Web 

# Publish app.
# Assets must be restored first before building/publishing the entire project so that the static web assets can resolve the new files.
RUN dotnet msbuild -target:RestoreAssets ./src/SmoothNanners.Web
RUN dotnet publish ./src/SmoothNanners.Web -c Release -o ./out

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
