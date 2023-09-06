/** @type {import('tailwindcss').Config} */

import { colors as defaultColors } from 'tailwindcss/defaultTheme'
import tailwindColors from 'tailwindcss/colors'

const colors = {
  ...defaultColors,
  ...{
    "custom-gray": '#ccc',
    "main-background": tailwindColors.gray[300],
    "main-background-dark": '#121212',
    "secondary-background": tailwindColors.white,
    "secondary-backgroud-dark": '#161616',
    "main-font":tailwindColors.black,
    "main-font-dark" : tailwindColors.white,
    "button" : tailwindColors.green[700],
    "button-hover" : tailwindColors.green[500],
    "skeleton-dark": '#27272a'
  },
}

export default {
  content: [
    './pages/**/*.{ts,tsx}',
    './components/**/*.{ts,tsx}',
    './app/**/*.{ts,tsx}',
    './src/**/*.{ts,tsx}',
    "./index.html",
  ],
  darkMode: 'class',
  theme: {
    container: {
      center: true,
      padding: "2rem",
      screens: {
        "2xl": "1400px",
      },
    },
    extend: {
      colors: colors,
      keyframes: {
        "accordion-down": {
          from: { height: 0 },
          to: { height: "var(--radix-accordion-content-height)" },
        },
        "accordion-up": {
          from: { height: "var(--radix-accordion-content-height)" },
          to: { height: 0 },
        },
      },
      animation: {
        "accordion-down": "accordion-down 0.2s ease-out",
        "accordion-up": "accordion-up 0.2s ease-out",
      },
    }
  },
  plugins: [require("tailwindcss-animate")]
}