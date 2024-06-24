import { rm } from "node:fs/promises";

const outdir = "./wwwroot/assets";

// Clean outdir if it exists.
await rm(outdir, { force: true, recursive: true });

// Build JS assets.
await Bun.build({
    entrypoints: ["./Assets/main.ts"],
    outdir,
    splitting: true,
    minify: true
});
