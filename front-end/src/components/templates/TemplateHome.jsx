import React from 'react';
import { useNavigate } from 'react-router-dom';
import BotaoMenu from '../atoms/BotaoMenu';
import BotaoOff from '../atoms/BotaoOff';

const TemplateHome = ({ userType }) => {
    const navigate = useNavigate();

    const renderBotoes = () => {
        switch (userType) {
            case 'ADMIN':
                return (
                    <>
                        <BotaoMenu texto="Gerenciar Usuários" onClick={() => navigate('/usuarios')} />
                        <BotaoOff texto="Gerenciar Agroindústrias" onClick={() => navigate('/agroindustrias')} />
                        <BotaoOff texto="Gerenciar Granjas" onClick={() => navigate('/granjas')} />
                        <BotaoOff texto="Gerenciar Animais" onClick={() => navigate('/animais')} />
                        <BotaoOff texto="Gerenciar Doses" onClick={() => navigate('/doses')} />
                    </>
                );
            case 'GESTOR_AGRO':
                return (
                    <>
                        <BotaoMenu texto="Gerenciar Usuários" onClick={() => navigate('/usuarios')} />
                        <BotaoOff texto="Gerenciar Granjas" onClick={() => navigate('/granjas')} />
                        <BotaoOff texto="Gerenciar Animais" onClick={() => navigate('/animais')} />
                    </>
                );
            case 'GESTOR_GRANJA':
                return (
                    <>
                        <BotaoMenu texto="Gerenciar Usuários" onClick={() => navigate('/usuarios')} />
                        <BotaoOff texto="Gerenciar Animais" onClick={() => navigate('/animais')} />
                        <BotaoOff texto="Gerenciar Doses" onClick={() => navigate('/doses')} />
                    </>
                );
            case 'TECNICO':
                return (
                    <>
                        <BotaoOff texto="Gerenciar Animais" onClick={() => navigate('/animais')} />
                        <BotaoOff texto="Gerenciar Doses" onClick={() => navigate('/doses')} />
                    </>
                );
            default:
                return <p>Nenhuma ação disponível para o seu tipo de usuário.</p>;
        }
    };

    const renderBemVindo = () => {
        switch (userType) {
            case 'ADMIN':
                return 'Bem-vindo, Administrador!';
            case 'GESTOR_AGRO':
                return 'Bem-vindo, Gestor de Agroindústria!';
            case 'GESTOR_GRANJA':
                return 'Bem-vindo, Gestor de Granja!';
            case 'TECNICO':
                return 'Bem-vindo, Técnico!';
            default:
                return 'Bem-vindo! (sem role)';
        }
    };

    return (
        <div className="pt-8">
            <h1 className="text-2xl font-bold mb-4 text-green-dark">{renderBemVindo()}</h1>
            <div className="flex flex-wrap justify-center gap-4 pt-8">
                {renderBotoes()}
            </div>
        </div>
    );
};

export default TemplateHome;
