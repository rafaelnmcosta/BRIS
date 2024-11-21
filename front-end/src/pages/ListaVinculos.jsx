import React, { useEffect, useState } from 'react';
import TemplateListaVinculos from '../components/templates/TemplateListaVinculos';
import { autenticacao } from '../api/autenticacaoAPI';

const ListaVinculos = () => {
    const [vinculos, setVinculos] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchVinculos = async () => {
            try {
                const response = await autenticacao.listarVinculos();
                setVinculos(response);
            } catch (err) {
                setError('Erro ao carregar os vínculos.');
            } finally {
                setLoading(false);
            }
        };

        fetchVinculos();
    }, []);

    if (loading) {
        return <p>Carregando vínculos...</p>;
    }

    if (error) {
        return <p className="text-red-500">{error}</p>;
    }

    return <TemplateListaVinculos vinculos={vinculos} />;
};

export default ListaVinculos;
