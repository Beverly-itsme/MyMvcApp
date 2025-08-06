using Microsoft.AspNetCore.Mvc;
using B.filmes.Models;
using BibliotecaFilmes.Data;

namespace BibliotecaFilmes.Controllers
{
    public class LoginController : Controller
    {
        private readonly UsuarioDAO _usuarioDAO;

        public LoginController(IConfiguration configuration)
        {
            _usuarioDAO = new UsuarioDAO(configuration);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string email, string senha)
        {
            var usuario = _usuarioDAO.Login(email, senha);
            if (usuario != null)
            {
                if (usuario.Tipo == "admin")
                    return RedirectToAction("MenuAdmin", "Home");
                else
                    return RedirectToAction("MenuUsuario", "Home");
            }

            ViewBag.Erro = "Usuário ou senha inválidos!";
            return View();
        }
    }
}
