/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{js,jsx,ts,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        green: {
          DEFAULT: '#21BA45', // Verde de realce
          //DEFAULT: '#4CAF50', // Verde secundario
          light: '#A8D5A2',  // Verde claro
          dark: '#2E7D32',   // Verde escuro
        },
        white: {
          DEFAULT: '#FFFFFF',
        },
        gray: {
          light: '#F7F7F7',
          dark: '#9E9E9E',
        },
      },
    },
  },
  plugins: [],
};