import React from 'react';
import { Card } from 'antd';

const CardInfo = ({ entidade, tipoEntidade }) => {
  console.log('entidade:', entidade);
  const renderCardContent = () => {
    switch (tipoEntidade) {
      case 'Agroindustria':
        return (
          <>
            <p>Nome Fantasia: {entidade.NomeFantasia}</p>
            <p>CNPJ: {entidade.CNPJ}</p>
          </>
        );
      case 'Animal':
        return (
          <>
            <p>Linhagem: {entidade.Linhagem}</p>
            <p>Granja: {entidade.Granja ? entidade.Granja.NomePropriedade : 'N/A'}</p>
          </>
        );
      case 'Granja':
        return (
          <>
            <p>Nome da Propriedade: {entidade.NomePropriedade}</p>
            <p>CNPJ: {entidade.CNPJ}</p>
          </>
        );
      case 'Usuario':
        return (
          <>
            <p>Nome: {entidade.Nome}</p>
            <p>Email: {entidade.Email}</p>
          </>
        );
      case 'Avaliacao':
        return (
          <>
            <p>Animal ID: {entidade.AnimalId}</p>
            <p>Status: {entidade.Status}</p>
          </>
        );
      case 'Vinculo':
        return (
          <>
            <p>Tipo de Usuário: {entidade.role.toLowerCase()}</p>
            <p>Granja: {entidade.nomeGranja ? entidade.nomeGranja : ''}</p>
            <p>Agroindústria: {entidade.nomeAgroindustria ? entidade.nomeAgroindustria : ''}</p>
          </>
        );
      default:
        return <p>entidade desconhecida</p>;
    }
  };

  return (
    <Card
      title={`${tipoEntidade}`}
      className="shadow-lg hover:shadow-2xl transition-all duration-300"
    >
      {renderCardContent()}
    </Card>
  );
};

export default CardInfo;
