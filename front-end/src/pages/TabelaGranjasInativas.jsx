import React, { useEffect, useState } from 'react';
import { granjas } from '../api/granjasAPI';
import { useAuth } from '../services/AuthContext';
import TemplateTabelaGranjas from '../components/templates/TemplateTabelaGranjas';
import { Spin } from 'antd';

const TabelaGranjasInativas = () => {
    const [granjaLista, setGranjaLista] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const { isAuthenticated } = useAuth();

    const carregarGranjas = async () => {
        try {
            const dados = await granjas.listarGranjasInativas();
            setGranjaLista(dados);
        } catch (err) {
            setError('Falha ao carregar lista de granjas inativas');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        if (isAuthenticated) {
            carregarGranjas();
        }
    }, [isAuthenticated]);

    if (loading) {
        return (
            <div className="flex justify-center items-center h-screen">
                <Spin size="large" tip="Carregando granjas inativas..." />
            </div>
        );
    }

    if (error) {
        return <p className="text-red-500">{error}</p>;
    }

    return (
        <TemplateTabelaGranjas
            tipo="Granja"
            lista={granjaLista}
            ativos={false}
            onAtualizar={carregarGranjas}
        />
    );
};

export default TabelaGranjasInativas;
