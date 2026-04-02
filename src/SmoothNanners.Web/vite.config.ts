import { defineConfig } from "vite";
import tailwindcss from "@tailwindcss/vite";

export default defineConfig({
    plugins: [tailwindcss()],
    build: {
        outDir: "wwwroot/assets",
        assetsDir: ".",
        emptyOutDir: true,
        rolldownOptions: {
            input: "Assets/app.ts",
            output: {
                entryFileNames: "[name].js",
                chunkFileNames: "[name]-[hash].js",
                assetFileNames: "[name][extname]"
            }
        }
    }
});
