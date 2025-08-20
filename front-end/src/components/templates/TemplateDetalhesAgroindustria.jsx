import React from 'react';
import { useNavigate } from 'react-router-dom';
import BotaoPrimario from '../atoms/BotaoPrimario';
import { formatarCNPJ, formatarTelefone, formatarData } from '../../services/Formatter';
import { agroindustrias } from '../../api/agroindustriasAPI';

const TemplateDetalhesAgroindustria = ({ dados }) => {
  const navigate = useNavigate();

  return (
    <div className="container mx-auto pt-8 h-fit">
      {/* Header */}
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold text-green-dark">
          Detalhes da Agroindústria
        </h1>
        <div className="flex gap-4">
          <BotaoPrimario
            texto="Editar"
            onClick={() => navigate(`/agroindustrias/${dados.id}/editar`)}
          />
          <BotaoPrimario
            texto={dados.ativo ? 'Inativar' : 'Ativar'}
            onClick={async () => {
              try {
                if (dados.ativo) {
                  await agroindustrias.desativarAgroindustria(dados.id);
                } else {
                  await agroindustrias.ativarAgroindustria(dados.id);
                }
                navigate(dados.ativo ? `/agroindustrias/inativas` : `/agroindustrias`);
              } catch (error) {
                console.error('Erro ao alterar status da agroindústria:', error);
              }
            }}
          />
        </div>
      </div>

      {/* Conteúdo */}
      <div className="bg-white shadow-lg rounded-lg p-8 grid grid-cols-1 md:grid-cols-2 gap-y-6 gap-x-16 text-green-dark">
        <div>
          <p className="text-sm text-gray-500">Nome Fantasia</p>
          <p className="text-base font-medium">{dados.nomeFantasia}</p>
        </div>

        <div>
          <p className="text-sm text-gray-500">Razão Social</p>
          <p className="text-base font-medium">{dados.razaoSocial}</p>
        </div>

        <div>
          <p className="text-sm text-gray-500">CNPJ</p>
          <p className="text-base font-medium">{formatarCNPJ(dados.cnpj)}</p>
        </div>

        <div>
          <p className="text-sm text-gray-500">Endereço</p>
          <p className="text-base font-medium">{dados.endereco}</p>
        </div>

        <div>
          <p className="text-sm text-gray-500">E-mail</p>
          <p className="text-base font-medium">{dados.email}</p>
        </div>

        <div>
          <p className="text-sm text-gray-500">Telefone</p>
          <p className="text-base font-medium">{formatarTelefone(dados.telefone)}</p>
        </div>

        <div>
          <p className="text-sm text-gray-500">Data de Cadastro</p>
          <p className="text-base font-medium">{formatarData(dados.dataCadastro)}</p>
        </div>

        <div>
          <p className="text-sm text-gray-500">Status</p>
          <p className="text-base font-medium">
            {dados.ativo ? 'Ativa' : 'Inativa'}
          </p>
        </div>
      </div>
    </div>
  );
};

export default TemplateDetalhesAgroindustria;
