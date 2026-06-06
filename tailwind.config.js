/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/WebUI/Views/**/*.cshtml",
    "./src/WebUI/Pages/**/*.cshtml",
    "./src/WebUI/wwwroot/**/*.js"
  ],
  theme: {
    extend: {
      fontFamily: {
        sans: ['Vazirmatn', 'ui-sans-serif', 'system-ui'],
      },
      colors: {
        'brand-primary': '#4f46e5', // Indigo 600
        'brand-secondary': '#10b981', // Emerald 500
        'brand-accent': '#f59e0b', // Amber 500
      }
    },
  },
  plugins: [
    require('tailwindcss-rtl'),
  ],
}
