import React, { useEffect, useState } from 'react';
import { animais } from '../api/animaisAPI';
import { useAuth } from '../services/AuthContext';
import TemplateTabelaAnimais from '../components/templates/TemplateTabelaAnimais';

const TabelaAnimais = () => {
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
                    dados = await animais.listarAtivos();
                    break;
                case 'GESTOR_AGRO':
                    dados = await animais.listarAtivosPorAgroindustria(userData.agroindustriaId);
                    break;
                case 'GESTOR_GRANJA':
                case 'TECNICO':
                    dados = await animais.listarAtivosPorGranja(userData.granjaId);
                    break;
                default:
                    dados = [];
            }
            setAnimalLista(dados);
        } catch (err) {
            setError('Falha ao carregar lista de animais');
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
        return <p>Carregando animais...</p>;
    }

    if (error) {
        return <p className="text-red-500">{error}</p>;
    }

    return (
        <TemplateTabelaAnimais
            tipo="Animal"
            lista={animalLista}
            ativos={true}
            onAtualizar={carregarAnimais}
        />
    );
};

export default TabelaAnimais;
