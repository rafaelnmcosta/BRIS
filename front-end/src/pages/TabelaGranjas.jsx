import React, { useEffect, useState } from 'react';
import { granjas } from '../api/granjasAPI';
import { useAuth } from '../services/AuthContext';
import TemplateTabelaGranjas from '../components/templates/TemplateTabelaGranjas';

const TabelaGranjas = () => {
    const [agroLista, setAgroLista] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const { isAuthenticated, userData } = useAuth();
    console.log(userData)

    const carregarGranjas = async () => {
        if (userData.role === 'ADMIN'){
            try {
                const dados = await granjas.listarGranjas();
                setAgroLista(dados);
            } catch (err) {
                setError('Falha ao carregar lista de granjas');
            } finally {
                setLoading(false);
            }
        }
        else{
            try {
                const dados = await granjas.listarPorAgroindustria(userData.agroindustriaId);
                setAgroLista(dados);
            } catch (err) {
                setError('Falha ao carregar lista de granjas');
            } finally {
                setLoading(false);
            }
        }
    };

    useEffect(() => {
        if (isAuthenticated) {
            carregarGranjas();
        }
    }, [isAuthenticated]);


    if (loading) {
        return <p>Carregando granjas...</p>;
    }

    if (error) {
        return <p className="text-red-500">{error}</p>;
    }

    return <TemplateTabelaGranjas tipo="Granja" lista={agroLista} ativos={true} onAtualizar={carregarGranjas}/>;
};

export default TabelaGranjas;
