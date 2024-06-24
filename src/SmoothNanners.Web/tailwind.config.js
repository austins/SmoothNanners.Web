/** @type {import("tailwindcss").Config} */
export default {
    content: ["./Pages/**/*.cshtml"],
    theme: {
        container: {
            center: true,
            padding: "1rem",
            screens: { sm: "768px" }
        },
        extend: {
            fontFamily: {
                roundsans: [
                    "ui-rounded",
                    "Hiragino Maru Gothic ProN",
                    "Quicksand",
                    "Comfortaa",
                    "Manjari",
                    "Calibri",
                    "source-sans-pro",
                    "sans-serif"
                ],
                handwritten: ["Segoe Print", "Chilanka", "TSCu_Comic", "casual", "cursive"]
            }
        }
    }
};
