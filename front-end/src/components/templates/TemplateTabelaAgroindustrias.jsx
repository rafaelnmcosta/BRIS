import Tabela from '../organisms/Tabela';
import BotaoPrimario from '../atoms/BotaoPrimario';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../services/AuthContext';

const TemplateTabelaAgroindustrias = ({ tipo, lista, ativos, onAtualizar }) => {
    const { userData } = useAuth();
    const navigate = useNavigate();

    return (
        <div className="container mx-auto pt-8 h-fit">
            {/* Header com título e botões */}
            <div className="flex justify-between items-center mb-6">
                <h1 className="text-2xl font-bold text-green-dark">
                    Lista de {tipo}s {ativos ? '' : 'inativas'}
                </h1>

                <div className="flex gap-4 w-fit">
                    {ativos && (
                        <BotaoPrimario
                            texto="Cadastrar nova"
                            onClick={() => navigate('/agroindustrias/cadastrar')}
                        />
                    )}

                    {userData?.role === 'ADMIN' && (
                        ativos ? (
                            <BotaoPrimario
                                texto="Listar inativas"
                                onClick={() => {
                                    navigate('/agroindustrias/inativas');
                                    onAtualizar();
                                }}
                            />
                        ) : (
                            <BotaoPrimario
                                texto="Listar ativas"
                                onClick={() => {
                                    navigate('/agroindustrias');
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

export default TemplateTabelaAgroindustrias;
