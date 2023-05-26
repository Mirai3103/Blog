/** @type {import('tailwindcss').Config} */
export default {
    mode: "jit",
    content: ["./Views/**/*.{js,ts,jsx,tsx,html,cshtml}"],
    theme: {
        extend: {
            screens: {
                $ssm: { max: "550px" },
                $sm: { max: "639px" },
                $md: { max: "767px" },
                $lg: { max: "1023px" },
                $xl: { max: "1279px" },
            },
        },
    },
    plugins: [],
};
