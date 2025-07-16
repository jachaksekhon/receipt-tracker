module.exports = {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}"
  ],
  safelist: [
    'text-secondary',
    'text-primary',
    'bg-primary',
    'bg-secondary',
  ],
  theme: {
    extend: {
      colors: {
        primary: "#39FF14",
        "primary-foreground": "#ffffff",

        secondary: "#fcda00",
        "secondary-foreground": "#ffffff",
      },
      fontFamily: {
        sans: ['Inter', 'sans-serif'],
      }
    },
  },
  plugins: [],
}
