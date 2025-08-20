using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bris_API.DTOs
{
    // DTO para criação/edição de Animal
    public class AnimalDto
    {
        [Required(ErrorMessage = "A linhagem é obrigatória.")]
        [StringLength(200, ErrorMessage = "A linhagem deve ter no máximo 200 caracteres.")]
        public required string Linhagem { get; set; }

        [Required(ErrorMessage = "A idade é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "A idade deve ser um valor positivo.")]
        public required int Idade { get; set; }

        [Required(ErrorMessage = "O peso é obrigatório.")]
        [Range(0.1, float.MaxValue, ErrorMessage = "O peso deve ser positivo.")]
        public required float Peso { get; set; }

        // O status inicia como nulo e será atualizado após avaliação
        public bool? Status { get; set; }

        [Required(ErrorMessage = "A granja é obrigatória.")]
        public required int GranjaId { get; set; }

        [Required(ErrorMessage = "O usuário responsável é obrigatório.")]
        public required int UsuarioResponsavelId { get; set; }

        [Required(ErrorMessage = "O estado de ativação é obrigatório.")]
        public required bool Ativo { get; set; }
    }
}
