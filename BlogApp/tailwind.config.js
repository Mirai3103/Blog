/** @type {import('tailwindcss').Config} */
export default {
    mode: "jit",
    content: ["./Views/**/*.{js,ts,jsx,tsx,html,cshtml}", "./node_modules/flowbite/**/*.js"],
    theme: {
        extend: {
            screens: {
         
              
            
                $xl: { max: "1279px" }, $lg: { max: "1023px" }, $md: { max: "767px" }, $sm: { max: "639px" }, $ssm: { max: "550px" },

            },
        },
    },
    plugins: [
        require('flowbite/plugin')
    ]
};
