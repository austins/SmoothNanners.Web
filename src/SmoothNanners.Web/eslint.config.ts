import js from "@eslint/js";
import globals from "globals";
import ts from "typescript-eslint";
import { defineConfig } from "eslint/config";
import stylistic from "@stylistic/eslint-plugin";
import html from "@html-eslint/eslint-plugin";
import htmlParser from "@html-eslint/parser";
import betterTailwindcss from "eslint-plugin-better-tailwindcss";

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
        plugins: { "@html-eslint": html },
        extends: [betterTailwindcss.configs["recommended-error"]],
        settings: { "better-tailwindcss": { entryPoint: "Assets/app.css" } },
        languageOptions: { parser: htmlParser },
        rules: {
            "@html-eslint/head-order": "error",
            "@html-eslint/no-abstract-roles": "error",
            "@html-eslint/no-accesskey-attrs": "error",
            "@html-eslint/no-aria-hidden-body": "error",
            "@html-eslint/no-aria-hidden-on-focusable": "error",
            "@html-eslint/no-duplicate-attrs": "error",
            "@html-eslint/no-duplicate-id": "error",
            "@html-eslint/no-duplicate-in-head": "error",
            "@html-eslint/no-empty-headings": "error",
            "@html-eslint/no-extra-spacing-text": "error",
            "@html-eslint/no-heading-inside-button": "error",
            "@html-eslint/no-ineffective-attrs": "error",
            "@html-eslint/no-inline-styles": "error",
            "@html-eslint/no-invalid-attr-value": [
                "error",
                {
                    allow: [
                        { tag: "a", attr: "href" },
                        { tag: "time", attr: "datetime" }
                    ]
                }
            ],
            "@html-eslint/no-invalid-entity": "error",
            "@html-eslint/no-invalid-role": "error",
            "@html-eslint/no-multiple-empty-lines": ["error", { max: 1 }],
            "@html-eslint/no-nested-interactive": "error",
            "@html-eslint/no-non-scalable-viewport": "error",
            "@html-eslint/no-obsolete-attrs": "error",
            "@html-eslint/no-obsolete-tags": "error",
            "@html-eslint/no-positive-tabindex": "error",
            "@html-eslint/no-redundant-role": "error",
            "@html-eslint/no-script-style-type": "error",
            "@html-eslint/no-target-blank": "error",
            "@html-eslint/no-trailing-spaces": "error",
            "@html-eslint/require-button-type": "error",
            "@html-eslint/require-details-summary": "error",
            "@html-eslint/require-doctype": "error",
            "@html-eslint/require-form-method": "error",
            "@html-eslint/require-frame-title": "error",
            "@html-eslint/require-img-alt": "error",
            "@html-eslint/require-lang": "error",
            "@html-eslint/require-li-container": "error",
            "@html-eslint/require-meta-charset": "error",
            "@html-eslint/require-meta-viewport": "error",
            "@html-eslint/require-title": "error",
            "better-tailwindcss/enforce-consistent-line-wrapping": [
                "error",
                { group: "newLine", preferSingleLine: true, indent: 4, printWidth: 120 }
            ],
            "better-tailwindcss/no-unknown-classes": ["error", { ignore: ["^@.*$"] }]
        }
    }
]);
