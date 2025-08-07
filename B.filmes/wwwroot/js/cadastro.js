// cadastro.js

document.getElementById("formCadastro").addEventListener("submit", function (event) {
    event.preventDefault(); // Evita envio real

    let nome = document.getElementById("nome").value.trim();
    let email = document.getElementById("email").value.trim();
    let senha = document.getElementById("senha").value.trim();
    //let tipo = document.getElementById("tipo").value;

    if (!nome || !email || !senha /*|| !tipo*/) {
        document.getElementById("mensagem").style.color = "red";
        document.getElementById("mensagem").textContent = "Preencha todos os campos!";
        return;
    }

    document.getElementById("mensagem").style.color = "green";
    document.getElementById("mensagem").textContent = "Usuário cadastrado com sucesso!";

    // Aqui você pode chamar o backend via AJAX (se quiser)
});
