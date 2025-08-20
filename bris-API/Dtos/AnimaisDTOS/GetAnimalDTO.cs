using System.ComponentModel.DataAnnotations;

namespace bris_API.DTOs
{
    public class GetAnimalDTO
    {
        public int Id { get; set; }
        public string Linhagem { get; set; } = string.Empty;
        public int Idade { get; set; }
        public float Peso { get; set; }
        public bool? Status { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }

        // Relação com Granja
        public GetGranjaDTO? Granja { get; set; }

        // Relação com Usuário responsável
        public GetUsuarioDTO? UsuarioResponsavel { get; set; }

        // Avaliações do animal
        public ICollection<GetAvaliacaoDTO>? Avaliacoes { get; set; }
    }
}