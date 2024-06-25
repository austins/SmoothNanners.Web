import { defineConfig } from "vite";
import tailwindcssPlugin from "tailwindcss";
import autoprefixerPlugin from "autoprefixer";

export default defineConfig({
    css: { postcss: { plugins: [tailwindcssPlugin(), autoprefixerPlugin()] } },
    build: {
        outDir: `./wwwroot/assets`,
        emptyOutDir: true,
        rollupOptions: {
            input: {
                "main.css": "./Assets/main.css",
                "main": "./Assets/main.ts"
            },
            output: {
                assetFileNames: "[name][extname]",
                chunkFileNames: "[name].js",
                entryFileNames: "[name].js"
            }
        }
    }
});
