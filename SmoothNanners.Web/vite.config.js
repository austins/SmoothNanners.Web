import { defineConfig } from "vite";
import tailwindcssPlugin from "tailwindcss";
import autoprefixerPlugin from "autoprefixer";
import { parse as parseJSONC } from "jsonc-parser";
import { readFileSync } from "node:fs";
import { resolve } from "node:path";
import isSubdir from "is-subdir";

const appsettings = parseJSONC(readFileSync("./appsettings.json", "utf8"));

// Validate config.
if (!isSubdir(process.cwd(), resolve(appsettings.Vite.AssetsDirectoryName))
    || appsettings.Vite.ManifestFileName.includes("/")) {
    throw new Error("Invalid Vite config.");
}

export default defineConfig({
    css: { postcss: { plugins: [tailwindcssPlugin(), autoprefixerPlugin()] } },
    build: {
        outDir: `./wwwroot/${appsettings.Vite.AssetsDirectoryName.toLowerCase()}`,
        emptyOutDir: true,
        manifest: appsettings.Vite.ManifestFileName,
        rollupOptions: {
            input: appsettings.Vite.AssetPaths.map(asset => `./${appsettings.Vite.AssetsDirectoryName}/${asset}`),
            output: {
                assetFileNames: "[name]-[hash][extname]",
                chunkFileNames: "[name]-[hash].js",
                entryFileNames: "[name]-[hash].js"
            }
        }
    }
});
