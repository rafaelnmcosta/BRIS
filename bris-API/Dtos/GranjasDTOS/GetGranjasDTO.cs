using System.ComponentModel.DataAnnotations;

namespace bris_API.DTOs
{
    public class GetGranjaDTO
    {
        public int Id { get; set; }
        public string NomePropriedade { get; set; }
        public string Endereco { get; set; }
        public string CNPJ { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
        public GetAgroindustriaDTO Agroindustria { get; set; }
    }
}