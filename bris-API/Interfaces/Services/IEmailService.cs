namespace bris_API.Services
{
    public interface IEmailService
    {
        Task EnviarEmailRecuperacaoSenha(string email, string novaSenha);
    }
}
