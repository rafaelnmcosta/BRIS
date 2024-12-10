import React from 'react';
import BotaoSecundario from '../atoms/BotaoSecundario';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../services/AuthContext';

const CardInfo = ({ item, tipo }) => {
  const { escolherVinculo } = useAuth();
  const navigate = useNavigate();

  const handleRedirect = async () => {
    switch (tipo) {
      case 'Agroindústria':
        navigate(`/agroindustrias/${item.id}`);
        break;
      case 'Animal':
        navigate(`/animais/${item.id}`);
        break;
      case 'Granja':
        navigate(`/granjas/${item.id}`);
        break;
      case 'Usuário':
        navigate(`/usuarios/${item.id}`);
        break;
      case 'Avaliação':
        navigate(`/avaliacoes/${item.id}`);
        break;
      case 'Vínculo':
        try {
          await escolherVinculo(item.id);
        } catch (error) {
          console.error('Erro ao escolher vínculo:', error);
          alert('Não foi possível selecionar o vínculo. Tente novamente.');
        }
        break;
      default:
        console.warn('Tipo de item desconhecido:', tipo);
        alert('Tipo de item não reconhecido.');
    }
  };

  const renderCardContent = () => {
    switch (tipo) {
      case 'Agroindústria':
        return (
          <>
            <p className='text-green-dark'>Nome Fantasia: {item.nomeFantasia}</p>
            <p className='text-green-dark'>CNPJ: {item.CNPJ}</p>
          </>
        );
      case 'Animal':
        return (
          <>
            <p className='text-green-dark'>Linhagem: {item.linhagem}</p>
            <p className='text-green-dark'>Granja: {item.granja ? item.granja.NomePropriedade : 'N/A'}</p>
          </>
        );
      case 'Granja':
        return (
          <>
            <p className='text-green-dark'>Nome da Propriedade: {item.nomePropriedade}</p>
            <p className='text-green-dark'>CNPJ: {item.CNPJ}</p>
          </>
        );
      case 'Usuário':
        return (
          <>
            <p className='text-green-dark'>Nome: {item.nome}</p>
            <p className='text-green-dark'>Email: {item.email}</p>
          </>
        );
      case 'Avaliação':
        return (
          <>
            <p className='text-green-dark'>Identificação do animal: {item.animalId}</p>
            <p className='text-green-dark'>Status: {item.status}</p>
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
        const role = roleMap[item.role] || 'Desconhecido';

        return (
          <>
            <p className='text-green-dark'>Perfil: {role}</p>
            {item.nomeGranja && <p className='text-green-dark'>Granja: {item.nomeGranja}</p>}
            {item.nomeAgroindustria && <p className='text-green-dark'>Agroindústria: {item.nomeAgroindustria}</p>}
          </>
        );
      default:
        return <p className='text-green-dark'>item desconhecido</p>;
    }
  };

  return (
    <div className="flex items-center justify-between p-4 mb-4 bg-white shadow-lg rounded-lg w-full">
      <div className="flex flex-col space-y-2">
        <h3 className="font-bold text-lg text-green-dark">{tipo}</h3>
        <div className="pl-4">
          {renderCardContent()}
        </div>
      </div>
      <BotaoSecundario texto="Acessar" onClick={handleRedirect} />
    </div>
  );
};

export default CardInfo;
