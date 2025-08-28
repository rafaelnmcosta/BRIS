import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../services/AuthContext';
import BotaoMenu from '../atoms/BotaoMenu';
import BotaoOff from '../atoms/BotaoOff';

const TemplateHome = () => {
    const navigate = useNavigate();
    const { userData, loading } = useAuth();

    const renderBotoes = () => {
        switch (userData.role) {
            case 'ADMIN':
                return (
                    <>
                        <BotaoMenu texto="Gerenciar Usuários" onClick={() => navigate('/usuarios')} />
                        <BotaoMenu texto="Gerenciar Agroindústrias" onClick={() => navigate('/agroindustrias')} />
                        <BotaoMenu texto="Gerenciar Granjas" onClick={() => navigate('/granjas')} />
                        <BotaoMenu texto="Gerenciar Animais" onClick={() => navigate('/animais')} />
                        <BotaoOff texto="Gerenciar Doses" onClick={() => navigate('/doses')} />
                    </>
                );
            case 'GESTOR_AGRO':
                return (
                    <>
                        <BotaoMenu texto="Gerenciar Usuários" onClick={() => navigate('/usuarios')} />
                        <BotaoMenu texto="Gerenciar Granjas" onClick={() => navigate('/granjas')} />
                        <BotaoMenu texto="Gerenciar Animais" onClick={() => navigate('/animais')} />
                    </>
                );
            case 'GESTOR_GRANJA':
                return (
                    <>
                        <BotaoMenu texto="Gerenciar Usuários" onClick={() => navigate('/usuarios')} />
                        <BotaoMenu texto="Gerenciar Animais" onClick={() => navigate('/animais')} />
                        <BotaoOff texto="Gerenciar Doses" onClick={() => navigate('/doses')} />
                    </>
                );
            case 'TECNICO':
                return (
                    <>
                        <BotaoMenu texto="Gerenciar Animais" onClick={() => navigate('/animais')} />
                        <BotaoOff texto="Gerenciar Doses" onClick={() => navigate('/doses')} />
                    </>
                );
            default:
                return <p>Nenhuma ação disponível para o seu tipo de usuário.</p>;
        }
    };

    if (loading) {
        return <div>Carregando...</div>;
    }

    return (
        <div className="pt-8">
            <h1 className="text-2xl font-bold mb-4 text-green-dark">Bem-vindo, {userData.nome}!</h1>
            <div className="flex flex-wrap justify-center gap-4 pt-8">
                {renderBotoes()}
            </div>
        </div>
    );
};

export default TemplateHome;
