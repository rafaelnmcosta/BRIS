import React, { useState } from 'react';
import Tabela from '../organisms/Tabela';
import BotaoPrimario from '../atoms/BotaoPrimario';
import { useNavigate } from 'react-router-dom';

const TemplateTabela = ({ tipo, lista }) => {
  const [showModal, setShowModal] = useState(false);
  const navigate = useNavigate();

  const handleOpenModal = () => setShowModal(true);
  const handleCloseModal = () => setShowModal(false);

  return (
    <div className="container mx-auto pt-8 h-fit">
      {/* Header com título e botões */}
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold text-green-dark">Lista de {tipo}s</h1>
        
        <div className="flex gap-4 w-fit">
          <BotaoPrimario 
            texto="Cadastrar novo" 
            onClick={handleOpenModal}
          />
          
          <BotaoPrimario 
            texto="Listar inativos" 
            onClick={() => navigate('/usuarios/inativos')}
          />
          
        </div>
      </div>

      {/* Tabela */}
      <Tabela tipo={tipo} lista={lista} />

      {/* Modal de Cadastro (placeholder) */}
      {showModal && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
          <div className="bg-white p-6 rounded-lg w-96">
            <div className="flex justify-between items-center mb-4">
              <h2 className="text-xl font-bold text-green-dark">Cadastrar novo {tipo}</h2>
              <button 
                onClick={handleCloseModal}
                className="text-gray-500 hover:text-gray-700"
              >
                ×
              </button>
            </div>
            
            <div className="space-y-4">
              <p>Formulário de cadastro será implementado aqui...</p>
              
              <div className="flex justify-end gap-3">
                <button
                  onClick={handleCloseModal}
                  className="px-4 py-2 text-gray-600 hover:text-gray-800"
                >
                  Cancelar
                </button>
                <button
                  className="px-4 py-2 bg-green-dark text-white rounded hover:bg-green"
                >
                  Salvar
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default TemplateTabela;