import js from "@eslint/js";
import globals from "globals";
import ts from "typescript-eslint";
import { defineConfig } from "eslint/config";
import betterTailwindcss from "eslint-plugin-better-tailwindcss";
import htmlParser from "@html-eslint/parser";
import stylistic from "@stylistic/eslint-plugin";

export default defineConfig([
    {
        files: ["*.config.ts", "Assets/**/*.{js,cjs,mjs,ts,cts,mts}"],
        plugins: { js },
        extends: [
            "js/recommended",
            ts.configs.recommended,
            stylistic.configs.customize({
                indent: 4,
                quotes: "double",
                semi: true,
                jsx: false,
                arrowParens: false,
                braceStyle: "1tbs",
                commaDangle: "never"
            })
        ],
        languageOptions: { globals: globals.browser },
        rules: {
            "@stylistic/array-bracket-newline": ["error", "consistent"],
            "@stylistic/array-element-newline": ["error", { consistent: true, multiline: true }],
            "@stylistic/object-curly-newline": ["error", { consistent: true, multiline: true }],
            "@stylistic/object-property-newline": ["error", { allowAllPropertiesOnSameLine: true }]
        }
    },
    {
        files: ["Pages/**/*.cshtml"],
        extends: [betterTailwindcss.configs["recommended-error"]],
        settings: { "better-tailwindcss": { entryPoint: "Assets/app.css" } },
        languageOptions: { parser: htmlParser },
        rules: {
            "better-tailwindcss/enforce-consistent-line-wrapping": [
                "error",
                { group: "newLine", preferSingleLine: true, indent: 4, printWidth: 120 }
            ],
            "better-tailwindcss/no-unknown-classes": ["error", { ignore: ["^@.*$"] }]
        }
    }
]);
