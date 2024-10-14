import React from 'react';

const DetalhesEntidade = ({ entidade, tipoEntidade }) => {
  const renderDetalhes = () => {
    switch (tipoEntidade) {
      case 'Agroindustria':
        return (
          <>
            <h3>Agroindústria de id: {entidade.ID}</h3>
            <p><strong>Nome Fantasia:</strong> {entidade.NomeFantasia}</p>
            <p><strong>Razão Social:</strong> {entidade.RazaoSocial}</p>
            <p><strong>CNPJ:</strong> {entidade.CNPJ}</p>
            <p><strong>Ativo:</strong> {entidade.Ativo ? 'Sim' : 'Não'}</p>
          </>
        );
      case 'Animal':
        return (
          <>
            <h3>Animal de id: {entidade.ID}</h3>
            <p><strong>Linhagem:</strong> {entidade.Linhagem}</p>
            <p><strong>Idade:</strong> {entidade.Idade}</p>
            <p><strong>Peso:</strong> {entidade.Peso} kg</p>
            <p><strong>Status:</strong> {entidade.Status ? 'Saudável' : 'Doente'}</p>
            <p><strong>Granja:</strong> {entidade.Granja?.NomePropriedade || 'Não associado a uma granja'}</p>
            <p><strong>Usuário Responsável:</strong> {entidade.UsuarioResponsavel?.Nome || 'Não atribuído'}</p>
            <p><strong>Ativo:</strong> {entidade.Ativo ? 'Sim' : 'Não'}</p>
          </>
        );
      case 'Granja':
        return (
          <>
            <h3>Granja de id: {entidade.ID}</h3>
            <p><strong>Nome da Propriedade:</strong> {entidade.NomePropriedade}</p>
            <p><strong>Agroindústria:</strong> {entidade.Agroindustria?.NomeFantasia || 'Não associado a uma agroindústria'}</p>
            <p><strong>Endereço:</strong> {entidade.Endereco}</p>
            <p><strong>CNPJ:</strong> {entidade.CNPJ}</p>
            <p><strong>Ativo:</strong> {entidade.Ativo ? 'Sim' : 'Não'}</p>
          </>
        );
      case 'Usuario':
        return (
          <>
            <h3>Usuário de id: {entidade.ID}</h3>
            <p><strong>Nome:</strong> {entidade.Nome}</p>
            <p><strong>Email:</strong> {entidade.Email}</p>
            <p><strong>CPF:</strong> {entidade.CPF}</p>
            <p><strong>Agroindústria:</strong> {entidade.Agroindustria?.NomeFantasia || 'Não associado a uma agroindústria'}</p>
          </>
        );
      default:
        return <p>Tipo de entidade desconhecido.</p>;
    }
  };

  return (
    <div className="detalhes-entidade">
      {renderDetalhes()}
    </div>
  );
};

export default DetalhesEntidade;
