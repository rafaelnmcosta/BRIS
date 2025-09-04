import React, { useState } from 'react';
import { avaliacoes } from '../api/avaliacoesAPI';

const Teste = () => {
  const [id, setId] = useState(''); // usado para avaliaçãoId / granjaId conforme botão
  const [animalId, setAnimalId] = useState(''); // usado para criar avaliação
  const [valorDose, setValorDose] = useState(''); // para novaDose
  const [resposta, setResposta] = useState(null);
  const [loading, setLoading] = useState(false);

  const executarRequisicao = async (callback) => {
    try {
      setLoading(true);
      const resultado = await callback();
      setResposta(resultado);
    } catch (error) {
      // tentar extrair mensagem do backend
      const msg = error?.response?.data ?? error;
      setResposta({ erro: msg });
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ padding: 20 }}>
      <h1>🔧 Teste de API - Avaliações</h1>

      <div style={{ marginBottom: 8 }}>
        <label style={{ width: 120, display: 'inline-block' }}>ID (avaliação / granja):</label>
        <input value={id} onChange={(e) => setId(e.target.value)} placeholder="ex: 1" />
      </div>

      <div style={{ marginBottom: 8 }}>
        <label style={{ width: 120, display: 'inline-block' }}>Animal ID:</label>
        <input value={animalId} onChange={(e) => setAnimalId(e.target.value)} placeholder="ex: 12" />
      </div>

      <div style={{ marginBottom: 8 }}>
        <label style={{ width: 120, display: 'inline-block' }}>Valor da Dose:</label>
        <input
          value={valorDose}
          onChange={(e) => setValorDose(e.target.value)}
          placeholder="ex: 75.2"
        />
      </div>

      <hr />

      <div style={{ display: 'flex', flexDirection: 'column', gap: 8, marginTop: 12 }}>
        <button onClick={() => executarRequisicao(() => avaliacoes.listarPorGranja(id))}>
          Listar Avaliações por Granja (passar granjaId em ID)
        </button>

        <button onClick={() => executarRequisicao(() => avaliacoes.listarInterrompidas())}>
          Listar Avaliações Interrompidas
        </button>

        <button onClick={() => executarRequisicao(() => avaliacoes.detalhes(id))}>
          Detalhes Avaliação (passar avaliaçãoId em ID)
        </button>

        <button onClick={() => executarRequisicao(() => avaliacoes.novaAvaliacao(animalId))}>
          Nova Avaliação (passar animalId)
        </button>

        <button
          onClick={() =>
            executarRequisicao(() =>
              // seu endpoint aceita body; caso ignore, não tem problema
              avaliacoes.novaAvaliacao(animalId /*, opcional avaliacaoData */)
            )
          }
        >
          Criar Nova Avaliação (server-side gera semanas/doses)
        </button>

        <button
          onClick={() =>
            executarRequisicao(() =>
              avaliacoes.novaDose(id, { ValorRegistrado: parseFloat(valorDose) })
            )
          }
        >
          Registrar Nova Dose (passar avaliaçãoId em ID e Valor na caixa "Valor da Dose")
        </button>

        <button onClick={() => executarRequisicao(() => avaliacoes.finalizar(id))}>
          Finalizar Avaliação (passar avaliaçãoId em ID)
        </button>

        <button onClick={() => executarRequisicao(() => avaliacoes.interromper(id))}>
          Interromper Avaliação (passar avaliaçãoId em ID)
        </button>

        <button onClick={() => executarRequisicao(() => avaliacoes.reativar(id))}>
          Reativar Avaliação (passar avaliaçãoId em ID)
        </button>
      </div>

      <hr style={{ marginTop: 12, marginBottom: 12 }} />

      <div>
        <h3>Resposta:</h3>
        {loading ? (
          <p>Carregando...</p>
        ) : (
          <pre style={{ background: '#f5f5f5', padding: 10, borderRadius: 6, maxHeight: 400, overflow: 'auto' }}>
            {resposta ? JSON.stringify(resposta, null, 2) : 'Nenhuma resposta ainda.'}
          </pre>
        )}
      </div>
    </div>
  );
};

export default Teste;
