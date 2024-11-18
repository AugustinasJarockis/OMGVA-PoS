// vite.config.ts

import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    port: 3000, // Vite dev server port
    strictPort: true,
    proxy: {
      '/api': {
        target: 'http://localhost:9900', // Backend server URL
        changeOrigin: true,
        secure: false, // Set to true if using HTTPS with valid certificates
        // Remove the rewrite to maintain the '/api' prefix
        rewrite: (path) => path.replace(/^\/api/, ''),
      },
    },
  },
  build: {
    outDir: 'dist', // Output directory for production build
  },
})
