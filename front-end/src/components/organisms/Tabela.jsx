import React, { useState } from 'react';
import { Table, Button } from 'antd';
import { EditOutlined, StopOutlined, EyeOutlined, CheckOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import { formatarCPF, formatarCNPJ } from '../../services/Formatter';
import { useAuth } from '../../services/AuthContext';
import ModalConfirmacao from './ModalConfirmacao';
import { usuarios } from '../../api/usuariosAPI';
import { agroindustrias } from '../../api/agroindustriasAPI';
import { granjas } from '../../api/granjasAPI';

const Tabela = ({ tipo, lista, ativos, onAtualizar }) => {
  const [selectedId, setSelectedId] = useState(null);
  const [showConfirmacao, setShowConfirmacao] = useState(false);
  const [acaoConfirmacao, setAcaoConfirmacao] = useState(null); // 'inativar' ou 'ativar'
  const navigate = useNavigate();
  const { userData } = useAuth();

  const handleEditar = (id) => {
    let rota;
    switch (tipo) {
      case 'Agroindústria':
        rota = `/agroindustrias/${id}/editar`;
        break;
      case 'Granja':
        rota = `/granjas/${id}/editar`;
        break;
      case 'Usuário':
        rota = `/usuarios/${id}/editar`;
        break;
      default:
        rota = `/home`
    }
    navigate(rota);
  };

  const handleDetalhes = (id) => {
    let rota;
    switch (tipo) {
      case 'Agroindústria':
        rota = `/agroindustrias/${id}`;
        break;
      case 'Granja':
        rota = `/granjas/${id}`;
        break;
      case 'Usuário':
        rota = `/usuarios/${id}`; break;
      default:
        rota = `/home`

    }
    navigate(rota);
  };

  const handleInativarConfirmacao = (id) => {
    setSelectedId(id);
    setAcaoConfirmacao('inativar');
    setShowConfirmacao(true);
  };

  const handleAtivarConfirmacao = (id) => {
    setSelectedId(id);
    setAcaoConfirmacao('ativar');
    setShowConfirmacao(true);
  };


  const handleConfirmacao = async () => {
    try {
      switch (tipo) {
        case 'Agroindústria':
          if (acaoConfirmacao === 'inativar') {
            await agroindustrias.desativarAgroindustria(selectedId);
          } else {
            await agroindustrias.ativarAgroindustria(selectedId);
          }
          break;
        case 'Granja':
          if (acaoConfirmacao === 'inativar') {
            await granjas.desativarGranja(selectedId);
          } else {
            await granjas.ativarGranja(selectedId);
          }
          break;
        case 'Usuário':
          if (acaoConfirmacao === 'inativar') {
            await usuarios.inativarUsuario(selectedId);
          } else {
            navigate(`/usuarios/reativar/${selectedId}`);
          }
          break;
        default:
          console.warn(`Tipo "${tipo}" não possui lógica para ${acaoConfirmacao}.`);
      }

      if (onAtualizar) await onAtualizar();

    } finally {
      setShowConfirmacao(false);
      setSelectedId(null);
      setAcaoConfirmacao(null);
    }
  };


  const gerarColunas = () => {
    let colunas = [];

    switch (tipo) {
      case 'Usuário':
        colunas = [
          { title: 'Nome', dataIndex: 'nome', key: 'nome' },
          { title: 'Email', dataIndex: 'email', key: 'email' },
          { title: 'CPF', dataIndex: 'cpf', key: 'cpf', render: (cpf) => formatarCPF(cpf) || 'Não informado' }
        ];
        break;

      case 'Agroindústria':
        colunas = [
          { title: 'Nome Fantasia', dataIndex: 'nomeFantasia', key: 'nomeFantasia' },
          { title: 'CNPJ', dataIndex: 'cnpj', key: 'cnpj', render: (cnpj) => formatarCNPJ(cnpj) || 'Não informado' },
        ];
        break;

      case 'Granja':
        colunas = [
          { title: 'Nome da propriedade', dataIndex: 'nomePropriedade', key: 'nomePropriedade' },
          { title: 'CNPJ', dataIndex: 'cnpj', key: 'cnpj', render: (cnpj) => formatarCNPJ(cnpj) || 'Não informado' },
        ];
        break;

      default:
        colunas = [
          { title: 'Item', dataIndex: 'nome', key: 'nome' },
          { title: 'Ações', key: 'acoes', render: () => 'Placeholder' },
        ];
    }

    colunas.push({
      title: 'Ações',
      key: 'acoes',
      width: 150,
      align: 'center',
      render: (_, record) => (
        <div className="flex gap-2 justify-center">
          <Button
            icon={<EyeOutlined />}
            onClick={() => handleDetalhes(record.id)}
            className="hover:bg-gray-100 border-gray-300"
            aria-label="Ver detalhes"
            title="Ver detalhes"
          />
          {ativos && (
            <Button
              type="primary"
              icon={<EditOutlined />}
              onClick={() => handleEditar(record.id)}
              className="bg-green-dark hover:bg-green text-white"
              aria-label="Editar"
              title="Editar"
            />
          )}

          {!(tipo === 'Usuário' && userData.role !== 'ADMIN') && (
            ativos ? (
              <Button
                danger
                icon={<StopOutlined />}
                onClick={() => handleInativarConfirmacao(record.id)}
                className="hover:bg-red-100"
                aria-label="Inativar"
                title="Inativar"
              />
            ) : (
              <Button
                type="default"
                icon={<CheckOutlined />}
                onClick={() => handleAtivarConfirmacao(record.id)}
                className="hover:bg-green-100 border-green-500 text-green-dark"
                aria-label="Ativar"
                title="Ativar"
              />
            )
          )}
        </div>
      ),
    });

    return colunas;
  };

  return (
    <>
      <Table
        columns={gerarColunas()}
        dataSource={lista.map((item, index) => ({ key: index, ...item }))}
        className="shadow-lg rounded-lg overflow-hidden"
        bordered
      />

      <ModalConfirmacao
        open={showConfirmacao}
        onClose={() => {
          setShowConfirmacao(false);
          setAcaoConfirmacao(null);
          setSelectedId(null);
        }}
        onConfirm={handleConfirmacao}
        title={`Confirmar ${acaoConfirmacao === 'ativar' ? 'Ativação' : 'Inativação'}`}
        content={`Tem certeza que deseja ${acaoConfirmacao === 'ativar' ? 'ativar' : 'inativar'} esse/a ${tipo.toLowerCase()}?`}
        okText={acaoConfirmacao === 'ativar' ? 'Ativar' : 'Inativar'}
        danger={acaoConfirmacao !== 'ativar'}
      />
    </>
  );
};

export default Tabela;
