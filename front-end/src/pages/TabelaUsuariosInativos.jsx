import React, { useEffect, useState } from 'react';
import { usuarios } from '../api/usuariosAPI';
import { useAuth } from '../services/AuthContext';
import TemplateTabelaUsuarios from '../components/templates/TemplateTabelaUsuarios';

const TabelaUsuariosInativos = () => {
    const [usuariosLista, setUsuariosLista] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const { isAuthenticated } = useAuth();

    const carregarUsuarios = async () => {
        try {
            const dados = await usuarios.listarUsuariosInativos();
            setUsuariosLista(dados);
        } catch (err) {
            setError('Falha ao carregar lista de usuários inativos');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        if (isAuthenticated) {
            carregarUsuarios();
        }
    }, [isAuthenticated]);

    if (loading) {
        return <p>Carregando usuários inativos...</p>;
    }

    if (error) {
        return <p className="text-red-500">{error}</p>;
    }

    return (
        <TemplateTabelaUsuarios
            tipo="Usuário"
            lista={usuariosLista}
            ativos={false}
            onAtualizar={carregarUsuarios}
        />
    );
};

export default TabelaUsuariosInativos;
