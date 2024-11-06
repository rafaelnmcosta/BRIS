using bris_API.Models;

namespace bris_API.Services
{
    public class ResultsService : IResultsService
    {
        private int GeraResultadoSemana(Semana semana)
        {
            // Obtém a dose de 120h
            var dose120h = semana.Doses.FirstOrDefault(d => d.Ordem == 2);
            
            // Obtém a dose de 168h
            var dose168h = semana.Doses.FirstOrDefault(d => d.Ordem == 3);

            //Caso uma das doses seja nula retorna zero para indicar erro
            if (dose120h == null || dose168h == null) return 0;

            // Verifica o PMP na dose de 120h
            if (dose120h.ValorRegistrado < 60)
            {
                // retorna código para "Maior" caso o PMP às 120h seja menor que 60%;
                return 3;
            }

            // Verifica o PMP na dose de 120h e na dose de 168h
            if (dose120h.ValorRegistrado >= 60 && dose168h.ValorRegistrado < 60)
            {
                // retorna código para "Médio"  caso o PMP às 120h seja maior ou igual a 60% e às 168h seja menor que 60%;
                return 2;
            }

            if (dose168h.ValorRegistrado >= 60)
            {
                // retorna código para "Menor" caso o PMP às 168h seja maior ou igual a 60%;
                return 1; 
            }

            // Retorna código para erro se nenhum dos critérios for atendido
            return 0;
        }


        private bool GeraResultadoFinal(Avaliacao avaliacao)
        {
            // Conta quantas semanas obtiveram resultado de sensibilidade "Maior"
            int semanasComResultadoMaior = avaliacao.Semanas.Count(s => s.Resultado == 3);

            // Retorna false se pelo menos 3 semanas têm resultado "Maior"
            return semanasComResultadoMaior < 3;
        }

        private Avaliacao AtualizaProximaDose(Avaliacao avaliacao)
        {
            var proximaOrdem = avaliacao.ProximaDoseOrdem + 1;

            // Caso complete as doses da semana, inicia uma nova semana e gera o resultado da semana fechada
            if (proximaOrdem > 3)
            {
                proximaOrdem = 1;
                var semanaAtual = avaliacao.Semanas
                    .FirstOrDefault(s => s.NroSemana == avaliacao.ProximaDoseSemana);

                // Verifica se a semanaAtual não é nula antes de calcular o resultado
                if (semanaAtual != null)
                {
                    semanaAtual.Resultado = GeraResultadoSemana(semanaAtual);
                }

                // Atualiza o número da semana para a próxima
                avaliacao.ProximaDoseSemana++;
            }

            // Caso complete as 5 semanas, finaliza a avaliação
            if (avaliacao.ProximaDoseSemana > 5)
            {
                avaliacao.ProximaDoseSemana = -1;
                avaliacao.ProximaDoseOrdem = -1;
                avaliacao.StatusAvaliacao = 2;
                avaliacao.ResultadoFinal = GeraResultadoFinal(avaliacao);
            }
            else
            {
                // Atualiza a ordem da dose
                avaliacao.ProximaDoseOrdem = proximaOrdem;

                // Obtém a próxima dose
                var proximaDose = avaliacao.Semanas
                    .FirstOrDefault(s => s.NroSemana == avaliacao.ProximaDoseSemana)?
                    .Doses.FirstOrDefault(d => d.Ordem == avaliacao.ProximaDoseOrdem);

                // Marca a próxima dose como preenchível, se existir
                if (proximaDose != null)
                {
                    proximaDose.PodePreencher = true;
                }
            }

            return avaliacao;
        }

        public Avaliacao ProcessaAvaliacao(Avaliacao avaliacao)
        {
            return AtualizaProximaDose(avaliacao);
        }
    }
}
