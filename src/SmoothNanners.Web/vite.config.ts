import { defineConfig } from "vite";
import tailwindcss from "@tailwindcss/vite";

export default defineConfig({
    plugins: [tailwindcss()],
    base: "/assets/",
    build: {
        assetsDir: "",
        outDir: "./wwwroot/assets",
        rollupOptions: {
            input: {
                app: "./Assets/app.ts",
                "app.css": "./Assets/app.css"
            },
            output: {
                entryFileNames: "[name].js",
                assetFileNames: "[name][extname]"
            }
        }
    }
});
