import React from 'react';
import BotaoSecundario from '../atoms/BotaoSecundario';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../services/AuthContext';

const CardInfo = ({ entidade, tipoEntidade }) => {
  const { escolherVinculo } = useAuth();
  const navigate = useNavigate();

  const handleRedirect = async () => {
    switch (tipoEntidade) {
      case 'Agroindústria':
        navigate(`/agroindustrias/${entidade.id}`);
        break;
      case 'Animal':
        navigate(`/animais/${entidade.id}`);
        break;
      case 'Granja':
        navigate(`/granjas/${entidade.id}`);
        break;
      case 'Usuário':
        navigate(`/usuarios/${entidade.id}`);
        break;
      case 'Avaliação':
        navigate(`/avaliacoes/${entidade.id}`);
        break;
      case 'Vínculo':
        try {
          await escolherVinculo(entidade.id);
        } catch (error) {
          console.error('Erro ao escolher vínculo:', error);
          alert('Não foi possível selecionar o vínculo. Tente novamente.');
        }
        break;
      default:
        console.warn('Tipo de entidade desconhecido:', tipoEntidade);
        alert('Tipo de entidade não reconhecido.');
    }
  };

  const renderCardContent = () => {
    switch (tipoEntidade) {
      case 'Agroindústria':
        return (
          <>
            <p className='text-green-dark'>Nome Fantasia: {entidade.nomeFantasia}</p>
            <p className='text-green-dark'>CNPJ: {entidade.CNPJ}</p>
          </>
        );
      case 'Animal':
        return (
          <>
            <p className='text-green-dark'>Linhagem: {entidade.linhagem}</p>
            <p className='text-green-dark'>Granja: {entidade.granja ? entidade.granja.NomePropriedade : 'N/A'}</p>
          </>
        );
      case 'Granja':
        return (
          <>
            <p className='text-green-dark'>Nome da Propriedade: {entidade.nomePropriedade}</p>
            <p className='text-green-dark'>CNPJ: {entidade.CNPJ}</p>
          </>
        );
      case 'Usuário':
        return (
          <>
            <p className='text-green-dark'>Nome: {entidade.nome}</p>
            <p className='text-green-dark'>Email: {entidade.email}</p>
          </>
        );
      case 'Avaliação':
        return (
          <>
            <p className='text-green-dark'>Identificação do animal: {entidade.animalId}</p>
            <p className='text-green-dark'>Status: {entidade.status}</p>
          </>
        );
      case 'Vínculo':
        const roleMap = {
          ADMIN: 'Administrador',
          GESTOR_AGRO: 'Gestor de Agroindústria',
          GESTOR_GRANJA: 'Gestor de Granja',
          TECNICO: 'Técnico',
          VISUALIZADOR: 'Visualizador',
        };
        const role = roleMap[entidade.role] || 'Desconhecido';

        return (
          <>
            <p className='text-green-dark'>Perfil: {role}</p>
            {entidade.nomeGranja && <p className='text-green-dark'>Granja: {entidade.nomeGranja}</p>}
            {entidade.nomeAgroindustria && <p className='text-green-dark'>Agroindústria: {entidade.nomeAgroindustria}</p>}
          </>
        );
      default:
        return <p className='text-green-dark'>Entidade desconhecida</p>;
    }
  };

  return (
    <div className="flex items-center justify-between p-4 mb-4 bg-white shadow-lg rounded-lg w-full">
      <div className="flex flex-col space-y-2">
        <h3 className="font-bold text-lg text-green-dark">{tipoEntidade}</h3>
        <div className="pl-4">
          {renderCardContent()}
        </div>
      </div>
      <BotaoSecundario texto="Acessar" onClick={handleRedirect} />
    </div>
  );
};

export default CardInfo;
