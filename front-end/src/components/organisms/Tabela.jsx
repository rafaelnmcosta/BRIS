import React, { useState } from 'react';
import { Table, Button } from 'antd';
import { EditOutlined, StopOutlined, EyeOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../services/AuthContext';
import ModalConfirmacao from './ModalConfirmacao';
import { usuarios } from '../../api/usuariosAPI';
import { agroindustrias } from '../../api/agroindustriasAPI';

const Tabela = ({ tipo, lista }) => {
  const navigate = useNavigate();
  const [showConfirmacao, setShowConfirmacao] = useState(false);
  const [selectedId, setSelectedId] = useState(null);
  const { userData } = useAuth();

  const handleEditar = (id) => {
    let rota;
    switch (tipo) {
      case 'Agroindústria':
        rota = `/agroindustrias/${id}/editar`;
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
      case 'Usuário':
        rota = `/usuarios/${id}`; break;
      default:
        rota = `/home`

    }
    navigate(rota);
  };

  const handleInativarConfirmacao = (id) => {
    setSelectedId(id);
    setShowConfirmacao(true);
  };

  const handleInativar = async () => {
    try {
      switch (tipo) {
        case 'Agroindústria':
          await agroindustrias.inativarAgroindustria(selectedId);
          break;
        case 'Usuário':
          await usuarios.inativarUsuario(selectedId);
          break;
        default:
          console.warn(`Tipo "${tipo}" não possui lógica de inativação definida.`);
      }
    } finally {
      setShowConfirmacao(false);
    }
  };


  const gerarColunas = () => {
    let colunas = [];

    switch (tipo) {
      case 'Usuário':
        colunas = [
          { title: 'Nome', dataIndex: 'nome', key: 'nome' },
          { title: 'Email', dataIndex: 'email', key: 'email' },
          { title: 'CPF', dataIndex: 'cpf', key: 'cpf' },
        ];
        break;

      case 'Agroindústria':
        colunas = [
          { title: 'Nome Fantasia', dataIndex: 'nomeFantasia', key: 'nomeFantasia' },
          { title: 'CNPJ', dataIndex: 'cnpj', key: 'cnpj' },
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
          <Button
            type="primary"
            icon={<EditOutlined />}
            onClick={() => handleEditar(record.id)}
            className="bg-green-dark hover:bg-green text-white"
            aria-label="Editar"
            title="Editar"
          />
          {!(tipo === 'Usuário' && userData.role !== 'ADMIN') && (
            <Button
              danger
              icon={<StopOutlined />}
              onClick={() => handleInativarConfirmacao(record.id)}
              className="hover:bg-red-100"
              aria-label="Inativar"
              title="Inativar"
            />
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
        onClose={() => setShowConfirmacao(false)}
        onConfirm={handleInativar}
        title="Confirmar Inativação"
        content={`Tem certeza que deseja inativar esse/a ${tipo.toLowerCase()}?`}
        okText="Inativar"
        danger={true}
      />
    </>
  );
};

export default Tabela;
