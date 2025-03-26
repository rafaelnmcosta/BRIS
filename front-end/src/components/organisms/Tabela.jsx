import React from 'react';
import { Table } from 'antd';

const Tabela = ({ tipo, lista }) => {
  const gerarColunas = () => {
    switch (tipo) {
      case 'Usuário':
        return [
          {
            title: 'Nome',
            dataIndex: 'nome',
            key: 'nome',
          },
          {
            title: 'Email',
            dataIndex: 'email',
            key: 'email',
          },
          {
            title: 'CPF',
            dataIndex: 'cpf',
            key: 'cpf',
          },
          {
            title: 'Telefone',
            dataIndex: 'telefone',
            key: 'telefone',
          }
        ];
      // adicionar outros casos dpss
      default:
        return [
          {
            title: 'Item',
            dataIndex: 'nome',
            key: 'nome',
          },
          {
            title: 'Ações',
            key: 'acoes',
            render: () => 'Placeholder'
          }
        ];
    }
  };

  const dadosFormatados = lista.map((item, index) => ({
    key: index,
    ...item
  }));

  return (
    <Table
      columns={gerarColunas()}
      dataSource={dadosFormatados}
      className="shadow-lg rounded-lg overflow-hidden"
      bordered
    />
  );
};

export default Tabela;