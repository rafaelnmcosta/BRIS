import React, { useEffect, useState } from 'react';
import { agroindustrias } from '../api/agroindustriasAPI';
import { useAuth } from '../services/AuthContext';
import TemplateTabelaAgroindustrias from '../components/templates/TemplateTabelaAgroindustrias';

const TabelaAgroindustrias = () => {
    const [agroLista, setAgroLista] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const { isAuthenticated } = useAuth();

    const carregarAgroindustrias = async () => {
        try {
            const dados = await agroindustrias.listarAgroindustrias();
            setAgroLista(dados);
        } catch (err) {
            setError('Falha ao carregar lista de agroindústrias');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        if (isAuthenticated) {
            carregarAgroindustrias();
        }
    }, [isAuthenticated]);


    if (loading) {
        return <p>Carregando agroindústrias...</p>;
    }

    if (error) {
        return <p className="text-red-500">{error}</p>;
    }

    return <TemplateTabelaAgroindustrias tipo="Agroindústria" lista={agroLista} ativos={true} onAtualizar={carregarAgroindustrias}/>;
};

export default TabelaAgroindustrias;
