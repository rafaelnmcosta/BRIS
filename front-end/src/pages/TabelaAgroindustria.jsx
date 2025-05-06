import React, { useEffect, useState } from 'react';
import { agroindustrias } from '../api/agroindustriasAPI';
import { useAuth } from '../services/AuthContext';
import TemplateTabelaAgroindustria from '../components/templates/TemplateTabelaAgroindustrias';

const TabelaAgroindustria = () => {
    const [agroLista, setAgroLista] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const { isAuthenticated } = useAuth();

    useEffect(() => {
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

    return <TemplateTabelaAgroindustria tipo="Agroindústria" lista={agroLista} />;
};

export default TabelaAgroindustria;
