import React, { useEffect, useState } from 'react';
import { animais } from '../api/animaisAPI';
import { useAuth } from '../services/AuthContext';
import TemplateTabelaAnimais from '../components/templates/TemplateTabelaAnimais';
import { Spin } from 'antd';

const TabelaAnimaisInativos = () => {
    const [animalLista, setAnimalLista] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const { isAuthenticated, userData } = useAuth();

    const carregarAnimais = async () => {
        setLoading(true);
        try {
            let dados = [];
            switch (userData.role) {
                case 'ADMIN':
                    dados = await animais.listarInativos();
                    break;
                case 'GESTOR_AGRO':
                    dados = await animais.listarInativosPorAgroindustria(userData.agroindustriaId);
                    break;
                case 'GESTOR_GRANJA':
                case 'TECNICO':
                    dados = await animais.listarInativosPorGranja(userData.granjaId);
                    break;
                default:
                    dados = [];
            }
            setAnimalLista(dados);
        } catch (err) {
            setError('Falha ao carregar lista de animais inativos');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        if (isAuthenticated) {
            carregarAnimais();
        }
    }, [isAuthenticated]);

    if (loading) {
        return (
            <div className="flex justify-center items-center h-screen">
                <Spin size="large" tip="Carregando animais inativos..." />
            </div>
        );
    }

    if (error) {
        return <p className="text-red-500">{error}</p>;
    }

    return (
        <TemplateTabelaAnimais
            tipo="Animal"
            lista={animalLista}
            ativos={false}
            onAtualizar={carregarAnimais}
        />
    );
};

export default TabelaAnimaisInativos;
