document.addEventListener("DOMContentLoaded", () => {
    const form = document.querySelector("form");

    form.addEventListener("submit", (e) => {
        const email = form.email.value.trim();
        const senha = form.senha.value.trim();

        if (!email || !senha) {
            alert("Por favor, preencha todos os campos!");
            e.preventDefault();
        }
    });
});
