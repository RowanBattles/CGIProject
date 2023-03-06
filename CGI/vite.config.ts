import { defineConfig } from 'vite';

export default defineConfig({
    build: {
        outDir: './wwwroot/build',
        emptyOutDir: true,
        manifest: true,
        rollupOptions: {
            input: {
                site: './wwwroot/js/site.js',
            },
        },
    },
});
