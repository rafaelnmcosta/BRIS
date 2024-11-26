/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{js,jsx,ts,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        green: {
          DEFAULT: '#21AE44', // Verde de realce
          // DEFAULT: '#4CAF00', // Verde de realce
          // DEFAULT: '#C9D6BB', // Verde clariiin
          light: '#A8E6A1',  // Verde claro
          dark: '#2E7D32',   // Verde escuro
        },
        white: {
          DEFAULT: '#FFFFFF',
        },
        gray: {
          light: '#F7F7F7',
          dark: '#9E9E9E',
        },
        'background-custom': '#BFDAAA',
      },
    },
  },
  plugins: [],
};