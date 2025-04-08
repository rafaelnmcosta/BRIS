using System.ComponentModel.DataAnnotations;

namespace bris_API.DTOs
{
    public class GetAgroindustriaDTO
    {
        public int Id { get; set; }
        public string NomeFantasia { get; set; }
        public string RazaoSocial { get; set; }
        public string CNPJ { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
    }
}