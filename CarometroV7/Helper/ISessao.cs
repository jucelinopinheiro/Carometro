using CarometroV7.Models;
namespace CarometroV7.Helper
{
    public interface ISessao
    {
        void CriarSessao(Usuario userModell);
        void RemoverSessao();
        Usuario BuscaSessao();
    }
}
