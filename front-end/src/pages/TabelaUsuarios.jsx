import React, { useEffect, useState } from 'react';
import { usuarios } from '../api/usuariosAPI';
import { useAuth } from '../services/AuthContext';
import TemplateTabelaUsuarios from '../components/templates/TemplateTabelaUsuarios';

const TabelaUsuarios = () => {
    const [usuariosLista, setUsuariosLista] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const { isAuthenticated } = useAuth();

    useEffect(() => {
        const carregarUsuarios = async () => {
            try {
                const dados = await usuarios.listarUsuarios();
                setUsuariosLista(dados);
            } catch (err) {
                setError('Falha ao carregar lista de usuários');
            } finally {
                setLoading(false);
            }
        };

        if (isAuthenticated) {
            carregarUsuarios();
        }
    }, [isAuthenticated]);

    if (loading) {
        return <p>Carregando usuários...</p>;
    }

    if (error) {
        return <p className="text-red-500">{error}</p>;
    }

    return <TemplateTabelaUsuarios tipo="Usuário" lista={usuariosLista} />;
};

export default TabelaUsuarios;