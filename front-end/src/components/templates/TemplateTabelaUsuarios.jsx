import Tabela from '../organisms/Tabela';
import BotaoPrimario from '../atoms/BotaoPrimario';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../services/AuthContext';

const TemplateTabelaUsuarios = ({ tipo, lista, ativos, onAtualizar }) => {
  const { userData } = useAuth();
  const navigate = useNavigate();

  return (
    <div className="container mx-auto pt-8 h-fit">
      {/* Header com título e botões */}
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold text-green-dark">
          Lista de {tipo}s {ativos ? '' : 'inativos'}
        </h1>

        <div className="flex gap-4 w-fit">
          {ativos && (
            <BotaoPrimario
              texto="Cadastrar novo"
              onClick={() => navigate('/usuarios/cadastrar')}
            />
          )}

          {userData?.role === 'ADMIN' && (
            ativos ? (
              <BotaoPrimario
                texto="Listar inativos"
                onClick={() => {
                  navigate('/usuarios/inativos');
                  onAtualizar();
                }}
              />
            ) : (
              <BotaoPrimario
                texto="Listar ativos"
                onClick={() => {
                  navigate('/usuarios');
                  onAtualizar();
                }}
              />
            )
          )}
        </div>
      </div>

      {/* Tabela */}
      <Tabela tipo={tipo} lista={lista} ativos={ativos} onAtualizar={onAtualizar} />
    </div>
  );
};

export default TemplateTabelaUsuarios;
